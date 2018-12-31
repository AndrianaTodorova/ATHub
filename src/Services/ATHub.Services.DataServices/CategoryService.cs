using ATHub.Data.Common;
using ATHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> ctegoriesRepository;
        private readonly IRepository<Video> videosRepository;

        public CategoryService(IRepository<Category> ctegoriesRepository, IRepository<Video> videosRepository)
        {
            this.ctegoriesRepository = ctegoriesRepository;
            this.videosRepository = videosRepository;
        }
        public IEnumerable<string> GetCategories()
        {
            var categories = this.ctegoriesRepository.All().Select(c => c.Name).ToList();
            return categories;
        }

        public async Task<int> CreateCategory(string name)
        {
            if(name == null)
            {
                throw new NullReferenceException(ServicesDataConstants.NullCategoryName);
            }
            var category = new Category() { Name = name };
            await this.ctegoriesRepository.AddAsync(category);
            await this.ctegoriesRepository.SaveChangesAsync();
            return category.Id;
        }

        public async Task<int> DeleteCategory(string name)
        {
            var category = this.ctegoriesRepository.All().FirstOrDefault(x => x.Name == name);
            if (category == null)
            {
                throw new NullReferenceException(string.Format(ServicesDataConstants.InvalidCategoryName, name));
            }
            this.ctegoriesRepository.Delete(category);
            await this.ctegoriesRepository.SaveChangesAsync();
            return category.Id;
        }
    }
}
