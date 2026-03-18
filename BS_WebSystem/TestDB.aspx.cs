using System;
using System.Data.SqlClient;
using System.Configuration;

public partial class TestDB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;
            Response.Write("连接字符串: " + connStr + "<br>");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Response.Write("数据库连接成功！<br>");

                // 查询用户表
                string sql = "SELECT COUNT(*) FROM Users";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int count = (int)cmd.ExecuteScalar();
                Response.Write("Users表中有 " + count + " 条记录<br>");

                // 查询具体用户
                sql = "SELECT Username, Password, Role FROM Users";
                cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Response.Write("<h3>用户列表:</h3>");
                Response.Write("<table border='1'><tr><th>用户名</th><th>密码</th><th>角色</th></tr>");
                while (reader.Read())
                {
                    Response.Write("<tr>");
                    Response.Write("<td>" + reader["Username"] + "</td>");
                    Response.Write("<td>" + reader["Password"] + "</td>");
                    Response.Write("<td>" + reader["Role"] + "</td>");
                    Response.Write("</tr>");
                }
                Response.Write("</table>");
                reader.Close();
            }
        }
        catch (Exception ex)
        {
            Response.Write("错误: " + ex.Message + "<br>");
            Response.Write("堆栈: " + ex.StackTrace + "<br>");
        }
    }
}