using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

public partial class Admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 检查是否已登录且是管理员
            if (!IsAdminUser())
            {
                // 如果不是管理员，重定向到首页
                Response.Redirect("~/Default.aspx?message=权限不足，只有管理员可以访问后台");

            }
        }
    }

    private bool IsAdminUser()
    {
        try
        {
            // 检查用户是否已认证
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
                return false;
            }

            string username = User.Identity.Name;

            // 先检查Session
            string sessionRole = Session["CurrentRole"]?.ToString();
            if (!string.IsNullOrEmpty(sessionRole))
            {
                if (sessionRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // 检查数据库
            string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Role FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string dbRole = result.ToString();

                    // 更新Session
                    Session["CurrentRole"] = dbRole;

                    return dbRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"后台权限检查错误: {ex.Message}");
            return false;
        }
    }
}