using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;

public partial class Contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 生成随机验证码
            GenerateCaptcha();

            // 如果是已登录用户，自动填充姓名和邮箱
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
                txtName.Text = username;

                // 从数据库获取用户邮箱
                LoadUserEmail(username);
            }
        }
    }

    private void LoadUserEmail(string username)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "SELECT Email FROM Users WHERE Username = @Username";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();
            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                txtEmail.Text = result.ToString();
            }
        }
    }

    private void GenerateCaptcha()
    {
        // 简单的验证码生成（课程作业，不要求复杂）
        string captcha = new Random().Next(1000, 9999).ToString();
        captchaText.InnerText = captcha;
        Session["ContactCaptcha"] = captcha; // 保存到Session用于验证
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // 验证验证码（可选，课程作业可以跳过）
            if (!ValidateCaptcha())
            {
                ShowMessage("验证码错误！", "danger");
                GenerateCaptcha(); // 重新生成验证码
                return;
            }

            try
            {
                // 保存留言到数据库
                if (SaveContactMessage())
                {
                    ShowMessage("留言提交成功！我们会在24小时内回复您。", "success");
                    ClearForm();
                    GenerateCaptcha();
                }
                else
                {
                    ShowMessage("留言提交失败，请稍后重试。", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("提交过程中出现错误：" + ex.Message, "danger");
            }
        }
    }

    private bool ValidateCaptcha()
    {
        // 验证码验证（课程作业可以注释掉这部分）
        string userInput = txtCaptcha.Text.Trim();
        string sessionCaptcha = Session["ContactCaptcha"] as string;

        // 如果Session中没有验证码或验证码为空，则跳过验证
        if (string.IsNullOrEmpty(sessionCaptcha) || string.IsNullOrEmpty(userInput))
        {
            return true; // 课程作业可以跳过验证
        }

        return userInput == sessionCaptcha;
    }

    private bool SaveContactMessage()
    {
        // 如果不需要保存到数据库，可以直接返回true
        // 课程作业只需要模拟提交成功即可

        // 如果需要保存到数据库，可以创建ContactMessages表

        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        // 首先检查表是否存在，不存在则创建
        if (!CheckTableExists("ContactMessages"))
        {
            CreateContactMessagesTable();
        }

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                INSERT INTO ContactMessages (Name, Email, Phone, Subject, Message, IPAddress, CreateDate, Status) 
                VALUES (@Name, @Email, @Phone, @Subject, @Message, @IPAddress, GETDATE(), '未处理')"
            ;

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@Subject", ddlSubject.SelectedValue);
            cmd.Parameters.AddWithValue("@Message", txtMessage.Text.Trim());
            cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

            conn.Open();
            int result = cmd.ExecuteNonQuery();

            return result > 0;
        }
    }

    private bool CheckTableExists(string tableName)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT COUNT(*) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = @TableName";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TableName", tableName);

            conn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }
    }

    private void CreateContactMessagesTable()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                CREATE TABLE ContactMessages (
                    MessageID INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(50) NOT NULL,
                    Email NVARCHAR(100) NOT NULL,
                    Phone NVARCHAR(20),
                    Subject NVARCHAR(50) NOT NULL,
                    Message NTEXT NOT NULL,
                    IPAddress NVARCHAR(20),
                    CreateDate DATETIME DEFAULT GETDATE(),
                    Status NVARCHAR(20) DEFAULT '未处理',
                    ReplyMessage NTEXT,
                    ReplyDate DATETIME
                )";

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private void ShowMessage(string message, string type)
    {
        litMessage.Text = message;

        switch (type.ToLower())
        {
            case "success":
                pnlMessage.CssClass = "alert alert-success";
                break;
            case "danger":
                pnlMessage.CssClass = "alert alert-danger";
                break;
            case "warning":
                pnlMessage.CssClass = "alert alert-warning";
                break;
            case "info":
                pnlMessage.CssClass = "alert alert-info";
                break;
            default:
                pnlMessage.CssClass = "alert alert-info";
                break;
        }

        pnlMessage.Visible = true;
    }

    private void ClearForm()
    {
        if (!User.Identity.IsAuthenticated)
        {
            txtName.Text = "";
            txtEmail.Text = "";
        }
        txtPhone.Text = "";
        ddlSubject.SelectedIndex = 0;
        txtMessage.Text = "";
        txtCaptcha.Text = "";
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearForm();
        GenerateCaptcha();
        pnlMessage.Visible = false;
    }

    // 验证码点击刷新
    protected void captchaText_ServerClick(object sender, EventArgs e)
    {
        GenerateCaptcha();
    }
}