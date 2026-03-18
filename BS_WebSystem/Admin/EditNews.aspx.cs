using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class Admin_EditNews : System.Web.UI.Page
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

            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int newsId))
            {
                hdnNewsID.Value = newsId.ToString();
                LoadNewsDetails(newsId);
            }
            else
            {
                Response.Redirect("NewsManage.aspx");
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

    private void LoadNewsDetails(int newsId)
    {
        string currentUser = User.Identity.Name;
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            try
            {
                // 只检查是否是当前用户发布的新闻
                string sql = @"
                    SELECT n.*, u.Username 
                    FROM News n 
                    INNER JOIN Users u ON n.AuthorID = u.UserID 
                    WHERE n.NewsID = @NewsID AND u.Username = @Username";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@NewsID", newsId);
                cmd.Parameters.AddWithValue("@Username", currentUser);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // 填充表单数据
                    txtTitle.Text = reader["Title"].ToString();
                    txtContent.Text = reader["Content"].ToString();

                    // 设置分类
                    string category = reader["Category"].ToString();
                    if (ddlCategory.Items.FindByValue(category) != null)
                    {
                        ddlCategory.SelectedValue = category;
                    }

                    // 显示当前图片
                    if (!string.IsNullOrEmpty(reader["ImageUrl"].ToString()))
                    {
                        imgCurrent.ImageUrl = reader["ImageUrl"].ToString();
                        imgCurrent.Visible = true;
                    }
                    else
                    {
                        lblNoImage.Visible = true;
                    }

                    // 设置状态
                    bool isActive = Convert.ToBoolean(reader["IsActive"]);
                    rblStatus.SelectedValue = isActive ? "1" : "0";

                    // 显示其他信息
                    if (reader["UpdateDate"] != DBNull.Value)
                    {
                        lblUpdateInfo.Text = Convert.ToDateTime(reader["UpdateDate"]).ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        lblUpdateInfo.Text = "从未修改";
                    }

                    lblViewCount.Text = reader["Views"].ToString();
                }
                else
                {
                    // 新闻不存在或不是当前用户的新闻
                    lblMessage.Text = "新闻不存在或您没有权限编辑！";
                    lblMessage.CssClass = "alert alert-danger";
                    lblMessage.Visible = true;

                    // 禁用表单
                    DisableForm();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "加载新闻详情失败：" + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }
    }

    private void DisableForm()
    {
        txtTitle.Enabled = false;
        txtContent.Enabled = false;
        ddlCategory.Enabled = false;
        fuImage.Enabled = false;
        cbRemoveImage.Enabled = false;
        rblStatus.Enabled = false;
        btnSave.Enabled = false;
        btnDelete.Enabled = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid && int.TryParse(hdnNewsID.Value, out int newsId))
        {
            try
            {
                string currentUser = User.Identity.Name;
                string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // 首先验证是否是当前用户的新闻
                    string checkSql = @"
                        SELECT COUNT(*) 
                        FROM News 
                        WHERE NewsID = @NewsID AND AuthorID = 
                            (SELECT UserID FROM Users WHERE Username = @Username)";

                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@NewsID", newsId);
                    checkCmd.Parameters.AddWithValue("@Username", currentUser);

                    conn.Open();
                    int canEdit = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (canEdit == 0)
                    {
                        lblMessage.Text = "您没有权限编辑此新闻！";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    string imageUrl = null;
                    bool removeImage = cbRemoveImage.Checked;

                    if (removeImage)
                    {
                        imageUrl = "~/Images/default.jpg";
                    }
                    else if (fuImage.HasFile)
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
                        string getImageSql = "SELECT ImageUrl FROM News WHERE NewsID = @NewsID";
                        SqlCommand getImageCmd = new SqlCommand(getImageSql, conn);
                        getImageCmd.Parameters.AddWithValue("@NewsID", newsId);
                        object currentImage = getImageCmd.ExecuteScalar();
                        imageUrl = currentImage != DBNull.Value ? currentImage.ToString() : "~/Images/default.jpg";
                    }

                    string updateSql = @"
                        UPDATE News 
                        SET Title = @Title,
                            Content = @Content,
                            Category = @Category,
                            ImageUrl = @ImageUrl,
                            UpdateDate = GETDATE(),
                            IsActive = @IsActive
                        WHERE NewsID = @NewsID";

                    SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    updateCmd.Parameters.AddWithValue("@Content", txtContent.Text);
                    updateCmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                    updateCmd.Parameters.AddWithValue("@IsActive", Convert.ToBoolean(int.Parse(rblStatus.SelectedValue)));
                    updateCmd.Parameters.AddWithValue("@NewsID", newsId);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMessage.Text = "新闻修改成功！";
                        lblMessage.CssClass = "alert alert-success";
                        lblMessage.Visible = true;
                        LoadNewsDetails(newsId);
                    }
                    else
                    {
                        lblMessage.Text = "修改失败！";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "保存失败：" + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewsManage.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (int.TryParse(hdnNewsID.Value, out int newsId))
        {
            try
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

                    if (result > 0)
                    {
                        Response.Redirect("NewsManage.aspx?message=删除成功");
                    }
                    else
                    {
                        lblMessage.Text = "删除失败，可能是无权限或新闻不存在！";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "删除失败：" + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }
    }
}