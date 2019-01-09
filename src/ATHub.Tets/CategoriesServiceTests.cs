using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.EntityFrameworkCore;
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
                new Video(){Link = "prosto link", Description = "description"},
                 new Video(){ Link = "prosto drug link", Description = "description"},
                  new Video(){ Link = "prosto treti link", Description = "description"},
            };
            await this.DbContext.Videos.AddRangeAsync(videos);
            await this.DbContext.SaveChangesAsync();
            await this.DbContext.Categories.AddAsync(new Category()
            {
                
                Name = "FirstCategory",
                CreatedOn = DateTime.UtcNow,
                Videos = videos
            });
            await this.DbContext.SaveChangesAsync();
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
            var expected = new Category() { Name = categoryName };
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
            var expected = new Category() { Name = categoryName };
            await this.DbContext.Categories.AddAsync(expected);
            await this.DbContext.SaveChangesAsync();
            var cat = this.DbContext.Categories.FirstOrDefault(x => x.Name == categoryName);
            var category = this.DbContext.Categories.FirstOrDefault(x => x.Id == cat.Id);
        
            category.Name = newName;
            await this.DbContext.SaveChangesAsync();

            var actual = this.CategoryService.EditCategory(cat.Id, newName);

            Assert.That(newName.Equals(this.DbContext.Categories.Find(cat.Id).Name));
   
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
