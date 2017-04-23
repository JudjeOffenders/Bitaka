
namespace ALS.Controllers
{
    using Data;
    using Models.Ad;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        //Listing summary of all ads
        public ActionResult ListAds()
        {
            using (var database = new AdsDbContext())
            {
                //Get ads from database
                var ads = database.Ads
                    .Select(a => new AdStatusListModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Category = a.Category,
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