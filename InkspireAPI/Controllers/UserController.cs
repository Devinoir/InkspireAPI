using InkspireAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
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
        MySqlCompiler compiler = new MySqlCompiler();
        QueryFactory queryFactory;
        MySqlConnection connection;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new MySqlConnection(utils.getConStrSQL());
            queryFactory = new QueryFactory(connection, compiler);

        }

        [HttpGet]
        public JsonResult Get()
        {
            GuidIsUnique("145bb647-e3cf-45ad-84ab-b2d20e377ffb");
            return new JsonResult(queryFactory.Query("Users").Get());
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            try
            {
                var newUserID = Guid.NewGuid().ToString();
                if (GuidIsUnique(newUserID))
                {
                    queryFactory.Query("Users").AsInsert(new
                    {
                        UserID = newUserID,
                        UserName = user.UserName,
                        Password = user.Password,
                        Description = user.Description,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Address = user.Address,
                        EMail = user.EMail,
                        PhoneNumber = user.PhoneNumber,
                        IsArtist = user.IsArtist,
                        Pronouns = user.Pronouns,
                        JoinDate = DateTime.Now,
                        ProfilePicture = user.ProfilePicture
                    }).Get();

                    return new JsonResult("Added successfully!");
                }
                else
                {
                    Post(user);
                    return new JsonResult(utils.Error("Guid is not unique."));
                }
            }
            catch (Exception e)
            {
                return new JsonResult(utils.Error(e.Message));
                throw;
            }
        }

        public JsonResult GetUserByGuid(string guid)
        {
            try
            {
                return new JsonResult(queryFactory.Query("Users").Where("UserID", guid).First());
            }
            catch (Exception)
            {
                return new JsonResult(utils.Error("User could not be found."));
                throw;
            }
        }

        private bool GuidIsUnique(string guid)
        {
            if (GetUserByGuid(guid).Value.ToString().StartsWith("ERROR"))
            {
                return true;
            }
            return false;
        }

        [HttpPut]
        public JsonResult Put(User user)
        {
            //string query = $@"UPDATE dbo.Users SET 
            //                UserName = '{user.UserName}',
            //                DateOfJoining = '{user.JoinDate}',
            //                ProfilePicture = '{user.ProfilePicture}'
            //                WHERE UserId = {user.UserId}";
            //utils.JsonResult(query, _configuration);
            return new JsonResult("Updated successfully!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = $@"DELETE FROM dbo.Users
                            WHERE UserId = {id}";
            utils.JsonResult(query);
            return new JsonResult("Deleted successfully!");
        }
    }
}
