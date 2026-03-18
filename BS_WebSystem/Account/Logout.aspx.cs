using System;
using System.Web;

public partial class Account_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 清除所有Session
        Session.Clear();
        Session.Abandon();

        // 清除身份验证Cookie
        System.Web.Security.FormsAuthentication.SignOut();

        // 清除所有相关Cookie
        string[] cookiesToClear = {
            System.Web.Security.FormsAuthentication.FormsCookieName,
            "ASP.NET_SessionId",
            "__RequestVerificationToken"
        };

        foreach (string cookieName in cookiesToClear)
        {
            if (Request.Cookies[cookieName] != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
        }

        // 添加无缓存头
        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        Response.Cache.SetNoStore();

        // 重定向到首页，添加随机参数防止缓存
        string redirectUrl = "~/Default.aspx?logout=true&t=" + DateTime.Now.Ticks;
        Response.Redirect(redirectUrl);
    }
}