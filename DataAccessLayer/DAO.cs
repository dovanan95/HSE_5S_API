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
        cmd.CommandText = "select * from [User]";
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
    public void postIssue(Issue issue)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "insert into Issue(Name_Issue, ID_Location, Team_Input, PIC, Time_Start, Time_End, Deadline,"
        +
        "Status, ID_Classify, Picture, ID_Loss, Content) value(@Name_Issue, @ID_Location, @Team_Input, @PIC, @Time_Start, @Time_End, @Deadline,"
        +
        "@Status, @ID_Classify, @Picture, @ID_Loss, @Content)";
        cmd.Parameters.AddWithValue("@Name_Issue", issue.Name_issue);
        cmd.Parameters.AddWithValue("@ID_Location", issue.Location_ID);
        cmd.Parameters.AddWithValue("@Team_Input", issue.Team_input);
        cmd.Parameters.AddWithValue("@PIC", issue.PIC);
        cmd.Parameters.AddWithValue("@Time_Start", issue.Time_Start);
        cmd.Parameters.AddWithValue("@Time_End", issue.Time_End);
        cmd.Parameters.AddWithValue("@Deadline", issue.Deadline);
        cmd.Parameters.AddWithValue("@Status", issue.Status);
        cmd.Parameters.AddWithValue("@ID_Classify", issue.ID_Classify);
        cmd.Parameters.AddWithValue("@Picture", issue.Picture);
        cmd.Parameters.AddWithValue("@ID_Loss", issue.ID_Loss);
        cmd.Parameters.AddWithValue("@Content", issue.Content);

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

    }

    public void PostImprovment(Improvement improvement)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        cmd.CommandText ="insert into Improve_Issue (ID_Issue, Title, Picture, Time_Improve, Status, Content)"
        + "values(@ID_Issue, @Title, @Picture, @Time_Improve, @Status, @Content)";
        cmd.Parameters.AddWithValue("@ID_Issue", improvement.ID_Issue);
        cmd.Parameters.AddWithValue("@Title", improvement.Title);
        cmd.Parameters.AddWithValue("@Picture", improvement.Picture);
        cmd.Parameters.AddWithValue("@Time_Improve", improvement.Time_Improved);
        cmd.Parameters.AddWithValue("@Status", improvement.Status);
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
                +"select a.ID_Function, b.Name_Function from Permission as a " 
                +"inner join [Function] as b on a.ID_Function = b.ID_Function "
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
}

