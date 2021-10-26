using InkspireAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InkspireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Utils utils = new Utils();

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select UserId, UserName, DateOfJoining, ProfilePicture from dbo.Users";
            return utils.JsonResult(query, _configuration);
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = $@"insert into dbo.Users values('{user.UserName}', '{user.DateOfJoining}', '{user.ProfilePicture}')";
            utils.JsonResult(query, _configuration);
            return new JsonResult("Added successfully!");
        }

        [HttpPut]
        public JsonResult Put(User user)
        {
            string query = $@"UPDATE dbo.Users SET 
                            UserName = '{user.UserName}',
                            DateOfJoining = '{user.DateOfJoining}',
                            ProfilePicture = '{user.ProfilePicture}'
                            WHERE UserId = {user.UserId}";
            utils.JsonResult(query, _configuration);
            return new JsonResult("Updated successfully!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = $@"DELETE FROM dbo.Users
                            WHERE UserId = {id}";
            utils.JsonResult(query, _configuration);
            return new JsonResult("Deleted successfully!");
        }
    }
}
