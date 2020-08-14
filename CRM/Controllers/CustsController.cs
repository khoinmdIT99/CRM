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
    public class CustsController : Controller
    {
        private readonly ICustRepository _custRepo;
        private readonly ICityRepository _cityRepo;
        private readonly IMainIndustryRepository _mainIndustryRepo;
        private readonly IIndustryRepository _industryRepo;
        private readonly IPositionRepository _positionRepo;
        private readonly IUserRepository _userRepo;
        private readonly IContactRepository _contactRepo;

        private readonly MyService My;

        public CustsController(IMyService my, ICustRepository custsRepo, ICityRepository cityRepo, 
            IMainIndustryRepository mainIndustryRepo, IIndustryRepository industryRepo, IPositionRepository positionRepo
            , IUserRepository userRepo, IContactRepository contactRepo)
        {
            My = (MyService)my;
            _custRepo = custsRepo;
            _cityRepo = cityRepo;
            _mainIndustryRepo = mainIndustryRepo;
            _industryRepo = industryRepo;
            _positionRepo = positionRepo;
            _contactRepo = contactRepo;
            _userRepo = userRepo;

        }

        public IActionResult Custs(string id)
        {
            var defaults = new PageModel();

            defaults.Selects.Add("cities", new List<SelectItem>(_cityRepo.Get().Result.Select(x => new SelectItem { ID = x.ID, Name = x.Name }).ToList()));
            defaults.Selects.Add("mainIndustries", new List<SelectItem>(_mainIndustryRepo.Get().Result.Select(x => new SelectItem { ID = x.ID, Name = x.Name }).ToList()));
            defaults.Selects.Add("industries", new List<SelectItem>(_industryRepo.Get().Result.Select(x => new SelectItem { ID = x.ID, Name = x.Name, Extra = x.MainIndustryID }).ToList()));
            if (id == null)  {
                return View("Custs", defaults);
            } else  {
                var newID = Int32.Parse(id);
                defaults.Item = newID;
                defaults.Selects.Add("positions", new List<SelectItem>(_positionRepo.Get().Result.Select(x => new SelectItem
                { ID = x.ID, Name = x.Name }).ToList()));

                    defaults.Selects.Add("contacts", new List<SelectItem>(_contactRepo.GetByCustID(newID).Result.Select(x => new SelectItem
                    { ID = x.ID, Name = x.Name }).ToList()));

                defaults.Selects.Add("users", new List<SelectItem>(_userRepo.Get().Result.Select(x => new SelectItem
                { ID = x.ID, Name = x.Username }).ToList()));

                defaults.Selects.Add("types", new List<SelectItem>(Enum.GetValues(typeof(Types)).Cast<Types>()
                    .Select(x => new SelectItem { ID = (int)x, Name = x.ToString() })).ToList());

                return View("Cust", defaults);
            }
        }
        

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IEnumerable<Cust> items;
            try
            {
                items = _custRepo.Get().Result;
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(items);

        }

        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            Cust item;

            if (id > 0)
            {
                try
                {
                    item = _custRepo.GetFull(id).Result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                item = new Cust
                {
                    //Name = "Liora Motza",
                    //Address = "Sokolov",
                    Description= "Some Desccrription",
                   // Email = "liora@gmail.com",
                    //Industry = 1,
                   // MainIndustryID = 1,
                   // Mobile = "0528785558",
                    Fax = "0000545454",
                   // Phone = "525252525",
                    StatusID = 5,
                    Website = "wwww.dddd.xxxx.ccc"

                };

            }
            return Ok(item);
        }

        [HttpPost("save")]
        public IActionResult Save([FromBody]Cust model)
        {
            Cust item;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                item = _custRepo.Save(model).Result;
            }
            catch (Exception ex)
            {
             throw ex;
            }

            return Ok(item);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    _custRepo.Delete(id);
                }
                catch (Exception)
                {
                    throw;
                }

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
