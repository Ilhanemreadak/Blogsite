using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<List<VMCategory>> GetAllCategoriesNonDeleted();
        Task<List<VMCategory>> GetAllCategoriesDeleted();
        Task CreateCategoryAsync(VMCategoryAdd vmCategoryAdd);
        Task<Category> GetCategoryByGuid(Guid id);
        Task<string> UpdateCategoryAsync(VMCategoryUpdate vmCategoryUpdate);
        Task<string> SafeDeleteCategoryAsync(Guid categoryId);
        Task<string> UndoDeleteCategoryAsync(Guid categoryId);
    }
}
