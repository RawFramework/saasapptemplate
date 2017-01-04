using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using demoApp.Repositories;
using ScioSaaSPlatform.Security.AppSaaSUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ScioSaaSPlatform.Security.Auth;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        //>>>>>>>>>>>>>>>>>>>>>      ScioSaaS START
        //this is injected but you need to register the service in the start.cs
        ScioSaaSPlatform.Multitenancy.Helpers.DBConnectionHelper ConnectionHelper;
                
            
        /// <summary>
        /// We need to create a constructor that receives a DBConnectionHelper object
        /// this object is injected by the framework but must be registered
        /// </summary>
        public HomeController(ScioSaaSPlatform.Multitenancy.Helpers.DBConnectionHelper cnHelper)
        {
            this.ConnectionHelper = cnHelper;
        }

        //>>>>>>>>>>>>>>>>>>>>>      ScioSaaS END


        public IActionResult Index()
        {

            
            return View();
        }

        public async Task<IActionResult> GetData()
        {
            if (!User.Identity.IsAuthenticated)
            {
                SaaSUserHelper usr = new SaaSUserHelper();
                await usr.SetCurrentUser(HttpContext, "pramirez");
            }

            var repo = new CourseRepository(this.ConnectionHelper.GetConnectionStringForMultiTenantApp);
            

            return Json(new { records = await repo.DatabaseTable.GetAllAsync() });
        }

        public IActionResult GetDataEF()
        {
            EFContext.ConnectionString = this.ConnectionHelper.GetConnectionStringForMultiTenantApp;
            
            //You can also create the connection string manually
            //var optionsBuilder = new DbContextOptionsBuilder<EFContext>();
            //optionsBuilder.UseSqlServer(cn.GetConnectionStringForMultiTenantApp());
            
            //using (var dbContext = new EFContext(optionsBuilder.Options))
            using (var dbContext = new EFContext())
            {
                return Json(new { efRecords =  dbContext.Course.ToList() });
            }
            
        }

        [AuthorizeSaaS("admin")]
        public ActionResult GetUsers()
        {
            SaaSUserHelper usrs = new SaaSUserHelper();
            return Json(new { users = usrs.GetUsersForApp() });
        }

        [AuthorizeSaaS("notexists")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
