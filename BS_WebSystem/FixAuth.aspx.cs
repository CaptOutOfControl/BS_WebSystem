using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;

public partial class FixAuth : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DisplayDiagnosticInfo();
            CheckWebConfig();
            DisplayCookies();
        }
    }

    private void DisplayDiagnosticInfo()
    {
        sessionUser.InnerText = Session["CurrentUser"]?.ToString() ?? "null";
        sessionRole.InnerText = Session["CurrentRole"]?.ToString() ?? "null";
        identityName.InnerText = User.Identity.Name ?? "null";
        isAuthenticated.InnerText = User.Identity.IsAuthenticated.ToString();
    }

    private void CheckWebConfig()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        try
        {
            // 获取FormsAuthentication配置
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            AuthenticationSection authSection = (AuthenticationSection)config.GetSection("system.web/authentication");

            if (authSection != null && authSection.Mode == AuthenticationMode.Forms)
            {
                FormsAuthenticationConfiguration formsConfig = authSection.Forms;

                sb.Append("<div class='table-responsive'>");
                sb.Append("<table class='table table-bordered table-sm'>");
                sb.Append("<tbody>");

                sb.AppendFormat("<tr><th width='30%'>LoginUrl</th><td>{0}</td></tr>", formsConfig.LoginUrl);
                sb.AppendFormat("<tr><th>DefaultUrl</th><td>{0}</td></tr>", formsConfig.DefaultUrl);
                sb.AppendFormat("<tr><th>Cookie名称</th><td>{0}</td></tr>", formsConfig.Name);
                sb.AppendFormat("<tr><th>Timeout(分钟)</th><td>{0}</td></tr>", formsConfig.Timeout.TotalMinutes);
                sb.AppendFormat("<tr><th>Path</th><td>{0}</td></tr>", formsConfig.Path);
                sb.AppendFormat("<tr><th>Protection</th><td>{0}</td></tr>", formsConfig.Protection);
                sb.AppendFormat("<tr><th>SlidingExpiration</th><td>{0}</td></tr>", formsConfig.SlidingExpiration);
                sb.AppendFormat("<tr><th>Cookieless</th><td>{0}</td></tr>", formsConfig.Cookieless);
                sb.AppendFormat("<tr><th>RequireSSL</th><td>{0}</td></tr>", formsConfig.RequireSSL);

                sb.Append("</tbody></table></div>");
            }
            else
            {
                sb.Append("<div class='alert alert-danger'>未找到FormsAuthentication配置</div>");
            }
        }
        catch (Exception ex)
        {
            sb.AppendFormat("<div class='alert alert-danger'>读取配置失败: {0}</div>", ex.Message);
        }

        litConfigCheck.Text = sb.ToString();
    }

    private void DisplayCookies()
    {
        List<CookieInfo> cookieList = new List<CookieInfo>();

        foreach (string cookieName in Request.Cookies.AllKeys)
        {
            HttpCookie cookie = Request.Cookies[cookieName];
            cookieList.Add(new CookieInfo
            {
                Name = cookieName,
                Value = cookie.Value,
                Domain = cookie.Domain ?? "null",
                Path = cookie.Path ?? "null",
                Expires = cookie.Expires == DateTime.MinValue ? "Session Cookie" : cookie.Expires.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        if (cookieList.Count > 0)
        {
            rptCookies.DataSource = cookieList;
            rptCookies.DataBind();
            lblNoCookies.Visible = false;
        }
        else
        {
            rptCookies.Visible = false;
            lblNoCookies.Visible = true;
        }
    }

    protected void btnManualAuth_Click(object sender, EventArgs e)
    {
        string username = Session["CurrentUser"]?.ToString();
        string role = Session["CurrentRole"]?.ToString() ?? "User";

        if (string.IsNullOrEmpty(username))
        {
            ShowResult("Session中没有用户信息", false);
            return;
        }

        try
        {
            // 1. 创建FormsAuthentication票据
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                version: 1,
                name: username,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(30),
                isPersistent: false,
                userData: role,
                cookiePath: FormsAuthentication.FormsCookiePath);

            // 2. 加密票据
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // 3. 创建Cookie
            HttpCookie authCookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                encryptedTicket);

            authCookie.Path = FormsAuthentication.FormsCookiePath;
            authCookie.HttpOnly = true;

            // 重要：设置SameSite属性（现代浏览器需要）
            if (authCookie.SameSite == SameSiteMode.None && Request.Url.Scheme == "https")
            {
                authCookie.SameSite = SameSiteMode.Lax;
            }

            // 4. 添加到响应
            Response.Cookies.Add(authCookie);

            // 5. 强制刷新认证
            FormsAuthentication.SetAuthCookie(username, false);

            ShowResult($"已为用户 {username} 创建认证票据。票据已加密，角色: {role}。请刷新页面查看状态。", true);

            // 记录日志
            System.Diagnostics.Debug.WriteLine($"手动创建认证票据 - 用户: {username}, 角色: {role}");
        }
        catch (Exception ex)
        {
            ShowResult($"创建认证票据失败: {ex.Message}", false);
        }
    }

    protected void btnTestAuth_Click(object sender, EventArgs e)
    {
        string username = "testuser";
        string password = "test123";

        try
        {
            // 1. 先清除现有认证
            FormsAuthentication.SignOut();

            // 2. 验证用户（简化版，直接检查数据库）
            string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;
            bool userExists = false;
            string userRole = "User";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT COUNT(*), Role FROM Users WHERE Username = @Username AND Password = @Password GROUP BY Role";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userExists = Convert.ToInt32(reader[0]) > 0;
                    userRole = reader[1].ToString();
                }
                reader.Close();
            }

            if (userExists)
            {
                // 3. 使用FormsAuthentication的标准方法
                FormsAuthentication.SetAuthCookie(username, false);

                // 4. 设置Session
                Session["CurrentUser"] = username;
                Session["CurrentRole"] = userRole;

                ShowResult($"测试认证成功！用户名: {username}, 角色: {userRole}。请刷新页面查看认证状态。", true);
            }
            else
            {
                // 创建测试用户
                CreateTestUser(username, password);
                FormsAuthentication.SetAuthCookie(username, false);
                Session["CurrentUser"] = username;
                Session["CurrentRole"] = "User";

                ShowResult($"已创建测试用户并登录！用户名: {username}, 密码: {password}。请刷新页面查看认证状态。", true);
            }
        }
        catch (Exception ex)
        {
            ShowResult($"测试认证失败: {ex.Message}", false);
        }
    }

    protected void btnClearAndLogin_Click(object sender, EventArgs e)
    {
        // 1. 清除所有认证信息
        FormsAuthentication.SignOut();
        Session.Clear();
        Session.Abandon();

        // 2. 清除所有Cookies
        foreach (string cookieName in Request.Cookies.AllKeys)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }

        // 3. 重定向到登录页面
        Response.Redirect("~/Account/Login.aspx");
    }

    private void CreateTestUser(string username, string password)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            // 检查用户是否已存在
            string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlCommand checkCmd = new SqlCommand(checkSql, conn);
            checkCmd.Parameters.AddWithValue("@Username", username);

            conn.Open();
            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (count == 0)
            {
                // 创建用户
                string insertSql = @"
                    INSERT INTO Users (Username, Password, Email, Role, CreatedDate) 
                    VALUES (@Username, @Password, @Email, 'User', GETDATE())";

                SqlCommand insertCmd = new SqlCommand(insertSql, conn);
                insertCmd.Parameters.AddWithValue("@Username", username);
                insertCmd.Parameters.AddWithValue("@Password", password);
                insertCmd.Parameters.AddWithValue("@Email", username + "@test.com");

                insertCmd.ExecuteNonQuery();
            }
        }
    }

    private void ShowResult(string message, bool isSuccess)
    {
        pnlResult.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
        pnlResult.Visible = true;
        litResult.Text = message;
    }

    // 截断长字符串
    public string TruncateValue(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return "";
        return value.Length > maxLength ? value.Substring(0, maxLength) + "..." : value;
    }

    // Cookie信息类
    public class CookieInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Expires { get; set; }
    }
}