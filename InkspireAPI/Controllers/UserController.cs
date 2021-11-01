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
            return new JsonResult(queryFactory.Query("Users").Get());
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            try
            {
                var newUserID = Guid.NewGuid().ToString();
                if (utils.GuidIsUnique("Users", "UserID", newUserID))
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
            }
        }


        [HttpPut]
        public JsonResult Put(User user)
        {
            try
            {
                queryFactory.Query("Users").Where("UserID", user.UserID).AsUpdate(new
                {
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
                    ProfilePicture = user.ProfilePicture
                }).Get();

                return new JsonResult("Updated successfully!");
            }
            catch (Exception e)
            {
                return new JsonResult(utils.Error(e.Message));
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            try
            {
                queryFactory.Query("Users").Where("UserID", id).AsDelete().Get();

                return new JsonResult($"User with ID '{id}' was successfully deleted.");
            }
            catch (Exception e)
            {
                return new JsonResult(utils.Error(e.Message));
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

        public bool GuidIsUnique(string guid)
        {
            if (GetUserByGuid(guid).Value.ToString().StartsWith("ERROR"))
            {
                return true;
            }
            return false;
        }
    }
}
