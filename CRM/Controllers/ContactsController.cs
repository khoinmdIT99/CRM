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
    public class ContactsController : Controller
    {
        private readonly IContactRepository _contactRepo;
        private readonly IPositionRepository _positionRepo;

        private readonly MyService My;

        public ContactsController(IMyService my, IContactRepository contactRepo, IPositionRepository positionRepo)
        {
            My = (MyService)my;
            _contactRepo = contactRepo;
            _positionRepo = positionRepo;
        }

        public IActionResult Contacts(int? custID, string id)
        {
            var defaults = new PageModel();
            defaults.Selects.Add("positions", new List<SelectItem>(_positionRepo.Get().Result.Select(x => 
            new SelectItem { ID = x.ID, Name = x.Name}).ToList()));

            if (id == null)
            {
                return View("Contacts", defaults);
            }
            else
            {
                var newID = Int32.Parse(id);
                defaults.Item = newID;
                 defaults.Temp =  custID != null ? custID : null;
                return View("Contact", defaults);
            }
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IEnumerable<Contact> items;
            try
            {
                items = _contactRepo.Get().Result;
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
            Contact item;

            if (id > 0)
            {
                try
                {
                    item = _contactRepo.GetByID(id).Result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                item = new Contact();
                item.StatusID = 5;
            }
            return Ok(item);
        }

        [HttpPost("save")]
        public IActionResult Save([FromBody]Contact model)
        {
            Contact item;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                item = _contactRepo.Save(model).Result;
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
                    _contactRepo.Delete(id);
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
