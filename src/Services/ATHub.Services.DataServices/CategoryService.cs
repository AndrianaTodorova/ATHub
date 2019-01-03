using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
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

        public async Task<int> DeleteCategory(int id)
        {
            var category = this.ctegoriesRepository.All().FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                throw new NullReferenceException(string.Format(ServicesDataConstants.InvalidCategoryName, id));
            }
            category.DeletedOn = DateTime.UtcNow;
            await this.ctegoriesRepository.SaveChangesAsync();
            return category.Id;
        }

        public IList<SingleCategoryViewModel> GetAllCategories()
        {
            var model = this.ctegoriesRepository.All().Where(c => c.DeletedOn == null).Select(a =>
            new SingleCategoryViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                VideosCount = a.Videos.Count(),
                CreatedOn = a.CreatedOn.ToShortDateString()
            }).OrderBy(x => x.CreatedOn).ToList();

            return model;
        }
    }
}
