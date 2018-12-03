using ATHub.Data.Common;
using ATHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATHub.Services.DataServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> ctegoriesRepository;

        public CategoryService(IRepository<Category> ctegoriesRepository)
        {
            this.ctegoriesRepository = ctegoriesRepository;
        }
        public IEnumerable<string> GetCategories()
        {
            var categories = this.ctegoriesRepository.All().Select(c => c.Name).ToList();
            return categories;
        }
    }
}
