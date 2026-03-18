using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Providers.Entities;
using System.Web;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UpdateLoginStatus();
            UpdateAdminLink();
            SetActiveNavigation(); // 设置导航栏激活状态
            // 注册客户端脚本
            RegisterClientScript();
        }

        // 检查是否刚刚登录
        if (Session["JustLoggedIn"] != null && (bool)Session["JustLoggedIn"])
        {
            // 如果Session中有用户，但User.Identity未认证，则跳转到修复页面
            if (Session["CurrentUser"] != null && !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Session.Remove("JustLoggedIn"); // 清除标记，避免重复跳转
                Response.Redirect("~/FixAuth.aspx?action=check");
            }
            else
            {
                // 认证正常，清除标记
                Session.Remove("JustLoggedIn");
            }
        }
    }

    private void UpdateLoginStatus()
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            pnlNotLoggedIn.Visible = false;
            pnlLoggedIn.Visible = true;
            loggedInUser.InnerText = "欢迎，" + HttpContext.Current.User.Identity.Name + "！";

            System.Diagnostics.Debug.WriteLine($"母版页: 用户已登录 - {HttpContext.Current.User.Identity.Name}");
        }
        else
        {
            pnlNotLoggedIn.Visible = true;
            pnlLoggedIn.Visible = false;

            // 检查Session（用于调试）
            if (Session["CurrentUser"] != null)
            {
                System.Diagnostics.Debug.WriteLine($"母版页: Session中有用户 - {Session["CurrentUser"]}，但User.Identity未认证");
            }
        }
    }

    private void UpdateAdminLink()
    {
        // 重要：只有Admin角色才显示后台管理链接
        bool isAdmin = IsAdminUser();
        hlAdmin.Visible = isAdmin;

        System.Diagnostics.Debug.WriteLine($"母版页: 用户 {HttpContext.Current.User.Identity.Name} 是管理员: {isAdmin}");
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        System.Web.Security.FormsAuthentication.SignOut();
        Session.Clear();
        Response.Redirect("~/Default.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string keyword = txtSearch.Text.Trim();
        if (!string.IsNullOrEmpty(keyword))
        {
            Response.Redirect($"SearchResult.aspx?keyword={Server.UrlEncode(keyword)}");
        }
    }

    // 检查用户是否管理员
    public bool IsAdminUser()
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated) return false;

        string username = HttpContext.Current.User.Identity.Name;
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Role = 'Admin'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();
            int result = Convert.ToInt32(cmd.ExecuteScalar());

            return result > 0;
        }
    }

    // 注册客户端脚本
    private void RegisterClientScript()
    {
        // 添加平滑滚动脚本
        string script = @"
            <script>
                // 平滑滚动
                document.querySelectorAll('a[href^=""#""]').forEach(anchor => {
                    anchor.addEventListener('click', function (e) {
                        e.preventDefault();
                        
                        const targetId = this.getAttribute('href');
                        if(targetId === '#') return;
                        
                        const targetElement = document.querySelector(targetId);
                        if(targetElement) {
                            window.scrollTo({
                                top: targetElement.offsetTop - 80,
                                behavior: 'smooth'
                            });
                        }
                    });
                });
                
                // 表单提交动画
                document.querySelectorAll('form').forEach(form => {
                    form.addEventListener('submit', function() {
                        const submitBtn = this.querySelector('button[type=""submit""], input[type=""submit""]');
                        if(submitBtn) {
                            submitBtn.innerHTML = '<i class=""fas fa-spinner fa-spin me-2""></i>处理中...';
                            submitBtn.disabled = true;
                        }
                    });
                });
            </script>
        ";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SmoothScroll", script);
    }

    /// <summary>
    /// 设置导航栏激活状态
    /// </summary>
    private void SetActiveNavigation()
    {
        // 先移除所有链接的active类
        RemoveAllActiveClasses();

        // 获取当前页面路径和查询参数
        string currentPage = Request.Url.AbsolutePath.ToLower();
        string category = Request.QueryString["category"];


        // 使用更简单的逻辑
        if (currentPage.EndsWith("default.aspx") || currentPage.EndsWith("/") || currentPage.EndsWith("default"))
        {
            // 首页
            hlHome.CssClass = "nav-link active";
            System.Diagnostics.Debug.WriteLine("设置首页为激活状态");
        }
        else if (currentPage.Contains("newslist") || currentPage.Contains("searchresult"))
        {
            // 新闻列表或搜索结果页面
            if (!string.IsNullOrEmpty(category))
            {
                switch (category)
                {
                    case "政治":
                        hlPolitics.CssClass = "nav-link animate-on-load delay-2 active";
                        break;
                    case "经济":
                        hlEconomics.CssClass = "nav-link animate-on-load delay-3 active";
                        break;
                    case "科技":
                        hlTechnology.CssClass = "nav-link animate-on-load delay-4 active";
                        break;
                    case "体育":
                        hlSports.CssClass = "nav-link animate-on-load delay-5 active";
                        break;
                    case "娱乐":
                        hlEntertainment.CssClass = "nav-link animate-on-load active";
                        break;
                }
            }
            // 如果没有分类参数，保持未激活状态
        }
        else if (currentPage.EndsWith("about.aspx") || currentPage.Contains("about"))
        {
            hlAbout.CssClass = "nav-link active";
        }
        else if (currentPage.EndsWith("contact.aspx") || currentPage.Contains("contact"))
        {
            hlContact.CssClass = "nav-link active";
        }
        else if (currentPage.Contains("admin"))
        {
            hlAdmin.CssClass = "nav-link text-danger animate-on-load active";
        }
    }

    /// <summary>
    /// 移除所有导航链接的active类
    /// </summary>
    private void RemoveAllActiveClasses()
    {
        hlHome.CssClass = hlHome.CssClass.Replace(" active", "").Trim();
        hlPolitics.CssClass = hlPolitics.CssClass.Replace(" active", "").Trim();
        hlEconomics.CssClass = hlEconomics.CssClass.Replace(" active", "").Trim();
        hlTechnology.CssClass = hlTechnology.CssClass.Replace(" active", "").Trim();
        hlSports.CssClass = hlSports.CssClass.Replace(" active", "").Trim();
        hlEntertainment.CssClass = hlEntertainment.CssClass.Replace(" active", "").Trim();
        hlAbout.CssClass = hlAbout.CssClass.Replace(" active", "").Trim();
        hlContact.CssClass = hlContact.CssClass.Replace(" active", "").Trim();
        hlAdmin.CssClass = hlAdmin.CssClass.Replace(" active", "").Trim();
    }


}