using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text.Json;
using System.Data;
public class Issue
{
    public int ID_Issue{get;set;}
    public string Name_issue{get;set;}
    public int LocationD_ID{get;set;}
    public string PIC{get;set;}
    public string Time_Start{get;set;}
    public string Time_Until{get;set;}
    public string Time_End{get;set;}
    public string Deadline{get;set;}
    public string Status{get;set;}
    public int ID_Classify{get;set;}
    public string Picture{get;set;}
    public int ID_Loss{get;set;}
    public string Content{get;set;}
    public List<int> improvement{get;set;}

    public DataTable GetFromDB(string sqlQueryString, List<Issue> param)
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