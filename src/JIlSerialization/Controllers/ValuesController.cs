using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JIlSerialization.Models;

namespace JIlSerialization.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public Item[] Get()
        {
            var items = Enumerable.Range(0, 1000).Select(s => new Item
            {
                Id = Guid.NewGuid(),
                Size = rand.Next(10, 1000),
                Date = DateTime.Now.AddHours(rand.Next(-500, 2000)),
                Price = rand.Next(100, 100000),
                Description = "This item for test Jil serializer for .net core. We've make it for fun."
            }).ToArray();
            return items;
        }

        private Random rand = new Random();

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"value: {id}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
