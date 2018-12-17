using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateCategoryViewModel model) 
        {
            var id = await this.categoryService.CreateCategory(model.Name);
            return this.RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            var categoryId = await this.categoryService.DeleteCategory(name);
            return this.RedirectToAction("Index", "Home", new { area = "" });
        }


    }
}