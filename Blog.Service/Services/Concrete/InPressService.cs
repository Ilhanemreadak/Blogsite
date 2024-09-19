using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Service.Extensions;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Blog.Entity.ViewModels.InPresses;

namespace Blog.Service.Services.Concrete
{
    public class InPressService : IInPressService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageHelper imageHelper;
        private readonly IUserService userService;
        private readonly ClaimsPrincipal _user;

        public InPressService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
            this.imageHelper = imageHelper;
            this.userService = userService;
        }

        public async Task<VMInPressList> GetAllInPressByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 6, bool isAscending = false)
        {

            pageSize = pageSize > 20 ? 20 : pageSize;

            var inpresses = categoryId == null
                ? await unitOfWork.GetRepository<InPress>().GetAllAsync(a => !a.IsDeleted, a => a.Category, i => i.Image, u => u.User)
                : await unitOfWork.GetRepository<InPress>().GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted, x => x.Category, i => i.Image, u => u.User);

            var sortedInpresses = isAscending
                ? inpresses.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : inpresses.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new VMInPressList
            {
                InPresses = sortedInpresses,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = inpresses.Count,
                IsAscending = isAscending
            };

        }


        public async Task CreateInPressAsync(VMInPressAdd vmInPressAdd)
        {
            var userId = _user.GetLoggedInUserId();
            var useremail = _user.GetLoggedInEmail();

            if (vmInPressAdd.Photo == null)
            {
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "inpress-images", "default-image.jpg");

                byte[] imageBytes = await File.ReadAllBytesAsync(defaultImagePath);
                var stream = new MemoryStream(imageBytes);

                vmInPressAdd.Photo = new FormFile(stream, 0, stream.Length, "image", "default.jpg")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpg"
                };
            }

            var imageUpload = await imageHelper.Upload(vmInPressAdd.Title, vmInPressAdd.Photo, ImageType.Post);
            Image image = new Image(imageUpload.FullName, vmInPressAdd.Photo.ContentType, useremail);

            await unitOfWork.GetRepository<Image>().AddAsync(image);

            var user = await userService.GetAppUserByIdAsync(userId);

            var inpress = new InPress(vmInPressAdd.Title, vmInPressAdd.Content, userId, user, useremail, vmInPressAdd.CategoryId, image.Id);
            await unitOfWork.GetRepository<InPress>().AddAsync(inpress);

            await unitOfWork.SaveAsync();
        }

        public async Task<List<VMInPress>> GetAllInPressesWithCategoryNonDeletedAsync()
        {

            var inpresses = await unitOfWork.GetRepository<InPress>().GetAllAsync(x => !x.IsDeleted, x => x.Category, x => x.Image);
            var map = mapper.Map<List<VMInPress>>(inpresses);

            return map;
        }
        public async Task<VMInPress> GetInPressesWithCategoryNonDeletedAsync(Guid inpressId)
        {

            var inpress = await unitOfWork.GetRepository<InPress>().GetAsync(x => !x.IsDeleted && x.Id == inpressId, x => x.Category, i => i.Image, u => u.User);
            var map = mapper.Map<VMInPress>(inpress);

            return map;
        }
        public async Task<string> UpdateInPressAsync(VMInPressUpdate vmInPressUpdate)
        {
            var useremail = _user.GetLoggedInEmail();
            var inpress = await unitOfWork.GetRepository<InPress>().GetAsync(x => !x.IsDeleted & x.Id == vmInPressUpdate.Id, x => x.Category, i => i.Image);

            if (vmInPressUpdate.Photo != null)
            {
                imageHelper.Delete(inpress.Image.FileName);

                var imageUpload = await imageHelper.Upload(vmInPressUpdate.Title, vmInPressUpdate.Photo, ImageType.Post);
                Image image = new(imageUpload.FullName, vmInPressUpdate.Photo.ContentType, useremail);
                await unitOfWork.GetRepository<Image>().AddAsync(image);

                inpress.ImageId = image.Id;
            }

            inpress.Title = vmInPressUpdate.Title;
            inpress.Content = vmInPressUpdate.Content;
            inpress.CategoryId = vmInPressUpdate.CategoryId;
            inpress.ModifiedDate = DateTime.Now;
            inpress.ModifiedBy = useremail;


            await unitOfWork.GetRepository<InPress>().UpdateAsync(inpress);
            await unitOfWork.SaveAsync();

            return inpress.Title;
        }

        public async Task<string> SafeDeleteInPressAsync(Guid inpressId)
        {
            var userEmail = _user.GetLoggedInEmail();
            var inpress = await unitOfWork.GetRepository<InPress>().GetByGuidAsync(inpressId);

            inpress.IsDeleted = true;
            inpress.DeletedDate = DateTime.Now;
            inpress.DeletedBy = userEmail;

            await unitOfWork.GetRepository<InPress>().UpdateAsync(inpress);
            await unitOfWork.SaveAsync();

            return inpress.Title;
        }

        public async Task<List<VMInPress>> GetAllInPressesWithCategoryDeletedAsync()
        {
            var inpress = await unitOfWork.GetRepository<InPress>().GetAllAsync(x => x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<VMInPress>>(inpress);

            return map;
        }

        public async Task<string> UndoDeleteInPressAsync(Guid inpressId)
        {
            var inpress = await unitOfWork.GetRepository<InPress>().GetByGuidAsync(inpressId);

            inpress.IsDeleted = false;
            inpress.DeletedDate = null;
            inpress.DeletedBy = null;

            await unitOfWork.GetRepository<InPress>().UpdateAsync(inpress);
            await unitOfWork.SaveAsync();

            return inpress.Title;
        }

        public async Task<bool> InPressVisitorCheckerAsync(Guid inpressId)
        {
            var ipAdress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var inpressVisitors = await unitOfWork.GetRepository<InPressVisitor>().GetAllAsync(null, x => x.Visitor, y => y.InPress);
            var inpress = await unitOfWork.GetRepository<Blog.Entity.Entities.InPress>().GetAsync(x => x.Id == inpressId);

            var visitor = await unitOfWork.GetRepository<Visitor>().GetAsync(x => x.IpAddress == ipAdress);

            var addInPressVisitors = new InPressVisitor(inpress.Id, visitor.Id);

            if (inpressVisitors.Any(x => x.VisitorId == addInPressVisitors.VisitorId && x.InPressId == addInPressVisitors.InPressId))
            {
                return true;
            }
            else
            {
                await unitOfWork.GetRepository<InPressVisitor>().AddAsync(addInPressVisitors);
                inpress.ViewCount += 1;
                await unitOfWork.GetRepository<InPress>().UpdateAsync(inpress);
                await unitOfWork.SaveAsync();

                return false;
            }
        }

    }
}
