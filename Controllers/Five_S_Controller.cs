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
                        return(host+ "//uploads//"+file.files.FileName);
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

        [HttpGet]
        [Route("GetAllElementIssue")]
        public string getAllIssueElement()
        {
            DataSet dsIE = new DataSet();
            dsIE = clmv.getAllElementIssue();
            var json_result = JsonConvert.SerializeObject(dsIE);
            return(json_result);
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

        [HttpGet]
        [Route("traceImpByIssue")]
        public string traceImpByIssue(int ID_Issue)
        {
            DataTable dtImp = new DataTable();
            dtImp = clmv.Trace_Imp_by_Issue(ID_Issue);
            var json_result = JsonConvert.SerializeObject(dtImp);
            return json_result;
        }
        [HttpGet]
        [Route("getImpDetail")]
        public string getImpDetail(int ID_Improve)
        {
            DataTable dtImp = new DataTable();
            dtImp = clmv.Trace_Imp_by_Imp_ID(ID_Improve);
            var json_result = JsonConvert.SerializeObject(dtImp);
            return json_result;
        }

        [HttpGet]
        [Route("getDeptImprove")]
        public string getDeptImprove(int ID_Issue)
        {
            DataTable dtDept = new DataTable();
            dtDept = clmv.deptImprove(ID_Issue);
            var json_result = JsonConvert.SerializeObject(dtDept);
            return json_result;
        }

        [HttpGet]
        [Route("SearchIssueName")]
        public string getIssueByName(string name)
        {
            DataTable dtISN = new DataTable();
            dtISN=clmv.searchNameIssue(name);
            var json_result = JsonConvert.SerializeObject(dtISN);
            return json_result;
        }

        [HttpGet]
        [Route("SearchIssueByID")]
        public string getIssueByID(int ID_Issue)
        {
            DataTable dtISD = new DataTable();
            dtISD=clmv.searchID_Issue(ID_Issue);
            var json_result = JsonConvert.SerializeObject(dtISD);
            return json_result;
        }

        [HttpPost]
        [Route("SearchIssue")]
        public ActionResult<string> searchIssue(Issue issue)
        {
            DataTable dtIssueHist = new DataTable();
            dtIssueHist = clmv.searchIssue(issue);
            var json_result = JsonConvert.SerializeObject(dtIssueHist);
            return json_result;
        }
        
        [HttpPost]
        [Route("UpdateIssue")]
        public ActionResult<string> UpdateIssue([FromBody] Issue issue)
        {
            string res = clmv.Update_Issue(issue);
            if(res=="OK")
            {
                return("OK");
            }
            else 
            {
                return(res);
            }
        }
        [HttpPost]
        [Route("ImproveDecision")]
        public ActionResult<string> ImproveDecision([FromBody] Improvement improvement)
        {
            SqlConnection con = new SqlConnection(DAO.connectionString);
            SqlConnection con2 = new SqlConnection(DAO.connectionString);
            SqlCommand cmdExec = new SqlCommand();
            //SqlCommand cmdReImp = new SqlCommand();
            cmdExec.CommandType=CommandType.Text;
            cmdExec.Connection = con;
            cmdExec.CommandText = "update Improve_Issue set Status = @status where ID_Improve =@id_imp";

            SqlCommand cmdReImp = new SqlCommand();
            cmdReImp.Connection = con2;
            cmdReImp.CommandType = CommandType.Text;
            cmdReImp.CommandText = "insert into Improve_Issue(ID_Issue, Status, Team_Improve) values(@id, @status, @team)";
            if(improvement.Status.ToString().ToLower()=="approve")
            {
                cmdExec.Parameters.AddWithValue("@id_imp", improvement.ID_Improve);
                cmdExec.Parameters.AddWithValue("@status", "Approve");
                con.Open();
                cmdExec.ExecuteNonQuery();
                con.Close();
            }
            else if(improvement.Status.ToString().ToLower()=="reject")
            {
                cmdExec.Parameters.AddWithValue("@id_imp", improvement.ID_Improve);
                cmdExec.Parameters.AddWithValue("@status", "Reject");
                con.Open();
                cmdExec.ExecuteNonQuery();
                con.Close();

                cmdReImp.Parameters.AddWithValue("@id", improvement.ID_Issue);
                cmdReImp.Parameters.AddWithValue("status", "Pending");
                cmdReImp.Parameters.AddWithValue("@team", improvement.Team_Improve);
                con2.Open();
                cmdReImp.ExecuteNonQuery();
                con2.Close();
            }
            return("OK");
        }
    }
}