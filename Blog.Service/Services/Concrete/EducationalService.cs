using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Educationals;
using Blog.Service.Extensions;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Service.Services.Concrete
{
    public class EducationalService : IEducationalService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageHelper imageHelper;
        private readonly IUserService userService;
        private readonly ClaimsPrincipal _user;

        public EducationalService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
            this.imageHelper = imageHelper;
            this.userService = userService;
        }

        public async Task<VMEducationalList> GetAllEducationalByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 6, bool isAscending = false)
        {

            pageSize = pageSize > 20 ? 20 : pageSize;

            var educationals = categoryId == null
                ? await unitOfWork.GetRepository<Educational>().GetAllAsync(a => !a.IsDeleted, a => a.Category, i => i.Image, u => u.User)
                : await unitOfWork.GetRepository<Educational>().GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted, x => x.Category, i => i.Image, u => u.User);

            var sortedEducationals = isAscending
                ? educationals.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : educationals.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new VMEducationalList
            {
                Educationals = sortedEducationals,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = educationals.Count,
                IsAscending = isAscending
            };

        }


        public async Task CreateEducationalAsync(VMEducationalAdd vmEducationalAdd)
        {
            var userId = _user.GetLoggedInUserId();
            var useremail = _user.GetLoggedInEmail();

            if (vmEducationalAdd.Photo == null)
            {
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "educational-images", "default-image.jpg");

                byte[] imageBytes = await File.ReadAllBytesAsync(defaultImagePath);
                var stream = new MemoryStream(imageBytes);

                vmEducationalAdd.Photo = new FormFile(stream, 0, stream.Length, "image", "default.jpg")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpg"
                };
            }

            var imageUpload = await imageHelper.Upload(vmEducationalAdd.Title, vmEducationalAdd.Photo, ImageType.Post);
            Image image = new Image(imageUpload.FullName, vmEducationalAdd.Photo.ContentType, useremail);

            await unitOfWork.GetRepository<Image>().AddAsync(image);

            var user = await userService.GetAppUserByIdAsync(userId);

            var educational = new Educational(vmEducationalAdd.Title, vmEducationalAdd.Content, userId, user, useremail, vmEducationalAdd.CategoryId, image.Id);
            await unitOfWork.GetRepository<Educational>().AddAsync(educational);

            await unitOfWork.SaveAsync();
        }

        public async Task<List<VMEducational>> GetAllEducationalsWithCategoryNonDeletedAsync()
        {

            var educationals = await unitOfWork.GetRepository<Educational>().GetAllAsync(x => !x.IsDeleted, x => x.Category, x => x.Image);
            var map = mapper.Map<List<VMEducational>>(educationals);

            return map;
        }
        public async Task<VMEducational> GetEducationalsWithCategoryNonDeletedAsync(Guid educationalId)
        {

            var educational = await unitOfWork.GetRepository<Educational>().GetAsync(x => !x.IsDeleted && x.Id == educationalId, x => x.Category, i => i.Image, u => u.User);
            var map = mapper.Map<VMEducational>(educational);

            return map;
        }
        public async Task<string> UpdateEducationalAsync(VMEducationalUpdate vmEducationalUpdate)
        {
            var useremail = _user.GetLoggedInEmail();
            var educational = await unitOfWork.GetRepository<Educational>().GetAsync(x => !x.IsDeleted & x.Id == vmEducationalUpdate.Id, x => x.Category, i => i.Image);

            if (vmEducationalUpdate.Photo != null)
            {
                imageHelper.Delete(educational.Image.FileName);

                var imageUpload = await imageHelper.Upload(vmEducationalUpdate.Title, vmEducationalUpdate.Photo, ImageType.Post);
                Image image = new(imageUpload.FullName, vmEducationalUpdate.Photo.ContentType, useremail);
                await unitOfWork.GetRepository<Image>().AddAsync(image);

                educational.ImageId = image.Id;
            }

            educational.Title = vmEducationalUpdate.Title;
            educational.Content = vmEducationalUpdate.Content;
            educational.CategoryId = vmEducationalUpdate.CategoryId;
            educational.ModifiedDate = DateTime.Now;
            educational.ModifiedBy = useremail;


            await unitOfWork.GetRepository<Educational>().UpdateAsync(educational);
            await unitOfWork.SaveAsync();

            return educational.Title;
        }

        public async Task<string> SafeDeleteEducationalAsync(Guid educationalId)
        {
            var userEmail = _user.GetLoggedInEmail();
            var educational = await unitOfWork.GetRepository<Educational>().GetByGuidAsync(educationalId);

            educational.IsDeleted = true;
            educational.DeletedDate = DateTime.Now;
            educational.DeletedBy = userEmail;

            await unitOfWork.GetRepository<Educational>().UpdateAsync(educational);
            await unitOfWork.SaveAsync();

            return educational.Title;
        }

        public async Task<List<VMEducational>> GetAllEducationalsWithCategoryDeletedAsync()
        {
            var educational = await unitOfWork.GetRepository<Educational>().GetAllAsync(x => x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<VMEducational>>(educational);

            return map;
        }

        public async Task<string> UndoDeleteEducationalAsync(Guid educationalId)
        {
            var educational = await unitOfWork.GetRepository<Educational>().GetByGuidAsync(educationalId);

            educational.IsDeleted = false;
            educational.DeletedDate = null;
            educational.DeletedBy = null;

            await unitOfWork.GetRepository<Educational>().UpdateAsync(educational);
            await unitOfWork.SaveAsync();

            return educational.Title;
        }

        public async Task<bool> EducationalVisitorCheckerAsync(Guid educationalId)
        {
            var ipAdress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var educationalVisitors = await unitOfWork.GetRepository<EducationalVisitor>().GetAllAsync(null, x => x.Visitor, y => y.Educational);
            var educational = await unitOfWork.GetRepository<Blog.Entity.Entities.Educational>().GetAsync(x => x.Id == educationalId);

            var visitor = await unitOfWork.GetRepository<Visitor>().GetAsync(x => x.IpAddress == ipAdress);

            var addEducationalVisitors = new EducationalVisitor(educational.Id, visitor.Id);

            if (educationalVisitors.Any(x => x.VisitorId == addEducationalVisitors.VisitorId && x.EducationalId == addEducationalVisitors.EducationalId))
            {
                return true;
            }
            else
            {
                await unitOfWork.GetRepository<EducationalVisitor>().AddAsync(addEducationalVisitors);
                educational.ViewCount += 1;
                await unitOfWork.GetRepository<Educational>().UpdateAsync(educational);
                await unitOfWork.SaveAsync();

                return false;
            }
        }



    }
}
