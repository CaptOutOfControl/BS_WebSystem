using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web;
using Utilities;

public partial class Account_Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 如果用户已登录，重定向到首页
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }

    protected void CreateUser_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            string username = UserName.Text.Trim();
            string password = Password.Text;
            string email = Email.Text.Trim();

            // 检查密码强度
            if (!PasswordHasher.IsPasswordStrong(password))
            {
                FailureText.Text = "密码强度不足！密码至少6位，且必须包含字母和数字";
                ErrorMessage.Visible = true;
                return;
            }

            // 检查用户名是否已存在
            if (CheckUserExists(username))
            {
                FailureText.Text = "用户名已存在，请选择其他用户名！";
                ErrorMessage.Visible = true;
                return;
            }

            // 注册新用户
            if (RegisterUser(username, password, email))
            {
                // 注册成功后创建身份验证票据
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    "", // 用户数据
                    FormsAuthentication.FormsCookiePath);

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                // 重定向到首页
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                FailureText.Text = "注册失败，请稍后重试！";
                ErrorMessage.Visible = true;
            }
        }
    }

    private bool CheckUserExists(string username)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }
    }

    private bool RegisterUser(string username, string password, string email)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        // 生成盐值和密码哈希
        var (salt, hash) = PasswordHasher.CreateHash(password);

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                INSERT INTO Users (Username, Password, Salt, Email, Role, CreatedDate) 
                VALUES (@Username, @Password, @Salt, @Email, 'User', GETDATE())";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", hash);
            cmd.Parameters.AddWithValue("@Salt", salt);
            cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? DBNull.Value : (object)email);

            try
            {
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"用户注册成功: {username}, 盐值: {salt.Substring(0, 16)}..., 哈希: {hash.Substring(0, 16)}...");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // 记录错误信息
                System.Diagnostics.Debug.WriteLine("注册用户失败: " + ex.Message);
                return false;
            }
        }
    }
}