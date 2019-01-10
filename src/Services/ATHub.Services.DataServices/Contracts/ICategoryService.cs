using ATHub.Services.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public interface ICategoryService
    {
        Task<int> CreateCategory(string name);
        Task<int> DeleteCategory(int id);
        IEnumerable<string> GetCategories();
        IList<SingleCategoryViewModel> GetAllCategories();
        Task EditCategory(int id, string name);
    }
}
