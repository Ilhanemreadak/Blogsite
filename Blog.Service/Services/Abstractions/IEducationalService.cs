using Blog.Entity.ViewModels.Educationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface IEducationalService
    {
        Task<VMEducationalList> GetAllEducationalByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 6, bool isAscending = false);
        Task CreateEducationalAsync(VMEducationalAdd vmEducationalAdd);
        Task<List<VMEducational>> GetAllEducationalsWithCategoryNonDeletedAsync();
        Task<VMEducational> GetEducationalsWithCategoryNonDeletedAsync(Guid educationalId);
        Task<string> UpdateEducationalAsync(VMEducationalUpdate vmEducationalUpdate);
        Task<string> SafeDeleteEducationalAsync(Guid educationalId);
        Task<List<VMEducational>> GetAllEducationalsWithCategoryDeletedAsync();
        Task<string> UndoDeleteEducationalAsync(Guid educationalId);
        Task<bool> EducationalVisitorCheckerAsync(Guid educationalId);
    }
}
