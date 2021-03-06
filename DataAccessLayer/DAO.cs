using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text.Json;

public class DAO
{
    public static string connectionString = "Server=DESKTOP-9JB1Q43\\SQLEXPRESS;Database=5sSystem;User Id=dovanan95;Password=hayvuilennao1;";
    public DataTable GetUsser()
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection= con;
        cmd.CommandText = "select * from [User] where Password=Password";
        //cmd.Parameters.AddWithValue("@Password", "Password");
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }
    public DataTable getClassify()
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "select * from Classify";
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;

    }

     public DataTable getDepartment()
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "select * from Department";
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;

    }
    public DataTable getLocation()
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "select ID_Location, Name_Location from Location";
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataTable getLocation_desc()
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select * from Location_Detail";
        con.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataTable getLoss()
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection= con;
        cmd.CommandText = "select * from Level";
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataSet getAllElementIssue()
    {
        DataSet dsTotal = new DataSet();
        SqlConnection con = new SqlConnection(connectionString);

        string getClassify = "select * from Classify";
        string getLocation = "select ID_Location, Name_Location from Location";
        string getLocation_desc = "select * from Location_Detail";
        string getDepartment = "select * from Department";
        string getLoss = "select * from Level";

        SqlCommand cmdClass = new SqlCommand(getClassify, con);
        SqlCommand cmdLoc = new SqlCommand(getLocation, con);
        SqlCommand cmdLocD = new SqlCommand(getLocation_desc, con);
        SqlCommand cmdPB = new SqlCommand(getDepartment, con);
        SqlCommand cmdLoss = new SqlCommand(getLoss, con);

        SqlDataAdapter daClass = new SqlDataAdapter(cmdClass);
        SqlDataAdapter daLoc = new SqlDataAdapter(cmdLoc);
        SqlDataAdapter daLocD = new SqlDataAdapter(cmdLocD);
        SqlDataAdapter daPB = new SqlDataAdapter(cmdPB);
        SqlDataAdapter daLoss = new SqlDataAdapter(cmdLoss);

        DataTable dtClass = new DataTable();
        DataTable dtLoc = new DataTable();
        DataTable dtLocD = new DataTable();
        DataTable dtPB = new DataTable();
        DataTable dtLoss = new DataTable();

        daClass.Fill(dtClass);
        daLoc.Fill(dtLoc);
        daLocD.Fill(dtLocD);
        daPB.Fill(dtPB);
        daLoss.Fill(dtLoss);

        dsTotal.Tables.Add(dtClass);
        dsTotal.Tables.Add(dtLoc);
        dsTotal.Tables.Add(dtLocD);
        dsTotal.Tables.Add(dtPB);
        dsTotal.Tables.Add(dtLoss);

        dsTotal.AcceptChanges();
        
        return dsTotal;
    }
    
    public void postIssue(Issue issue)
    {
        Int32 issue_identity;
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection=con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "insert into Issue(Name_Issue, ID_LocationD, PIC, Time_Start,  Deadline,"
        +
        " Status, ID_Classify, Picture, ID_Loss, Content) output inserted.ID_Issue"
        +
        " values(@Name_Issue, @ID_LocationD, @PIC, @Time_Start, @Deadline,"
        +
        " @Status, @ID_Classify, @Picture, @ID_Loss, @Content)";
        
        cmd.Parameters.AddWithValue("@Name_Issue", issue.Name_issue);
        cmd.Parameters.AddWithValue("@ID_LocationD", issue.LocationD_ID);
        cmd.Parameters.AddWithValue("@PIC", issue.PIC);
        cmd.Parameters.AddWithValue("@Time_Start", issue.Time_Start);
        //cmd.Parameters.AddWithValue("@Time_End", issue.Time_End);
        cmd.Parameters.AddWithValue("@Deadline", issue.Deadline);
        cmd.Parameters.AddWithValue("@Status", "Pending");
        cmd.Parameters.AddWithValue("@ID_Classify", issue.ID_Classify);
        cmd.Parameters.AddWithValue("@Picture", issue.Picture);
        cmd.Parameters.AddWithValue("@ID_Loss", issue.ID_Loss);
        cmd.Parameters.AddWithValue("@Content", issue.Content);

        con.Open();
        SqlDataAdapter da_issue = new SqlDataAdapter(cmd);
        DataTable dt_isue = new DataTable();
        //cmd.ExecuteNonQuery();
        da_issue.Fill(dt_isue);
        issue_identity = Convert.ToInt32(dt_isue.Rows[0][0]);
        con.Close();

        SqlCommand cmd_imp = new SqlCommand();
        cmd_imp.Connection=con;
        cmd_imp.CommandType = CommandType.Text;
        cmd_imp.CommandText = "insert into Improve_Issue(ID_Issue, Status, Team_Improve) values(@id_issue, @status, @team_imp)";
        foreach(var item in issue.improvement)
        {
            cmd_imp.Parameters.Clear();
            cmd_imp.Parameters.AddWithValue("@id_issue", issue_identity);
            cmd_imp.Parameters.AddWithValue("@status", "Pending");
            cmd_imp.Parameters.AddWithValue("@team_imp", item);
            con.Open();
            cmd_imp.ExecuteNonQuery();
            con.Close();
        }
    }

    public void PostImprovment(Improvement improvement)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        cmd.CommandText ="update Improve_Issue set  Title=@Title, Picture=@Picture, Time_Improve=@Time_Improve,  Content=@Content "
        + "where ID_Issue=@ID_Issue and Team_Improve = @Team_imp";
        cmd.Parameters.AddWithValue("@ID_Issue", improvement.ID_Issue);
        cmd.Parameters.AddWithValue("@Team_imp", improvement.Team_Improve);
        cmd.Parameters.AddWithValue("@Title", improvement.Title);
        cmd.Parameters.AddWithValue("@Picture", improvement.Picture);
        cmd.Parameters.AddWithValue("@Time_Improve", improvement.Time_Improved);
        cmd.Parameters.AddWithValue("@Content", improvement.Content);

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }
    public void PostUser(_User user)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType=CommandType.Text;
        cmd.Connection = con;
        cmd.CommandText = "insert into [User] (ID_User, Name_User, Permission, ID_Department, Password)"
        +"values (@ID_User, @Name_User, @Permission, @ID_Dept, @Password)";
        cmd.Parameters.AddWithValue("@ID_User", user.ID_User);
        cmd.Parameters.AddWithValue("@Name_User", user.Name_User);
        cmd.Parameters.AddWithValue("@Permission", user.Permission);
        cmd.Parameters.AddWithValue("@ID_Dept", user.ID_Dept);
        cmd.Parameters.AddWithValue("@Password", user.Password);

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }
    public string Update_Issue(Issue issue)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlConnection connect_Imprv = new SqlConnection(connectionString);
        try
        {
            SqlCommand cmdUpdate = new SqlCommand();
            cmdUpdate.Connection = con;
            cmdUpdate.CommandType=CommandType.Text;
            cmdUpdate.CommandText = 
            "update Issue set Name_Issue = @name_issue, "
            + " ID_LocationD=@id_locd, Deadline = @deadline, ID_Classify = @id_class, "
            + " Picture = @picture, ID_Loss=@id_loss, Content=@content "
            + " where ID_Issue = @id_issue";
            cmdUpdate.Parameters.AddWithValue("@name_issue", issue.Name_issue);
            cmdUpdate.Parameters.AddWithValue("@id_locd", issue.LocationD_ID);
            cmdUpdate.Parameters.AddWithValue("@deadline", issue.Deadline);
            cmdUpdate.Parameters.AddWithValue("@id_class", issue.ID_Classify);
            cmdUpdate.Parameters.AddWithValue("@picture", issue.Picture);
            cmdUpdate.Parameters.AddWithValue("@id_loss", issue.ID_Loss);
            cmdUpdate.Parameters.AddWithValue("@content", issue.Content);
            cmdUpdate.Parameters.AddWithValue("@id_issue", issue.ID_Issue);
            con.Open();
            cmdUpdate.ExecuteNonQuery();
            con.Close();

            SqlCommand cmdGetDeptImp = new SqlCommand();
            cmdGetDeptImp.Connection = connect_Imprv;
            cmdGetDeptImp.CommandType = CommandType.Text;
            cmdGetDeptImp.CommandText = "select * from Improve_Issue where ID_Issue= @id_issue";
            cmdGetDeptImp.Parameters.AddWithValue("@id_issue", issue.ID_Issue);
            SqlDataAdapter daGetDept = new SqlDataAdapter(cmdGetDeptImp);
            DataTable dtGetDept = new DataTable();
            connect_Imprv.Open();
            daGetDept.Fill(dtGetDept);
            connect_Imprv.Close();

            List<int> deptNameKeep = new List<int>();
            List<int> deptNameRemove = new List<int>();

            for(int i=0; i<dtGetDept.Rows.Count;i++)
            {
                if(dtGetDept.Rows[i]["Status"].ToString().ToLower() != "reject" && dtGetDept.Rows[i]["Time_Improve"].ToString() != "")
                {
                    deptNameKeep.Add(Convert.ToInt32(dtGetDept.Rows[i]["Team_Improve"]));
                }
                else if(dtGetDept.Rows[i]["Status"].ToString().ToLower() == "reject" || dtGetDept.Rows[i]["Time_Improve"].ToString() == "")
                {
                    deptNameRemove.Add(Convert.ToInt32(dtGetDept.Rows[i]["Team_Improve"]));
                }
            }


            SqlCommand cmdClearDB = new SqlCommand();
            cmdClearDB.Connection = con;
            cmdClearDB.CommandType = CommandType.Text;
            cmdClearDB.CommandText = "delete from Improve_Issue where ID_Issue = @id_issue and Team_Improve = @team_imp";

             if(deptNameRemove.Count>0)
            {
                 foreach(var item in deptNameRemove)
                {
                    cmdClearDB.Parameters.Clear();
                    cmdClearDB.Parameters.AddWithValue("@id_issue", issue.ID_Issue);
                    cmdClearDB.Parameters.AddWithValue("@team_imp", item);
                    con.Open();
                    cmdClearDB.ExecuteNonQuery();
                    con.Close();
                }
            }
           
            SqlCommand cmdUpdateImp = new SqlCommand();
            cmdUpdateImp.Connection = con;
            cmdUpdateImp.CommandType = CommandType.Text;
            cmdUpdateImp.CommandText = "insert into Improve_Issue(ID_Issue, Status, Team_Improve) values(@id_issue, @status, @team_imp)";
            if(issue.improvement.Count>0)
            {
                foreach(var item in issue.improvement)
                {
                    var flag = 0;
                    for(int k=0; k<deptNameKeep.Count; k++)
                    {
                        if(item==deptNameKeep[k])
                        {
                            flag=1;
                        }
                    }
                    if(flag==0)
                    {
                        cmdUpdateImp.Parameters.Clear();
                        cmdUpdateImp.Parameters.AddWithValue("@id_issue", issue.ID_Issue);
                        cmdUpdateImp.Parameters.AddWithValue("@status", "Pending");
                        cmdUpdateImp.Parameters.AddWithValue("@team_imp", item);
                        con.Open();
                        cmdUpdateImp.ExecuteNonQuery();
                        con.Close();
                    } 
                    else if(flag==1)
                    {
                        Console.WriteLine("pass");
                    }
                }
            }

            return("OK");
            
        }
        catch(Exception ex)
        {
            return(ex.Message);
            //con.Close();
        }
    }
    public DataTable Login_User(Login_User login_)
    {
        DataTable dtResult_Check = new DataTable();
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = 
        "if exists (select * from [User] where ID_User = @ID and Password =@PWD)"
	        +"begin "
                +"select a.ID_Function, b.Name_Function, c.ID_Department from Permission as a " 
                +"inner join [Function] as b on a.ID_Function = b.ID_Function "
                +"inner join [User] as c on c.ID_User = a.ID_User "
                +"where a.ID_User=@ID and a.Status=1 and b.Type = 'Mobile' "
	        +"end";
        cmd.Parameters.AddWithValue("@ID", login_.ID_User);
        cmd.Parameters.AddWithValue("@PWD", login_.Password);
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dtResult_Check);
        con.Close();
        return dtResult_Check;
    }

    //Trace Issue
    public DataTable generalTraceIssue(int numberRecord)
    {
        DataTable dtGeneralIssue = new DataTable();
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = 
        "select top (@numRec) a.*, b.Name_LocationDetail, c.Name_Classify, d.Name_Level "
        +"from Issue as a "
        +"inner join Location_Detail as b on a.ID_LocationD = b.ID_LocationD "
        +"inner join Classify as c on a.ID_Classify = c.ID_Classify "
        +"inner join [Level] as d on a.ID_Loss = d.ID_Level "
        +"order by ID_Issue desc";
        cmd.Parameters.AddWithValue("@numRec", numberRecord);
        SqlDataAdapter daIT = new SqlDataAdapter(cmd);
        con.Open();
        daIT.Fill(dtGeneralIssue);
        con.Close();
        return dtGeneralIssue;
    }

    public DataTable Trace_Imp_by_Issue(int ID_Issue)
    {
        DataTable dtImpOnIssue = new DataTable();
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select a.*, b.Name_Department"
        +" from Improve_Issue as a inner join Department as b on a.Team_improve = b.ID_Department"
        +" where ID_Issue = @ID_Issue";
        cmd.Parameters.AddWithValue("@ID_Issue", ID_Issue);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        con.Open();
        dataAdapter.Fill(dtImpOnIssue);
        con.Close();
        return dtImpOnIssue;
    }
    public DataTable Trace_Imp_by_Imp_ID(int ID_Imp)
    {
        DataTable dtImpOnImp = new DataTable();
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmdImp = new SqlCommand();
        cmdImp.Connection = con;
        cmdImp.CommandType = CommandType.Text;
        cmdImp.CommandText = "select a.*, b.Name_Department " +
        "from Improve_Issue as a inner join Department as b on a.Team_improve "+
        " = b.ID_Department where a.ID_Improve = @ID_Imp";
        cmdImp.Parameters.AddWithValue("@ID_Imp", ID_Imp);
        SqlDataAdapter daImp = new SqlDataAdapter(cmdImp);
        con.Open();
        daImp.Fill(dtImpOnImp);
        con.Close();
        return dtImpOnImp;
    }
    public string Improve_Decision(Improvement improvement)
    {
        try{
            SqlConnection con = new SqlConnection(connectionString);
                SqlConnection con2 = new SqlConnection(connectionString);
                SqlCommand cmdExec = new SqlCommand();
                //SqlCommand cmdReImp = new SqlCommand();
                cmdExec.CommandType=CommandType.Text;
                cmdExec.Connection = con;
                cmdExec.CommandText = "update Improve_Issue set Status = @status where ID_Improve =@id_imp";

                SqlCommand cmdReImp = new SqlCommand();
                cmdReImp.Connection = con2;
                cmdReImp.CommandType = CommandType.Text;
                cmdReImp.CommandText = "insert into Improve_Issue(ID_Issue, Status, Team_Improve) values(@id, @status, @team)";

                SqlCommand cmdCleanPending = new SqlCommand();
                cmdCleanPending.Connection = con;
                cmdCleanPending.CommandType = CommandType.Text;
                cmdCleanPending.CommandText = "delete from Improve_Issue where ID_Improve = @ID";
                //cmdCleanPending.Parameters.AddWithValue("@ID", improvement.ID_Improve);

                
                if(improvement.Status.ToString().ToLower()=="approve")
                {
                    cmdExec.Parameters.AddWithValue("@id_imp", improvement.ID_Improve);
                    cmdExec.Parameters.AddWithValue("@status", "Approve");
                    con.Open();
                    cmdExec.ExecuteNonQuery();
                    con.Close();
                    
                    SqlCommand cmdGetPendingKey = new SqlCommand();
                    cmdGetPendingKey.Connection = con2;
                    cmdGetPendingKey.CommandType = CommandType.Text;
                    cmdGetPendingKey.CommandText =
                    "select ID_Improve from Improve_Issue"+
                    " where ID_Issue = @id_issue and Team_Improve = @team"+
                    " and Status ='Pending'";
                    cmdGetPendingKey.Parameters.AddWithValue("@id_issue",improvement.ID_Issue);
                    cmdGetPendingKey.Parameters.AddWithValue("@team", improvement.Team_Improve);
                    SqlDataAdapter daGetPendingKey = new SqlDataAdapter(cmdGetPendingKey);
                    DataTable dtGetPending = new DataTable();
                    daGetPendingKey.Fill(dtGetPending);

                    List<int> impID_pending = new List<int>();

                    for(int i=0; i<dtGetPending.Rows.Count;i++)
                    {
                        impID_pending.Add(Convert.ToInt32(dtGetPending.Rows[i]["ID_Improve"]));
                    }

                    if(impID_pending.Count>0)
                    {
                        //code
                        for(int j=0; j<impID_pending.Count;j++)
                        {
                            cmdCleanPending.Parameters.Clear();
                            cmdCleanPending.Parameters.AddWithValue("@ID", impID_pending[j]);
                            con.Open();
                            cmdCleanPending.ExecuteNonQuery();
                            con.Close();
                        }
                    }
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
        catch(Exception ex)
        {
            return(ex.Message.ToString());
        }
    }
    public DataTable deptImprove(int ID_Issue)
    {
        DataTable dtDept = new DataTable();
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType= CommandType.Text;
        cmd.CommandText = " select a.Team_Improve,b.Name_Department"
        +" from Improve_Issue as a"
        +" inner join Department as b"
        +" on a.Team_Improve = b.ID_Department "
        +" where a.ID_Issue = @id_issue and not a.Status = 'Rejected' ";
        cmd.Parameters.AddWithValue("@id_issue", ID_Issue);
        SqlDataAdapter daDept = new SqlDataAdapter(cmd);
        con.Open();
        daDept.Fill(dtDept);
        con.Close();
        
        DataTable dtFixDup = new DataTable();

        return dtDept;
    }
    public DataTable searchIssue(Issue issue)
    {
        DataTable dtIssueSearch = new DataTable();
        string strSQL = "select a.*,  b.Name_LocationDetail, c.Name_Classify, d.Name_Level  "
        +" from Issue as a "
        +"inner join Location_Detail as b on a.ID_LocationD = b.ID_LocationD "
        +"inner join Classify as c on a.ID_Classify = c.ID_Classify "
        +"inner join [Level] as d on a.ID_Loss = d.ID_Level "
        +" where 1=1 and a.Time_Start between '"
        + issue.Time_Start + "' and '" + issue.Time_Until + "' ";

        //Condition check per selection
        if(issue.ID_Classify != 0)
        {
            strSQL = strSQL + " and a.ID_Classify = "+ issue.ID_Classify;
        }
        if(issue.ID_Loss != 0)
        {
            strSQL = strSQL + " and a.ID_Loss = " + issue.ID_Loss;
        }
        if(issue.Status != "none")
        {
            strSQL = strSQL + " and a.Status = "+ "'"+ issue.Status+"' ";
        }
        if(issue.LocationD_ID != 0)
        {
            strSQL = strSQL + " and a.ID_LocationD = " + issue.LocationD_ID;
        }

        strSQL = strSQL + " order by a.ID_Issue desc ";


        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmdISSUE = new SqlCommand();
        cmdISSUE.Connection = con;
        cmdISSUE.CommandType= CommandType.Text;
        cmdISSUE.CommandText= strSQL;
        SqlDataAdapter daSQL = new SqlDataAdapter(cmdISSUE);
        con.Open();
        daSQL.Fill(dtIssueSearch);
        con.Close();
        return dtIssueSearch;
    }
    public DataTable searchNameIssue(string name)
    {
        DataTable dtIssue = new DataTable();
        
        string strSQL = "select a.*,  b.Name_LocationDetail, c.Name_Classify, d.Name_Level  "
        +" from Issue as a "
        +"inner join Location_Detail as b on a.ID_LocationD = b.ID_LocationD "
        +"inner join Classify as c on a.ID_Classify = c.ID_Classify "
        +"inner join [Level] as d on a.ID_Loss = d.ID_Level "
        +"where a.Name_Issue like '%" +name+"%'" ;

        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmdSN = new SqlCommand();
        cmdSN.Connection=con;
        cmdSN.CommandType= CommandType.Text;
        cmdSN.CommandText= strSQL;
        SqlDataAdapter daSN = new SqlDataAdapter(cmdSN);
        con.Open();
        daSN.Fill(dtIssue);
        con.Close();
        return dtIssue;
    }
    public DataTable searchID_Issue(int ID_Issue)
    {
        DataTable dtIssue = new DataTable();

        string strSQL = "select a.*,  b.Name_LocationDetail, c.Name_Classify, d.Name_Level  "
        +" from Issue as a "
        +"inner join Location_Detail as b on a.ID_LocationD = b.ID_LocationD "
        +"inner join Classify as c on a.ID_Classify = c.ID_Classify "
        +"inner join [Level] as d on a.ID_Loss = d.ID_Level "
        +"where a.ID_Issue =@id_issue" ;

        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmdSN = new SqlCommand();
        cmdSN.Parameters.AddWithValue("@id_issue", ID_Issue);
        cmdSN.Connection=con;
        cmdSN.CommandType= CommandType.Text;
        cmdSN.CommandText= strSQL;
        SqlDataAdapter daSN = new SqlDataAdapter(cmdSN);
        con.Open();
        daSN.Fill(dtIssue);
        con.Close();
        
        return dtIssue;
    }
}

