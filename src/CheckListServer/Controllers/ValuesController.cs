using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = DAL.Models.Task;

namespace CheckListServer.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        private readonly ProjectContext _context;

        public ValuesController(ProjectContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Task> Get([FromHeader, Required]string userToken)
        {
            var identity = User.Identity;
            return _context.Tasks;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Task Get(int id)
        {
            return _context.Tasks.First(x => x.TaskId == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Task value)
        {
            _context.Tasks.Add(value);
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
