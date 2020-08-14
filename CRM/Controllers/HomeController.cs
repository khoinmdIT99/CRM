using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRM.Models;
using CRM.Services;

namespace CRM.Controllers
{
   [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly MyService My;

        public HomeController(IMyService my, IUserRepository repo) {
            My = (MyService)my;
            _repo = repo;
        }

        public IActionResult Index()
        {
            var a = My.User;
            return  View();
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Get()
        {
            var a =  _repo.Get();
            return Ok(a);
        }
        //public IActionResult Index()
        //{
        //    // var users = new UsersRepository();
        //    var a = _repo.Get();
        //    return View();
        //}

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
