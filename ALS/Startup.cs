using ALS.Data;
using ALS.Migrations;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(ALS.Startup))]
namespace ALS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<AdsDbContext, Configuration>());
            
                ConfigureAuth(app);
        }
    }
}
