using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Text; // 添加这个命名空间引用
using Utilities;

public partial class InitPasswords : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 安全限制：只允许本地访问
        if (!Request.IsLocal)
        {
            Response.StatusCode = 403;
            Response.End();
            return;
        }
    }

    protected void btnInitPasswords_Click(object sender, EventArgs e)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

            // 要初始化的用户和密码
            var users = new[]
            {
                new { Username = "admin", Password = "admin123" },
                new { Username = "admin1", Password = "admin1123" },
                new { Username = "user1", Password = "user123" }
            };

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 修改这里：将变量名从 results 改为 sbResults（StringBuilder）
                StringBuilder sbResults = new StringBuilder();

                foreach (var user in users)
                {
                    try
                    {
                        // 检查用户是否存在
                        string checkSql = "SELECT UserID FROM Users WHERE Username = @Username";
                        SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                        checkCmd.Parameters.AddWithValue("@Username", user.Username);

                        object userId = checkCmd.ExecuteScalar();

                        if (userId != null)
                        {
                            // 生成新的盐值和哈希
                            var (salt, hash) = PasswordHasher.CreateHash(user.Password);

                            // 更新密码和盐值
                            string updateSql = @"
                                UPDATE Users 
                                SET Password = @Password, 
                                    Salt = @Salt 
                                WHERE Username = @Username";

                            SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                            updateCmd.Parameters.AddWithValue("@Password", hash);
                            updateCmd.Parameters.AddWithValue("@Salt", salt);
                            updateCmd.Parameters.AddWithValue("@Username", user.Username);

                            int rowsAffected = updateCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                sbResults.AppendLine($"✓ 用户 '{user.Username}' 密码已更新");
                                sbResults.AppendLine($"  盐值: {salt.Substring(0, 16)}...");
                                sbResults.AppendLine($"  哈希: {hash.Substring(0, 16)}...");
                                sbResults.AppendLine();
                            }
                            else
                            {
                                sbResults.AppendLine($"✗ 用户 '{user.Username}' 更新失败");
                                sbResults.AppendLine();
                            }
                        }
                        else
                        {
                            sbResults.AppendLine($"✗ 用户 '{user.Username}' 不存在");
                            sbResults.AppendLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        sbResults.AppendLine($"✗ 用户 '{user.Username}' 处理失败: {ex.Message}");
                        sbResults.AppendLine();
                    }
                }

                // 修改这里：使用 sbResults 而不是 results
                litResults.Text = sbResults.ToString().Replace("\n", "<br/>");

                // results 是服务器控件，应该能正常访问
                if (results != null)
                {
                    results.Visible = true;
                }

                lblMessage.Text = "密码初始化完成！";
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = $"初始化失败: {ex.Message}";
            lblMessage.CssClass = "alert alert-danger";
            lblMessage.Visible = true;
        }
    }
}