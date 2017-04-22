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
        public ActionResult Delete(int id)
        {
            return View();
        }


        // GET: Ad
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
            //return View();
            //return RedirectToAction("List");
        }

        //GET: Ad/List
        public ActionResult List()
        {
            using (var database = new AdsDbContext())
            {
                //Get ads from database
                var ads = database.Ads
                    .Select(a => new AdListingModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Category = a.Category,
                        Price = a.Price,
                        MainPicture = a.Pictures.FirstOrDefault()
                    })
                    .ToList();
                return View(ads);
            }
        }

        //GET: Ads/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
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
                    Category = model.Category,
                    City = model.City,
                    Content = model.Content,
                    Price = model.Price,
                    Pictures = model.Pictures,
                    AuthorId = authorId,
                    DateAdded = DateTime.Now
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
                        Category = a.Category,
                        City = a.City,
                        Content = a.Content,
                        Price = a.Price,
                        DateAdded = a.DateAdded,
                        Pictures = a.Pictures.ToList(),
                        Contact = a.Author.Email
                    })
                    .First();

                if (ad == null)
                {
                    return HttpNotFound();
                }

                return View(ad);
            }
        }

        // GET: Article/edit
        public ActionResult Edit( int? id)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}