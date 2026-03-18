using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class SearchResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["keyword"] != null)
            {
                string keyword = Request.QueryString["keyword"];
                txtSearch.Text = keyword;
                lblSearchTerm.Text = "搜索结果: " + Server.HtmlEncode(keyword);
                BindSearchResults(keyword);
            }
            else if (Request.QueryString["category"] != null)
            {
                string category = Request.QueryString["category"];
                lblSearchTerm.Text = "分类: " + Server.HtmlEncode(category);
                BindSearchByCategory(category);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    private void BindSearchResults(string keyword)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT NewsID, Title, Content, PublishDate, Category, Views, ImageUrl 
                FROM News 
                WHERE (Title LIKE @Keyword OR Content LIKE @Keyword) 
                AND IsActive = 1 
                ORDER BY PublishDate DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                lvSearchResults.DataSource = dt;
                lvSearchResults.DataBind();
                lblResultCount.Text = "找到 " + dt.Rows.Count + " 条结果";
                pnlNoResults.Visible = false;
            }
            else
            {
                lblResultCount.Text = "没有找到相关新闻";
                pnlNoResults.Visible = true;
            }
        }
    }

    private void BindSearchByCategory(string category)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT NewsID, Title, Content, PublishDate, Category, Views, ImageUrl 
                FROM News 
                WHERE Category = @Category 
                AND IsActive = 1 
                ORDER BY PublishDate DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Category", category);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            lvSearchResults.DataSource = dt;
            lvSearchResults.DataBind();
            lblResultCount.Text = "分类 " + category + " 下有 " + dt.Rows.Count + " 条新闻";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string keyword = txtSearch.Text.Trim();
        if (!string.IsNullOrEmpty(keyword))
        {
            Response.Redirect($"SearchResult.aspx?keyword={Server.UrlEncode(keyword)}");
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

    public string GetShortTime(object publishDate)
    {
        if (publishDate == null) return "";

        DateTime date = Convert.ToDateTime(publishDate);
        TimeSpan span = DateTime.Now - date;

        if (span.TotalDays > 365)
            return (span.TotalDays / 365).ToString("0") + "年前";
        else if (span.TotalDays > 30)
            return (span.TotalDays / 30).ToString("0") + "个月前";
        else if (span.TotalDays > 1)
            return span.TotalDays.ToString("0") + "天前";
        else if (span.TotalHours > 1)
            return span.TotalHours.ToString("0") + "小时前";
        else if (span.TotalMinutes > 1)
            return span.TotalMinutes.ToString("0") + "分钟前";
        else
            return "刚刚";
    }
}