using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contexts;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = DAL.Models.Task;

namespace CheckListServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ProjectContext _context;

        private User CurrentUser
        {
            get
            {
                return _context.Users.Include(x => x.Tasks).FirstOrDefault(x => x.UserName == User.Identity.Name);
            }
        }

        public ValuesController(ProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            Console.WriteLine($"Name: {CurrentUser?.UserName}");
            return CurrentUser?.Tasks;
        }

        [HttpGet("{id}")]
        public Task Get(int id)
        {
            return CurrentUser?.Tasks.First(x => x.TaskId == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Task value)
        {
            CurrentUser?.Tasks.Add(value);
            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public ObjectResult Put(int id, [FromBody]Task value)
        {
            var task = CurrentUser?.Tasks.FirstOrDefault(x => x.TaskId == id);
            if (task == null)
            {
                return NotFound(id);
            }
            task.Description = value.Description;
            task.IsCompleted = value.IsCompleted;
            _context.SaveChanges();

            return new ObjectResult(task);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var firstOrDefault = CurrentUser?.Tasks.FirstOrDefault(x=>x.TaskId == id);
            if (firstOrDefault == null)
            {
                return NotFound(id);
            }

            CurrentUser?.Tasks.Remove(firstOrDefault);
            _context.Tasks.Remove(firstOrDefault);
            _context.SaveChanges();
            return Ok();
        }
    }
}
