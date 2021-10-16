using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text.Json;
using System.Data;

public class Improvement
{
    public int ID_Improve{get;set;}
    public int ID_Issue{get;set;}
    public string Title{get;set;}
    public string Time_Improved{get;set;}
    public string Content{get;set;}
    public string Picture{get;set;}
    public int Team_Improve{get;set;}
    public string Status{get;set;}

    public DataTable GetFromDB(string sqlQueryString, List<Improvement> param)
    {
        string connect = DAO.connectionString;
        DataTable dtGetDB = new DataTable();
        SqlConnection con = new SqlConnection(connect);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = sqlQueryString;
        int param_count = 0;
        for(int i=0;i<sqlQueryString.Length;i++)
        {
            if(sqlQueryString[i].ToString()=="@")
            {
                param_count++;
            }
        }
        if(param_count==0)
        {
            con.Open();
            SqlDataAdapter daGet = new SqlDataAdapter(cmd);
            daGet.Fill(dtGetDB);
            con.Close();
        }
        else if(param_count>0)
        {
            for(int j=0;j<param_count;j++)
            {
                cmd.Parameters.AddWithValue("@"+(j+1).ToString(),param[j]);
            }
            con.Open();
            SqlDataAdapter daGet = new SqlDataAdapter(cmd);
            daGet.Fill(dtGetDB);
            con.Close();
        }
        return dtGetDB;
    }
}