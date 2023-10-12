using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Data;

namespace net_il_mio_fotoalbum.Controllers
{
    public class CategoryController : Controller
    {
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Homepage";
            using var ctx = new PhotoContext();

            var categoryList = ctx.Categories.ToArray();
            if (!ctx.Categories.Any())
            {
                ViewData["Message"] = "Nessuna categoria trovato";
            }
            return View("Index", categoryList);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category data)
        {
            using var ctx = new PhotoContext();
            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }

            Category newCategory = new Category();
            newCategory.Name = data.Name;

            ctx.Categories.Add(newCategory);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            using var ctx = new PhotoContext();
            Category edit_category = ctx.Categories.Where(category => category.Id == id).FirstOrDefault();

            if (edit_category == null)
            {
                return NotFound();
            }
            else
            {
                return View(edit_category);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(long id, Category data)
        {
            using var ctx = new PhotoContext();

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            Category edit_category = ctx.Categories.Where(category => category.Id == id).FirstOrDefault();
            if (edit_category == null)
            {
                return NotFound();
            }
            edit_category.Name = data.Name;
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(long id)
        {
            using var ctx = new PhotoContext();
            Category category = ctx.Categories.Where(category => category.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            ctx.Categories.Remove(category);
            ctx.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}