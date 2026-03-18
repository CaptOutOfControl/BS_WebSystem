using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace BS_WebSystem
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        void Application_End(object sender, EventArgs e)
        {
            // 在应用程序关闭时运行的代码
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            Session["SessionStartTime"] = DateTime.Now;
        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // 在每个请求进行身份验证时运行

            string cookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (authCookie != null)
            {
                try
                {
                    // 解密认证票据
                    System.Web.Security.FormsAuthenticationTicket authTicket =
                        System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);

                    if (authTicket != null && !authTicket.Expired)
                    {
                        // 从票据中获取用户数据和角色
                        string[] roles = authTicket.UserData.Split(',');

                        // 创建身份标识
                        System.Security.Principal.GenericIdentity identity =
                            new System.Security.Principal.GenericIdentity(authTicket.Name, "Forms");

                        // 创建主体
                        System.Security.Principal.GenericPrincipal principal =
                            new System.Security.Principal.GenericPrincipal(identity, roles);

                        // 设置当前用户
                        Context.User = principal;

                        // 设置线程主体
                        System.Threading.Thread.CurrentPrincipal = principal;

                        // 同步Session
                        if (Context.Session != null)
                        {
                            Context.Session["CurrentUser"] = authTicket.Name;
                            Context.Session["CurrentRole"] = roles.Length > 0 ? roles[0] : "User";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 票据解密失败，清除无效的Cookie
                    System.Diagnostics.Debug.WriteLine("认证票据解密失败: " + ex.Message);

                    // 清除无效Cookie
                    if (Context.Request.Cookies[cookieName] != null)
                    {
                        Context.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
                    }
                }
            }
        }
    }
}