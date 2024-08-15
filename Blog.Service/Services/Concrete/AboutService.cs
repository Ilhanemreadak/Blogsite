using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Others;
using Blog.Service.Services.Abstractions;

namespace Blog.Service.Services.Concrete
{
    public class AboutService : IAboutService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AboutService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<About> GetAboutAsync(int aboutId)
        {
            var about = await unitOfWork.GetRepository<About>().GetByIdAsync(aboutId);
            return about;
        }
        public async Task<List<VMAbout>> GetAllAbouts()
        {
            var abouts = await unitOfWork.GetRepository<About>().GetAllAsync();
            var map = mapper.Map<List<VMAbout>>(abouts);

            return map;
        }

        public async Task<string> UpdateAboutAsync(VMAboutUpdate vmAboutUpdate)
        {
            var about = await unitOfWork.GetRepository<About>().GetAsync(x => x.Id == vmAboutUpdate.Id);

            about.Title = vmAboutUpdate.Title;
            about.Description = vmAboutUpdate.Description;

            await unitOfWork.GetRepository<About>().UpdateAsync(about);
            await unitOfWork.SaveAsync();

            return about.Title;
        }

    }
}
