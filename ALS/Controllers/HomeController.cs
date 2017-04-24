namespace ALS.Controllers
{
    using Data;
    using Models.Ad;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return RedirectToAction("List", "Ad");
            var database = new AdsDbContext();

            var ads = database.Ads
                .Where(a => a.Status != AdStatus.Pending)
                .OrderByDescending(a => a.Status)
                .ThenByDescending(a => a.Id)
                .Take(3)
                .Select(a => new AdListingModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Category = a.Category,
                    Price = a.Price,
                    MainPicture = a.Pictures.FirstOrDefault(),
                    Status = a.Status
                    
                })
                .ToList();

            return View(ads);
        }

    }
}