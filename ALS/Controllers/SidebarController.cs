
namespace ALS.Controllers
{
    using ALS.Data;
    using ALS.Models.Category;
    using System.Linq;
    using System.Web.Mvc;

    public class SidebarController : Controller
    {
        // GET: Sidebar
        public ActionResult ListCategories()
        {
            using (var database = new AdsDbContext())
            {
                var categories = database.Categories
                    .Select(c => new CategoryListingModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        AdsNumber = c.Ads
                        .Where(a => a.Status != AdStatus.Pending)
                        .Count()
                    })
                    .ToList();

                return PartialView("_CategoriesPartial", categories);
            }
        }
    }
}