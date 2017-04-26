
namespace ALS.Controllers
{
    using Data;
    using Models.Category;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    public class CategoryController : Controller
    {
        // GET: Catergory
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        //GET:Category/List
        public ActionResult List()
        {
            using (var database = new AdsDbContext())
            {
                
                //Get ads from database
                var categories = database.Categories
                    .Select(c => new CategoryListingModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        AdsNumber = c.Ads.Count()
                    })
                    .ToList();

                return View(categories);
            }

        }

        //
        //GET:Category/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        //GET:Category/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(CategoryCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.Name,
                    Ads = new List<Ad>()
                };

                //save article in DB
                using (var database = new AdsDbContext())
                {
                    database.Categories.Add(category);
                    database.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        //GET:Category/Edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new AdsDbContext())
            {
                var categoryDb = database.Categories
                    .Where(c => c.Id == id)
                    .First();

                var category = new CategoryModifyModel
                {
                    Id = categoryDb.Id,
                    Name = categoryDb.Name
                };

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
        }

        //
        // POST: Category/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(CategoryModifyModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new AdsDbContext())
                {
                    var categoryToEdit = database.Categories
                        .FirstOrDefault(a => a.Id == model.Id);

                    categoryToEdit.Name = model.Name;

                    database.Entry(categoryToEdit).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        //
        //GET:Category/Delete
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new AdsDbContext())
            {
                var categoryDb = database.Categories
                    .Where(c => c.Id == id)
                    .First();

                var category = new CategoryModifyModel
                {
                    Id = categoryDb.Id,
                    Name = categoryDb.Name
                };

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
        }

        //
        // POST: Category/Delete
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new AdsDbContext())
            {
                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);

                var categoryAds = category.Ads.ToList();

                foreach (var ad in categoryAds)
                {
                    var adsPictures = ad.Pictures.ToList();

                    foreach (var picture in adsPictures)
                    {
                        database.Pictures.Remove(picture);
                    }

                    database.Ads.Remove(ad);
                }

                database.Categories.Remove(category);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}