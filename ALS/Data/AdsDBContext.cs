
namespace ALS.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class AdsDbContext : IdentityDbContext<User>
    {
        public AdsDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Ad> Ads { get; set; }

        public virtual IDbSet<Pictures> Pictures { get; set; }

        public static AdsDbContext Create()
        {
            return new AdsDbContext();
        }
    }
}