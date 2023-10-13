using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Homepage";
            using var ctx = new PhotoContext();

            var photoGallery = ctx.Photos.ToArray();
            if (!ctx.Photos.Any())
            {
                ViewData["Message"] = "Nessun risultato trovato";
            }
            return View("Index", photoGallery);
        }

        public IActionResult IndexUser()
        {
            return View();
        }

        public IActionResult Show(long id)
        {
            using var ctx = new PhotoContext();
            var singlePhoto = ctx.Photos.Include(photo => photo.Categories).First(photo => photo.Id == id);

            return View("Show", singlePhoto);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            using var ctx = new PhotoContext();

            PhotoFormModel model = new PhotoFormModel();
            model.Photo = new Photo();

            List<Category> categories = ctx.Categories.ToList();
            List<SelectListItem> listCategories = new List<SelectListItem>();
            foreach (Category category in categories)
            {
                listCategories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            model.Categories = listCategories;

            return View("Create", model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel data)
        {
            using var ctx = new PhotoContext();
            if (!ModelState.IsValid)
            {

                List<Category> categories = ctx.Categories.ToList();
                List<SelectListItem> listCategories = new List<SelectListItem>();
                foreach (Category category in categories)
                {
                    listCategories.Add(new SelectListItem()
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }
                data.Categories = listCategories;
                return View("Create", data);
            }

            Photo newPhoto = new Photo();
            newPhoto.Title = data.Photo.Title;
            newPhoto.Description = data.Photo.Description;
            newPhoto.Url = data.Photo.Url;
            newPhoto.Visible = data.Photo.Visible;
            newPhoto.Categories = new List<Category>();

            if (data.SelectedCategories != null)
            {
                foreach (string selectedCategoryId in data.SelectedCategories)
                {
                    int selectedIntCategoryId = int.Parse(selectedCategoryId);
                    Category category = ctx.Categories.Where(x => x.Id == selectedIntCategoryId).FirstOrDefault();
                    newPhoto.Categories.Add(category);
                }
            }
            ctx.Photos.Add(newPhoto);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            using var ctx = new PhotoContext();

            Photo edit_photo = ctx.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();
            if (edit_photo == null)
            {
                return NotFound();
            }
            else
            {
                List<Category> categories = ctx.Categories.ToList();
                List<SelectListItem> listCategories = new List<SelectListItem>();
                foreach (Category category in categories)
                {
                    listCategories.Add(new SelectListItem()
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }

                PhotoFormModel model = new PhotoFormModel();
                model.Photo = edit_photo;
                model.Categories = listCategories;

                return View("Edit", model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Update(long id, PhotoFormModel data)
        {
            using var ctx = new PhotoContext();

            if (!ModelState.IsValid)
            {

                List<Category> categories = ctx.Categories.ToList();
                List<SelectListItem> listCategories = new List<SelectListItem>();
                foreach (Category category in categories)
                {
                    listCategories.Add(new SelectListItem()
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }
                data.Photo = ctx.Photos.Where(p => p.Id == id).FirstOrDefault();
                data.Categories = listCategories;

                return View("Create", data);
            }

            Photo edit_photo = ctx.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();
            if (edit_photo == null)
            {
                return NotFound();
            }
            edit_photo.Title = data.Photo.Title;
            edit_photo.Description = data.Photo.Description;
            edit_photo.Url = data.Photo.Url;
            edit_photo.Visible = data.Photo.Visible;
            edit_photo.Categories.Clear();

            if (data.SelectedCategories != null)
            {
                foreach (string selectedCategoryId in data.SelectedCategories)
                {
                    int selectedIntCategoryId = int.Parse(selectedCategoryId);
                    Category category = ctx.Categories.Where(x => x.Id == selectedIntCategoryId).FirstOrDefault();
                    edit_photo.Categories.Add(category);
                }
            }
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(long id)
        {
            using var ctx = new PhotoContext();
            Photo photo = ctx.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photo == null)
            {
                return NotFound();
            }

            ctx.Photos.Remove(photo);
            ctx.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}