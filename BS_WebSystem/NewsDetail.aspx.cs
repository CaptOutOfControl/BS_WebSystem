using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class NewsDetail : System.Web.UI.Page
{
    protected global::System.Web.UI.WebControls.PlaceHolder phRelatedNewsEmpty;
    protected global::System.Web.UI.WebControls.PlaceHolder phHotNewsEmpty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int newsId))
            {
                LoadNewsDetail(newsId);
                UpdateViewCount(newsId);
                BindRelatedNews();
                BindHotNewsSide();
                BindPrevNextNews(newsId);

                // 初始化 PlaceHolder 为隐藏
                phRelatedNewsEmpty.Visible = false;
                phHotNewsEmpty.Visible = false;
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    private void LoadNewsDetail(int newsId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT n.*, u.Username, u.Email 
                FROM News n 
                INNER JOIN Users u ON n.AuthorID = u.UserID 
                WHERE n.NewsID = @NewsID AND n.IsActive = 1";
            
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@NewsID", newsId);
            
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                Page.Title = reader["Title"].ToString() + " - 新闻发布系统";
                
                newsTitle.InnerText = reader["Title"].ToString();
                newsContent.InnerHtml = reader["Content"].ToString();
                authorInfo.InnerHtml = "<i class='fas fa-user me-1'></i>" + reader["Username"].ToString();
                publishDate.InnerHtml = "<i class='far fa-clock me-1'></i>" + 
                    Convert.ToDateTime(reader["PublishDate"]).ToString("yyyy年MM月dd日 HH:mm");
                categoryInfo.InnerHtml = "<i class='fas fa-tag me-1'></i>" + reader["Category"].ToString();
                viewCount.InnerHtml = "<i class='far fa-eye me-1'></i>" + reader["Views"].ToString() + " 次阅读";
                
                if (!string.IsNullOrEmpty(reader["ImageUrl"].ToString()))
                {
                    imgNews.ImageUrl = reader["ImageUrl"].ToString();
                    imgNews.Visible = true;
                    imgNews.AlternateText = reader["Title"].ToString();
                }
                
                ViewState["CurrentCategory"] = reader["Category"].ToString();
                ViewState["CurrentNewsId"] = newsId;
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
            reader.Close();
        }
    }
    
    private void UpdateViewCount(int newsId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "UPDATE News SET Views = Views + 1 WHERE NewsID = @NewsID";
            
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@NewsID", newsId);
            
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private void BindRelatedNews()
    {
        string category = ViewState["CurrentCategory"] as string;
        int currentNewsId = Convert.ToInt32(ViewState["CurrentNewsId"]);

        if (!string.IsNullOrEmpty(category))
        {
            string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                SELECT TOP 5 NewsID, Title, PublishDate 
                FROM News 
                WHERE Category = @Category 
                AND NewsID != @CurrentNewsID 
                AND IsActive = 1 
                ORDER BY PublishDate DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@CurrentNewsID", currentNewsId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptRelatedNews.DataSource = dt;
                    rptRelatedNews.DataBind();
                    phRelatedNewsEmpty.Visible = false;
                }
                else
                {
                    rptRelatedNews.DataSource = null;
                    rptRelatedNews.DataBind();
                    phRelatedNewsEmpty.Visible = true;
                }
            }
        }
        else
        {
            phRelatedNewsEmpty.Visible = true;
        }
    }

    private void BindHotNewsSide()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
            SELECT TOP 5 NewsID, Title, PublishDate, Views 
            FROM News 
            WHERE IsActive = 1 
            AND NewsID != @CurrentNewsID
            ORDER BY Views DESC, PublishDate DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CurrentNewsID", ViewState["CurrentNewsId"]);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                rptHotNewsSide.DataSource = dt;
                rptHotNewsSide.DataBind();
                phHotNewsEmpty.Visible = false;
            }
            else
            {
                rptHotNewsSide.DataSource = null;
                rptHotNewsSide.DataBind();
                phHotNewsEmpty.Visible = true;
            }
        }
    }

    private void BindPrevNextNews(int currentNewsId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;
        
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sqlPrev = @"
                SELECT TOP 1 NewsID, Title 
                FROM News 
                WHERE NewsID < @CurrentNewsID 
                AND IsActive = 1 
                ORDER BY NewsID DESC";
            
            SqlCommand cmdPrev = new SqlCommand(sqlPrev, conn);
            cmdPrev.Parameters.AddWithValue("@CurrentNewsID", currentNewsId);
            
            string sqlNext = @"
                SELECT TOP 1 NewsID, Title 
                FROM News 
                WHERE NewsID > @CurrentNewsID 
                AND IsActive = 1 
                ORDER BY NewsID ASC";
            
            SqlCommand cmdNext = new SqlCommand(sqlNext, conn);
            cmdNext.Parameters.AddWithValue("@CurrentNewsID", currentNewsId);
            
            conn.Open();
            
            SqlDataReader readerPrev = cmdPrev.ExecuteReader();
            if (readerPrev.Read())
            {
                lnkPrev.Text = readerPrev["Title"].ToString();
                lnkPrev.NavigateUrl = "NewsDetail.aspx?id=" + readerPrev["NewsID"].ToString();
            }
            else
            {
                lnkPrev.Text = "没有了";
                lnkPrev.NavigateUrl = "#";
                lnkPrev.Attributes["class"] = "text-muted";
            }
            readerPrev.Close();
            
            SqlDataReader readerNext = cmdNext.ExecuteReader();
            if (readerNext.Read())
            {
                lnkNext.Text = readerNext["Title"].ToString();
                lnkNext.NavigateUrl = "NewsDetail.aspx?id=" + readerNext["NewsID"].ToString();
            }
            else
            {
                lnkNext.Text = "没有了";
                lnkNext.NavigateUrl = "#";
                lnkNext.Attributes["class"] = "text-muted";
            }
            readerNext.Close();
        }
    }
}