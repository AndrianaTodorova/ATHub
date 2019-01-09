using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Tets
{
    public class CategoriesServiceTests : BaseServiceTests
    {
        private ICategoryService CategoryService => this.ServiceProvider.GetRequiredService<ICategoryService>();

        [Test]
        public async Task GetAllCategories()
        {
            var videos = new List<Video>()
            {
                new Video(){Id = 1, Link = "prosto link", Description = "description"},
                 new Video(){Id = 2, Link = "prosto drug link", Description = "description"},
                  new Video(){Id = 3, Link = "prosto treti link", Description = "description"},
            };
            await this.DbContext.Videos.AddRangeAsync(videos);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Categories.Add(new Category()
            {
                Id = 1,
                Name = "FirstCategory",
                CreatedOn = DateTime.UtcNow,
                Videos = videos
            });
            this.DbContext.SaveChanges();
            var expected = this.DbContext.Categories.Select(a =>
             new SingleCategoryViewModel()
           {
               Id = a.Id,
               Name = a.Name,
               VideosCount = a.Videos.Count(),
               CreatedOn = a.CreatedOn.ToShortDateString(),
               DeletedOn = a.DeletedOn.HasValue ? a.DeletedOn.Value.ToString("yyyy-MM-dd") : null
           }).OrderBy(x => x.CreatedOn).ToList();

            var actual = this.CategoryService.GetAllCategories();

            Assert.That(expected.Count.Equals(actual.Count()));
            Assert.That(expected.Select(x => x.Name).FirstOrDefault().Equals(actual.Select(x => x.Name).FirstOrDefault()));
            Assert.That(expected.Select(x => x.VideosCount).FirstOrDefault().Equals(actual.Select(x => x.VideosCount).FirstOrDefault()));
        }

        [Test]
        public async Task CreateCategory()
        {
            string categoryName = "MyCategory";
            var expected = new Category() { Id = 1,Name = categoryName };
            await this.DbContext.Categories.AddAsync(expected);
            await this.DbContext.SaveChangesAsync();

            var actual = this.CategoryService.CreateCategory(categoryName);
            Assert.That(expected.Id.Equals(actual.Id));
        }
        [Test]
        public async Task EditCategory()
        {
            string categoryName = "MyCategory";
            string newName = "NewCategoryName";
            var expected = new Category() { Id = 1, Name = categoryName };
            await this.DbContext.Categories.AddAsync(expected);
            await this.DbContext.SaveChangesAsync();
            int id = 1;
            var category = this.DbContext.Categories.FirstOrDefault(x => x.Id == id);
        
            category.Name = newName;
            await this.DbContext.SaveChangesAsync();

            var actual = this.CategoryService.EditCategory(id, newName);

            Assert.That(expected.Name.Equals(this.DbContext.Categories.Find(id).Name));
        }

        [Test]
        public void EditCategoryThrowsException()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
               this.CategoryService.EditCategory(4, "name"));
            Assert.That(ServicesDataConstants.InvalidId.Equals(exception.Message)) ;
        }
    }
}
