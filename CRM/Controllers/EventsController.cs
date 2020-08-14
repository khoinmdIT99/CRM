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
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepo;
        private readonly IUserRepository _userRepo;
        private readonly IContactRepository _contactRepo;
        private readonly MyService My;

        public EventsController(IMyService my, IEventRepository eventRepo, IUserRepository userRepo, IContactRepository contactRepo)
        {
            My = (MyService)my;
            _eventRepo = eventRepo;
            _contactRepo = contactRepo;
            _userRepo = userRepo;
        }

        public IActionResult Events(int? custID, string id)
        {
            var defaults = new PageModel();

            defaults.Selects.Add("users", new List<SelectItem>(_userRepo.Get().Result.Select(x => new SelectItem
            { ID = x.ID, Name = x.Username }).ToList()));

            defaults.Selects.Add("types", new List<SelectItem>(Enum.GetValues(typeof(Types)).Cast<Types>()
                .Select(x => new SelectItem { ID = (int)x, Name = x.ToString() })).ToList());

            if (id == null)
            {

                defaults.Selects.Add("contacts", new List<SelectItem>(_contactRepo.Get().Result.Select(x => new SelectItem
                { ID = x.ID, Name = x.Name }).ToList()));

                return View("Events", defaults);
            }
            else
            {
                if (custID != null && custID > 0)
                    defaults.Selects.Add("contacts", new List<SelectItem>(_contactRepo.GetByCustID(custID.Value).Result.Select(x => new SelectItem
                    { ID = x.ID, Name = x.Name }).ToList()));

                var newID = Int32.Parse(id);
                defaults.Item = newID;
                defaults.Temp = custID != null ? custID : null;
                return View("Event", defaults);
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IEnumerable<Event> items;
            try
            {
                items = _eventRepo.Get().Result;
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
            Event item;

            if (id > 0)
            {
                try
                {
                    item = _eventRepo.GetByID(id).Result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                item = new Event();
                item.UserID = 1;
                item.StatusID = 5;
                item.FileName = "vvv";
            }
            return Ok(item);
        }

        [HttpPost("save")]
        public IActionResult Save([FromBody]Event model)
        {
            Event item;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                item = _eventRepo.Save(model).Result;
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
                    _eventRepo.Delete(id);
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
