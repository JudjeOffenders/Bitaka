using ALS.Data;
using ALS.Models.Ad;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ALS.Controllers
{
    public class AdController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new AdsDbContext())
            {
                var ad = database.Ads
                    .Where(a => a.Id == id)
                    .First();


                // Check if User is the author or Admin (method below + added new method IsAuthor in Ad.cs)
                if (!IsUserAuthorizedToEdit(ad))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (ad == null)
                {
                    return HttpNotFound();
                }

                var adDeletingModel = new AdDeletingModel
                {
                    Id = ad.Id,
                    Title = ad.Title,
                    Category = ad.Category.Name,
                    Price = ad.Price,
                    Pictures = ad.Pictures.ToList()
                };

                return View(adDeletingModel);
            }

        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new AdsDbContext())
            {
                var ad = database.Ads
                    .Where(a => a.Id == id)
                    .First();

                var pictures = database.Pictures
                    .Where(p => p.Ad.Id == id)
                    .ToList();

                if (ad == null)
                {
                    return HttpNotFound();
                }

                database.Ads.Remove(ad);

                foreach (var picture in pictures)
                {
                    database.Pictures.Remove(picture);
                }

                database.SaveChanges();

                return RedirectToAction("List");
            }
        }


        // GET: Ad
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
            //return View();
            //return RedirectToAction("List");
        }

        //GET: Ad/List
        public ActionResult List(int? id, int page = 1, string user = null)
        {
            using (var database = new AdsDbContext())
            {
                var pageSize = 5;

                var adsQuery = database.Ads.AsQueryable();

                if (user != null)
                {
                    adsQuery = adsQuery
                        .Where(a => a.Author.Email == user);
                }
                else
                {
                    adsQuery = adsQuery.Where(a => a.Status != AdStatus.Pending);
                }

                if (id != null)
                {
                    adsQuery = adsQuery
                        .Where(a => a.CategoryId == id);
                }

                var ads = adsQuery
                    .OrderByDescending(a => a.Status)
                    .ThenByDescending(a => a.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new AdListingModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Category = a.Category.Name,
                        Price = a.Price,
                        MainPicture = a.Pictures.FirstOrDefault(),
                        Status = a.Status
                    })
                    .ToList();

                ViewBag.CurrentPage = page;

                return View(ads);
            }
        }

        //
        //GET: Ads/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            using (var database = new AdsDbContext())
            {
                var model = new CreateAdModel();
                model.Categories = database.Categories
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(model);
            }
        }

        //POST: Article/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateAdModel model, IEnumerable<HttpPostedFileBase> upload)
        {
            if (ModelState.IsValid)
            {
                //Get articles author
                var authorId = this.User.Identity.GetUserId();

                //constructor ???
                model.Pictures = new List<Pictures>();

                //uploads and saves the picture
                foreach (var picture in upload)
                {
                    if (picture != null && picture.ContentLength > 0)
                    {
                        var fileName = picture.FileName;
                        var path = Server.MapPath("~/Content/Pictures/") + fileName;
                        picture.SaveAs(path);
                        var currentPicture = new Pictures
                        {
                            FilePath = "/Content/Pictures/" + fileName
                        };

                        model.Pictures.Add(currentPicture);
                    }
                }


                var ad = new Ad
                {
                    Title = model.Title,
                    CategoryId = model.CategoryId,
                    City = model.City,
                    Content = model.Content,
                    Price = model.Price,
                    Pictures = model.Pictures,
                    AuthorId = authorId,
                    DateAdded = DateTime.Now,
                    Status = 0
                };

                //save article in DB
                using (var database = new AdsDbContext())
                {
                    database.Ads.Add(ad);
                    database.SaveChanges();
                }

                return RedirectToAction("Details", new { id = ad.Id });
            }

            return View(model);
        }


        //
        //GET Ad/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new AdsDbContext())
            {
                var ad = database.Ads
                    .Where(a => a.Id == id)
                    .Select(a => new AdDetailsModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Category = a.Category.Name,
                        City = a.City,
                        Content = a.Content,
                        Price = a.Price,
                        DateAdded = a.DateAdded,
                        Pictures = a.Pictures.ToList(),
                        Contact = a.Author.Email,
                        UserName = a.Author.UserName
                    })
                    .First();

                if (ad == null)
                {
                    return HttpNotFound();
                }

                return View(ad);
            }
        }

        //
        // GET: Article/Edit
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new AdsDbContext())
            {
                var adDb = database.Ads
                    .Where(a => a.Id == id)
                    .First();

                var ad = new AdEditModel
                {
                    Id = adDb.Id,
                    Title = adDb.Title,
                    CategoryId = adDb.CategoryId,
                    Categories = database.Categories
                        .OrderBy(c => c.Name)
                        .ToList(),
                    City = adDb.City,
                    Content = adDb.Content,
                    Price = adDb.Price,
                    Pictures = adDb.Pictures.ToList()
                };

                // Check if User is the author or Admin (method below + added new method IsAuthor in Ad.cs)
                if (!IsUserAuthorizedToEdit(adDb))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (ad == null)
                {
                    return HttpNotFound();
                }

                return View(ad);
            }
        }

        //
        // POST: Article/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(AdEditModel model, IEnumerable<HttpPostedFileBase> upload, int[] deleted)
        {
            if (ModelState.IsValid)
            {

                model.Pictures = new List<Pictures>();


                //uploads and saves newly added pictures
                foreach (var picture in upload)
                {
                    if (picture != null && picture.ContentLength > 0)
                    {
                        var fileName = picture.FileName;
                        var path = Server.MapPath("~/Content/Pictures/") + fileName;
                        picture.SaveAs(path);
                        var currentPicture = new Pictures
                        {
                            FilePath = "/Content/Pictures/" + fileName
                        };

                        model.Pictures.Add(currentPicture);
                    }
                }

                using (var database = new AdsDbContext())
                {
                    var adToEdit = database.Ads
                        .FirstOrDefault(a => a.Id == model.Id);

                    adToEdit.Title = model.Title;
                    adToEdit.CategoryId = model.CategoryId;
                    adToEdit.City = model.City;
                    adToEdit.Content = model.Content;
                    adToEdit.Price = model.Price;
                    adToEdit.Pictures = model.Pictures;

                    if (!(deleted == null))
                        foreach (var delId in deleted)
                        {
                            var picToDelete = database.Pictures
                                .Where(p => p.Id == delId)
                                .First();

                            database.Pictures.Remove(picToDelete);
                        }

                    database.Entry(adToEdit).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Details", new { id = adToEdit.Id });
                }
            }

            return View(model);
        }

        private bool IsUserAuthorizedToEdit(Ad ad)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = ad.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }

        //
        //Method for Aprove/Vip
        private void ChangeStatus(int? id, string status)
        {
            using (var database = new AdsDbContext())
            {
                var ad = database.Ads
                    .Where(a => a.Id == id)
                    .First();
                if (status == "Normal")
                {
                    ad.Status = AdStatus.Normal;
                }
                else
                {
                    ad.Status = AdStatus.VIP;
                }

                database.Entry(ad).State = EntityState.Modified;
                database.SaveChanges();

            }

        }

        //Approve ad by Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ChangeStatus(id, "Normal");

            return RedirectToAction("ListAdsStatus", "Ad");
        }


        //Make ad VIP by Admin
        [Authorize(Roles = "Admin")]
        public ActionResult MakeVIP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ChangeStatus(id, "VIP");

            return RedirectToAction("ListAdsStatus", "Ad");

        }


        [Authorize(Roles = "Admin")]
        public ActionResult ListAdsStatus()
        {
            using (var database = new AdsDbContext())
            {
                //Get ads from database
                var ads = database.Ads
                    .Select(a => new AdStatusListModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Category = a.Category.Name,
                        Author = a.Author.UserName,
                        DateAdded = a.DateAdded,
                        Status = a.Status
                    })
                    .OrderBy(a => a.Status)
                    .ThenByDescending(a => a.DateAdded)
                    .ToList();
                return View(ads);
            }
        }
    }
}