using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Admin_NewsManage : System.Web.UI.Page
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

            if (Request.QueryString["message"] != null)
            {
                string message = Request.QueryString["message"];
                lblMessage.Text = message + "！";
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;
            }

            BindNewsList();
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

    private void BindNewsList()
    {
        string currentUser = User.Identity.Name;
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT n.NewsID, n.Title, n.Category, n.PublishDate, 
                       n.UpdateDate, n.Views, n.IsActive,
                       CASE 
                           WHEN n.UpdateDate IS NOT NULL THEN n.UpdateDate
                           ELSE n.PublishDate
                       END as LastModified
                FROM News n 
                INNER JOIN Users u ON n.AuthorID = u.UserID 
                WHERE u.Username = @Username 
                ORDER BY LastModified DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", currentUser);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gvNews.DataSource = dt;
            gvNews.DataBind();
        }
    }

    protected void gvNews_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteNews")
        {
            int newsId = Convert.ToInt32(e.CommandArgument);

            try
            {
                DeleteNews(newsId);
                BindNewsList();

                lblMessage.Text = "删除成功！";
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "删除失败：" + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }
    }

    private void DeleteNews(int newsId)
    {
        string currentUser = User.Identity.Name;
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                DELETE FROM News 
                WHERE NewsID = @NewsID 
                AND AuthorID = (SELECT UserID FROM Users WHERE Username = @Username)";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@NewsID", newsId);
            cmd.Parameters.AddWithValue("@Username", currentUser);

            conn.Open();
            int result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                throw new Exception("无权限或新闻不存在");
            }
        }
    }

    protected void gvNews_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            System.Web.UI.WebControls.LinkButton btnDelete = (System.Web.UI.WebControls.LinkButton)e.Row.FindControl("btnDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes["onclick"] = "return confirm('确定要删除这条新闻吗？此操作不可恢复！');";
            }
        }
    }

    protected void btnAddNews_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddNews.aspx");
    }

    protected void gvNews_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvNews.PageIndex = e.NewPageIndex;
        BindNewsList();
    }

    public string TruncateTitle(string title, int length)
    {
        if (string.IsNullOrEmpty(title))
            return "";

        if (title.Length > length)
        {
            return title.Substring(0, length) + "...";
        }
        return title;
    }
}