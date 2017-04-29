using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SqlEditor.WebApi.Data;
using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ConnectionController : Controller
    {
        private readonly IRepository<DatabaseConnection> _repo;

        public ConnectionController(IRepository<DatabaseConnection> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IEnumerable<DatabaseConnection> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{name}")]
        public DatabaseConnection Get(string name)
        {
            return _repo.FindByName(name);
        }

        [HttpPost]
        public void Post([FromBody]DatabaseConnection value)
        {
            _repo.Save(value);         
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DatabaseConnection value)
        {
            _repo.Save(value);
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _repo.Remove(id);
        }
    }
}
