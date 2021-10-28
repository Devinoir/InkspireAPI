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

namespace InkspireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private Utils utils = new Utils();
        private IWebHostEnvironment _env;

        public PictureController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        { 
            string query = @"select PictureId, Title from dbo.Pictures";
            return utils.JsonResult(query);
        }

        [HttpPost]
        public JsonResult Post(Picture picture)
        {
            //string query = $@"insert into dbo.Pictures values('{picture.Title}', {picture.UploadUserId})";
            //utils.JsonResult(query, _configuration);
            return new JsonResult("Added successfully!");
        }

        [HttpPut]
        public JsonResult Put(Picture picture)
        {
            string query = $@"UPDATE dbo.Pictures SET 
                            Title = '{picture.Title}'
                            WHERE pictureId = {picture.PictureID}";
            utils.JsonResult(query);
            return new JsonResult("Updated successfully!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = $@"DELETE FROM dbo.Pictures
                            WHERE pictureId = {id}";
            utils.JsonResult(query);
            return new JsonResult("Deleted successfully!");
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
