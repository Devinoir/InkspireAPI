using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using InkspireAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using SqlKata.Execution;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;

namespace InkspireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private Utils utils = new Utils();
        private IWebHostEnvironment _env;

        MySqlCompiler compiler = new MySqlCompiler();
        QueryFactory queryFactory;
        MySqlConnection connection;

        public PictureController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            connection = new MySqlConnection(utils.getConStrSQL());
            queryFactory = new QueryFactory(connection, compiler);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(queryFactory.Query("Pictures").Get());
        }

        [HttpPost]
        public JsonResult Post(Picture picture)
        {
            try
            {
                var newPictureID = Guid.NewGuid().ToString();
                if (utils.GuidIsUnique("Pictures", "PictureID", newPictureID))
                {
                    queryFactory.Query("Pictures").AsInsert(new
                    {
                        PictureID = newPictureID,
                        UploadUserID = picture.UploadUserID,
                        Title = picture.Title,
                        Description = picture.Description,
                        Url = picture.Url
                    }).Get();

                    return new JsonResult("Added successfully!");
                }
                else
                {
                    Post(picture);
                    return new JsonResult(utils.Error("Guid is not unique."));
                }
            }
            catch (Exception e)
            {
                return new JsonResult(utils.Error(e.Message));
            }
        }

        [HttpPut]
        public JsonResult Put(Picture picture)
        {
            try
            {
                queryFactory.Query("Pictures").Where("PictureID", picture.PictureID).AsUpdate(new
                {
                    UploadUserID = picture.UploadUserID,
                    Title = picture.Title,
                    Description = picture.Description,
                    Url = picture.Url
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
                queryFactory.Query("Pictures").Where("PictureID", id).AsDelete().Get();

                return new JsonResult($"Picture with ID '{id}' was successfully deleted.");
            }
            catch (Exception e)
            {
                return new JsonResult(utils.Error(e.Message));
            }
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = $"{_env.ContentRootPath}/Pictures/{filename}";

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anon.png");
            }
        }
    }
}
