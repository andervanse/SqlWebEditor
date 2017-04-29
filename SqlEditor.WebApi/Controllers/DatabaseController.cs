using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SqlEditor.WebApi.Data;
using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DatabaseController : Controller
    {
        private readonly IDatabaseRepository _databaseRepo;
        private readonly IRepository<DatabaseConnection> _connRepo;

        public DatabaseController(IDatabaseRepository databaseRepo, IRepository<DatabaseConnection> connRepo)
        {
            _databaseRepo = databaseRepo;
            _connRepo = connRepo;
        }

        [HttpGet("{connName}")]
        public IEnumerable<TableSchema> Get(string connName)
        {
            var conn = _connRepo.FindByName(connName);
            return _databaseRepo.GetAll(conn);
        }

        [HttpGet("{connName}/{tableName}")]
        public TableSchema Get(string connName, string tableName)
        {
            var conn = _connRepo.FindByName(connName);
            return _databaseRepo.FindByName(conn, tableName);
        }
    }
}
