using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web;
using System.Xml.Linq;

public partial class DebugLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DisplayDebugInfo();
            CheckAndSuggestFix();
        }
    }

    private void DisplayDebugInfo()
    {
        // User.Identity 信息
        tdIsAuthenticated.InnerText = User.Identity.IsAuthenticated.ToString();
        tdName.InnerText = User.Identity.Name ?? "null";
        tdAuthenticationType.InnerText = User.Identity.AuthenticationType ?? "null";
        tdIdentityType.InnerText = User.Identity.GetType().Name;

        // Session 信息
        tdSessionID.InnerText = Session.SessionID ?? "null";
        tdCurrentUser.InnerText = Session["CurrentUser"]?.ToString() ?? "null";
        tdCurrentRole.InnerText = Session["CurrentRole"]?.ToString() ?? "null";
        tdSessionState.InnerText = Session.Mode + " (Timeout: " + Session.Timeout + "分钟)";

        // Cookies 信息
        var cookieList = new List<CookieInfo>();
        foreach (string cookieName in Request.Cookies.AllKeys)
        {
            var cookie = Request.Cookies[cookieName];
            cookieList.Add(new CookieInfo
            {
                Key = cookieName,
                Value = cookie.Value,
                Expires = cookie.Expires.ToString("yyyy-MM-dd HH:mm:ss") + " (已过期: " +
                         (cookie.Expires < DateTime.Now ? "是" : "否") + ")"
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

        // FormsAuthentication 信息
        tdFormsCookieName.InnerText = FormsAuthentication.FormsCookieName;
        tdLoginUrl.InnerText = FormsAuthentication.LoginUrl;
        tdDefaultUrl.InnerText = FormsAuthentication.DefaultUrl;
        tdCookieMode.InnerText = FormsAuthentication.CookieMode.ToString();
    }

    private void CheckAndSuggestFix()
    {
        bool needsFix = false;
        List<string> suggestions = new List<string>();

        // 检查问题
        if (!User.Identity.IsAuthenticated && Session["CurrentUser"] != null)
        {
            needsFix = true;
            suggestions.Add("Session中有用户信息，但User.Identity未认证");
        }

        if (User.Identity.IsAuthenticated && Session["CurrentUser"] == null)
        {
            needsFix = true;
            suggestions.Add("User.Identity已认证，但Session中没有用户信息");
        }

        // 检查认证Cookie
        bool hasAuthCookie = false;
        foreach (string cookieName in Request.Cookies.AllKeys)
        {
            if (cookieName == FormsAuthentication.FormsCookieName)
            {
                hasAuthCookie = true;
                break;
            }
        }

        if (!hasAuthCookie && User.Identity.IsAuthenticated)
        {
            needsFix = true;
            suggestions.Add("User.Identity已认证，但没有认证Cookie");
        }

        if (hasAuthCookie && !User.Identity.IsAuthenticated)
        {
            needsFix = true;
            suggestions.Add("有认证Cookie，但User.Identity未认证");
        }

        if (needsFix)
        {
            pnlFixSuggestions.Visible = true;
            fixSuggestions.InnerHtml = "<ul class='mb-0'>";
            foreach (string suggestion in suggestions)
            {
                fixSuggestions.InnerHtml += $"<li>{suggestion}</li>";
            }
            fixSuggestions.InnerHtml += "</ul>";
        }
        else
        {
            pnlFixSuggestions.Visible = false;
        }
    }

    protected void btnFixLogin_Click(object sender, EventArgs e)
    {
        // 方案1：如果Session中有用户，但没有认证，则创建认证
        if (Session["CurrentUser"] != null && !User.Identity.IsAuthenticated)
        {
            string username = Session["CurrentUser"].ToString();
            string role = Session["CurrentRole"]?.ToString() ?? "User";

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                role,
                FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(
                FormsAuthentication.FormsCookieName,
                encryptedTicket);

            Response.Cookies.Add(authCookie);

            // 刷新页面
            Response.Redirect(Request.Url.ToString());
        }
        // 方案2：如果User.Identity已认证，但Session中没有用户，则设置Session
        else if (User.Identity.IsAuthenticated && Session["CurrentUser"] == null)
        {
            // 从数据库获取用户角色
            string username = User.Identity.Name;
            string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Role FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                object result = cmd.ExecuteScalar();
                string role = result?.ToString() ?? "User";

                Session["CurrentUser"] = username;
                Session["CurrentRole"] = role;
            }

            // 刷新页面
            Response.Redirect(Request.Url.ToString());
        }
        else
        {
            // 其他情况，尝试重新登录
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("Account/Login.aspx");
        }
    }

    protected void btnClearSession_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect(Request.Url.ToString());
    }

    protected void btnTestAdminLogin_Click(object sender, EventArgs e)
    {
        // 测试直接设置Admin登录
        string username = "admin";
        string role = "Admin";

        // 创建认证票据
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1,
            username,
            DateTime.Now,
            DateTime.Now.AddMinutes(30),
            false,
            role,
            FormsAuthentication.FormsCookiePath);

        string encryptedTicket = FormsAuthentication.Encrypt(ticket);
        System.Web.HttpCookie authCookie = new System.Web.HttpCookie(
            FormsAuthentication.FormsCookieName,
            encryptedTicket);

        Response.Cookies.Add(authCookie);

        // 设置Session
        Session["CurrentUser"] = username;
        Session["CurrentRole"] = role;

        // 重定向到首页
        Response.Redirect("Default.aspx");
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Session.Clear();
        Session.Abandon();

        // 删除认证Cookie
        if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
        {
            HttpCookie myCookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            myCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(myCookie);
        }

        Response.Redirect("Default.aspx");
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }

    // Cookie信息类
    public class CookieInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Expires { get; set; }
    }
}