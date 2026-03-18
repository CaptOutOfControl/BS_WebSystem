using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

public partial class NewsList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string category = Request.QueryString["category"];

            if (!string.IsNullOrEmpty(category))
            {
                lblCategory.Text = category + " 新闻";
                BindNewsByCategory(category);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    private void BindNewsByCategory(string category)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT n.NewsID, n.Title, n.Content, n.PublishDate, n.Views, n.Category, u.Username 
                FROM News n 
                INNER JOIN Users u ON n.AuthorID = u.UserID 
                WHERE n.Category = @Category AND n.IsActive = 1 
                ORDER BY n.PublishDate DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Category", category);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            lvCategoryNews.DataSource = dt;
            lvCategoryNews.DataBind();
        }
    }

    public string TruncateContent(string content, int length)
    {
        if (string.IsNullOrEmpty(content))
            return "";

        string plainText = Regex.Replace(content, "<.*?>", string.Empty);

        if (plainText.Length > length)
        {
            return plainText.Substring(0, length) + "...";
        }
        return plainText;
    }
}