using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using ATHub.Web.Areas.Administrator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATHub.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class CategoriesController : Controller
    {
        public readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageCategories()
        {
            var model = new AllCategoriesVidewModel()
            {
                AllCategories = this.categoryService.GetAllCategories()
            };
           
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateCategoryViewModel model) 
        {
            var id = await this.categoryService.CreateCategory(model.Name);
            return this.RedirectToAction("ManageCategories", "Categories", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryId = await this.categoryService.DeleteCategory(id);
            return this.RedirectToAction("ManageCategories", "Categories", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditCategoryViewModel model)
        {
            await this.categoryService.EditCategory(model.Id, model.Name);
            return this.RedirectToAction("ManageCategories", "Categories", new { area = "Administrator" });
        }
    }
}