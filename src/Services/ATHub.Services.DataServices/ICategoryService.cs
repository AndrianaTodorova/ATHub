using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public interface ICategoryService
    {
        Task<int> CreateCategory(string name);
        Task<int> DeleteCategory(string name);
        IEnumerable<string> GetCategories();
    }
}
