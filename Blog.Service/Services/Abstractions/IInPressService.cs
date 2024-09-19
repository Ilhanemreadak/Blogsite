using Blog.Entity.ViewModels.Educationals;
using Blog.Entity.ViewModels.InPresses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface IInPressService
    {
        Task<VMInPressList> GetAllInPressByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 6, bool isAscending = false);
        Task CreateInPressAsync(VMInPressAdd vmInPressAdd);
        Task<List<VMInPress>> GetAllInPressesWithCategoryNonDeletedAsync();
        Task<VMInPress> GetInPressesWithCategoryNonDeletedAsync(Guid inpressId);
        Task<string> UpdateInPressAsync(VMInPressUpdate vmInPressUpdate);
        Task<string> SafeDeleteInPressAsync(Guid inpressId);
        Task<List<VMInPress>> GetAllInPressesWithCategoryDeletedAsync();
        Task<string> UndoDeleteInPressAsync(Guid inpressId);
        Task<bool> InPressVisitorCheckerAsync(Guid inpressId);
    }
}
