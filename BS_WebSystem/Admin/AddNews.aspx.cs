using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.EnterpriseServices.Internal;

public partial class Admin_AddNews : System.Web.UI.Page
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                string imageUrl = "";

                // 处理图片上传
                if (fuImage.HasFile)
                {
                    if (fuImage.PostedFile.ContentLength > 40 * 1024 * 1024)
                    {
                        lblMessage.Text = "图片大小不能超过40MB";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" +
                                    Path.GetFileName(fuImage.FileName);
                    string filePath = Server.MapPath("~/Images/") + fileName;
                    fuImage.SaveAs(filePath);
                    imageUrl = "~/Images/" + fileName;

                }
                else
                {
                    imageUrl = "~/Images/default.jpg"; // 默认图片
                }

                string currentUser = User.Identity.Name;
                string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // 获取用户ID
                    string getUserIdSql = "SELECT UserID FROM Users WHERE Username = @Username";
                    SqlCommand getUserIdCmd = new SqlCommand(getUserIdSql, conn);
                    getUserIdCmd.Parameters.AddWithValue("@Username", currentUser);

                    conn.Open();
                    object userIdObj = getUserIdCmd.ExecuteScalar();

                    if (userIdObj == null)
                    {
                        lblMessage.Text = "用户不存在";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    int userId = Convert.ToInt32(userIdObj);

                    // 确定是否激活
                    bool isActive = rbPublish.Checked;

                    // 插入新闻
                    string sql = @"
                        INSERT INTO News (Title, Content, Category, AuthorID, ImageUrl, PublishDate, IsActive) 
                        VALUES (@Title, @Content, @Category, @AuthorID, @ImageUrl, GETDATE(), @IsActive)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Content", txtContent.Text);
                    cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@AuthorID", userId);
                    cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMessage.Text = "新闻添加成功！";
                        lblMessage.CssClass = "alert alert-success";
                        lblMessage.Visible = true;

                        // 清空表单
                        txtTitle.Text = "";
                        txtContent.Text = "";
                        ddlCategory.SelectedIndex = 0;
                        rbPublish.Checked = true;
                    }
                    else
                    {
                        lblMessage.Text = "添加失败！";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "添加失败：" + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewsManage.aspx");
    }
}