using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Service.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Concrete
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<int>> GetYearlyArticleCounts()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted);

            var startDate = DateTime.Now.Date;
            startDate = new DateTime(startDate.Year, 1, 1);

            List<int> datas = new();

            for(int i = 1; i<=12; i++)
            {
                var startedDate = new DateTime(startDate.Year, i, 1);
                var endedDate = startedDate.AddMonths(1);
                var data = articles.Where(x => x.CreatedDate >= startedDate && x.CreatedDate < endedDate).Count();
                datas.Add(data);
            }
            return datas;
        }

        public async Task<int> GetTotalArticleCount()
        {
            return await unitOfWork.GetRepository<Article>().CountAsync();
        }
        public async Task<int> GetTotalCategoryCount()
        {
            return await unitOfWork.GetRepository<Category>().CountAsync();
        }

        public async Task<int> GetTotalViewCount()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync();
            int viewcounts = 0;
            foreach (var article in articles)
            {
                viewcounts += article.ViewCount;
            }

            return viewcounts;
        }

        public async Task<int> GetTotalMessageCount()
        {
            return await unitOfWork.GetRepository<ContactMessages>().CountAsync();
        }
    }
}
