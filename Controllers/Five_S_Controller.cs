using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HSE_5S_API.Controllers
{
    [ApiController]
    [Route("api/HSE5S")]
    public class Five_S_Controller:ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public Five_S_Controller(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public class FileUploadAPI
        {
            public IFormFile files{get;set;}
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit =15360000)]
        [RequestSizeLimit(15360000)]
        [Route("PostFile")]
        public async Task<string> Post([FromForm] FileUploadAPI file)
        {
            string host = "192.168.0.12:8080";
            string host_dev = "localhost:5000";
            if(file.files.Length >0)
            {
                try
                {
                    if(!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                    }
                    using(FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + file.files.FileName))
                    {
                        file.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return(host+ "\\uploads\\"+file.files.FileName);
                    }
                }
                catch(Exception ex)
                {
                    return(ex.ToString());
                }
            }
            else
            {
                return("Unsuccesful");
            }
        }
        DAO clmv = new DAO();

        [HttpGet]
        [Route("GetClassify")]
        public string getClassify()
        {
            DataTable dta = new DataTable();
            dta = clmv.getClassify();
            var json_result = JsonConvert.SerializeObject(dta);
            return json_result;
        }

        [HttpGet]
        [Route("GetLocation")]
        public string getLocation()
        {
            DataTable dta = new DataTable();
            dta = clmv.getLocation();
            var json_result = JsonConvert.SerializeObject(dta);
            return json_result;
        }

        [HttpGet]
        [Route("GetLocationDesc")]
        public ActionResult<string> getLocation_desc()
        {
            DataTable dta = new DataTable();
            dta = clmv.getLocation_desc();
            var json_result = JsonConvert.SerializeObject(dta);
            return json_result;
        }

        [HttpGet]
        [Route("GetDepartment")]
        public string GetDepartment()
        {
            DataTable dta = new DataTable();
            dta = clmv.getDepartment();
            var json_result = JsonConvert.SerializeObject(dta);
            return json_result;
        }
        
        [HttpGet]
        [Route("GetLoss")]
        public string getLoss()
        {
            DataTable dta = new DataTable();
            dta = clmv.getLoss();
            var json_result = JsonConvert.SerializeObject(dta);
            return json_result;
        }

        [HttpGet]
        [Route("GetUser")]
        public string getUser()
        {
            DataTable data = new DataTable();
            data = clmv.GetUsser();
            var json_result = JsonConvert.SerializeObject(data);
            return json_result;
        }
        
        [HttpPost]
        [Route("PostIssue")]
        public ActionResult postIssue([FromBody] Issue issue)
        {
            clmv.postIssue(issue);
            return Ok();
        }

        [HttpPost]
        [Route("PostImprovement")]
        public ActionResult postImprovement([FromBody] Improvement improvement)
        {
            clmv.PostImprovment(improvement);
            return Ok("OK");
        }

        [HttpPost]
        [Route("PostUser")]
        public ActionResult postUser([FromBody]_User user)
        {
            clmv.PostUser(user);
            return Ok("Ok");
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult<string> loginAction([FromBody] Login_User login_User)
        {
            DataTable dtlogin = new DataTable();
            dtlogin = clmv.Login_User(login_User);
            if(dtlogin.Rows.Count > 0)
            {
                var json_result = JsonConvert.SerializeObject(dtlogin);
                return json_result;
            }
            else
            {
                return("NG");
            }
        }
        
        [HttpGet]
        [Route("TraceGeneralIssue")]
        public string TraceGenIssue(int numberRecord)
        {
            DataTable dtIS = new DataTable();
            dtIS = clmv.generalTraceIssue(numberRecord);
            var json_result = JsonConvert.SerializeObject(dtIS);
            return json_result;
        }
    }
}