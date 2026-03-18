using System;
using System.Data.SqlClient;
using System.Configuration;
using Utilities;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }

    protected void LogIn(object sender, EventArgs e)
    {
        if (IsValid)
        {
            string username = UserName.Text.Trim();
            string password = Password.Text.Trim();

            System.Diagnostics.Debug.WriteLine($"登录尝试: 用户名={username}");

            // 验证用户凭据
            var userInfo = ValidateUser(username, password);

            if (userInfo != null)
            {
                System.Diagnostics.Debug.WriteLine($"密码验证成功，用户角色: {userInfo.Role}");

                // 创建FormsAuthentication票据
                System.Web.Security.FormsAuthenticationTicket ticket = new System.Web.Security.FormsAuthenticationTicket(
                    1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    RememberMe.Checked,
                    userInfo.Role,
                    System.Web.Security.FormsAuthentication.FormsCookiePath);

                string encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);

                // 创建Cookie
                System.Web.HttpCookie authCookie = new System.Web.HttpCookie(
                    System.Web.Security.FormsAuthentication.FormsCookieName,
                    encryptedTicket);

                if (RememberMe.Checked)
                {
                    authCookie.Expires = ticket.Expiration;
                }

                authCookie.Path = System.Web.Security.FormsAuthentication.FormsCookiePath;
                Response.Cookies.Add(authCookie);

                System.Diagnostics.Debug.WriteLine($"认证Cookie已创建，用户名: {username}");

                // 设置Session
                Session["CurrentUser"] = username;
                Session["CurrentRole"] = userInfo.Role;

                // 重定向
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Response.Redirect(returnUrl);
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            else
            {
                FailureText.Text = "用户名或密码错误！";
                ErrorMessage.Visible = true;
            }
        }
    }

    private UserInfo ValidateUser(string username, string password)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "SELECT Password, Salt, Role FROM Users WHERE Username = @Username";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = reader["Password"].ToString();
                    string storedSalt = reader["Salt"].ToString();
                    string role = reader["Role"].ToString();

                    reader.Close();

                    // 验证密码（使用哈希加盐验证）
                    if (PasswordHasher.VerifyPassword(password, storedHash, storedSalt))
                    {
                        System.Diagnostics.Debug.WriteLine($"密码验证成功 - 用户名: {username}");
                        return new UserInfo { Username = username, Role = role };
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"密码验证失败 - 用户名: {username}");
                        return null;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"用户不存在: {username}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"数据库查询错误: {ex.Message}");
                return null;
            }
        }
    }

    // 用户信息类
    private class UserInfo
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}