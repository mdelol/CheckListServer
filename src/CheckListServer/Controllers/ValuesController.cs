using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Contexts;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
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
            return _context.Tasks.First(x => x.TaskId == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Task value)
        {
            CurrentUser?.Tasks.Add(value);
            _context.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
