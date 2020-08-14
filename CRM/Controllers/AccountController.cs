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
     [Route("/account")]
    public class AccountController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly MyService My;

        public AccountController(IMyService my , IUserRepository repo){
            My = (MyService)my;
            _repo = repo;
        }
        
        public IActionResult Login() {
            return  View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {
            if (!ModelState.IsValid){
                return BadRequest();
            }
            var user = _repo.Login(model.Username, model.Password);
            if(user == null){
                model.Masseg = "Sorry there is no user with this diatel in the sysem. please go to your maneger.";
                return BadRequest();
            }  else{
                My.User = user.Result;
            }
            return RedirectToAction("Index","Home");
        }
    }
}
