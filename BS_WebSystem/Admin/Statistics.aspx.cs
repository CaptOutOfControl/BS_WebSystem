using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class Admin_Statistics : System.Web.UI.Page
{
    private DateTime startDate;
    private DateTime endDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 权限检查
            if (!User.Identity.IsAuthenticated || !IsAdminUser())
            {
                Response.Redirect("~/Account/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
                return;
            }

            // 设置默认时间段（本周）
            SetDateRange("week");

            // 加载统计数据
            LoadStatistics();

            // 显示更新时间
            lblUpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    private bool IsAdminUser()
    {
        if (!User.Identity.IsAuthenticated) return false;

        string username = User.Identity.Name;
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

    private void SetDateRange(string rangeType)
    {
        DateTime now = DateTime.Now;

        switch (rangeType)
        {
            case "today":
                startDate = now.Date;
                endDate = now.Date.AddDays(1).AddSeconds(-1);
                break;
            case "yesterday":
                startDate = now.Date.AddDays(-1);
                endDate = now.Date.AddSeconds(-1);
                break;
            case "week":
                startDate = now.Date.AddDays(-(int)now.DayOfWeek + 1); // 本周一
                endDate = startDate.AddDays(7).AddSeconds(-1);
                break;
            case "month":
                startDate = new DateTime(now.Year, now.Month, 1);
                endDate = startDate.AddMonths(1).AddSeconds(-1);
                break;
            case "year":
                startDate = new DateTime(now.Year, 1, 1);
                endDate = startDate.AddYears(1).AddSeconds(-1);
                break;
            case "all":
                startDate = new DateTime(2000, 1, 1); // 很早的日期
                endDate = now;
                break;
            default:
                startDate = now.Date.AddDays(-7);
                endDate = now;
                break;
        }

        // 更新日期选择器的显示
        txtStartDate.Text = startDate.ToString("yyyy-MM-dd");
        txtEndDate.Text = endDate.ToString("yyyy-MM-dd");
    }

    private void LoadStatistics()
    {
        try
        {
            // 加载基本统计数据
            LoadBasicStats();

            // 加载分类统计
            LoadCategoryStats();

            // 加载作者统计
            LoadAuthorStats();

            // 加载热门新闻
            LoadHotNews();

            // 加载每日趋势
            LoadDailyStats();
        }
        catch (Exception ex)
        {
            // 记录错误但不中断页面显示
            System.Diagnostics.Debug.WriteLine("加载统计时出错: " + ex.Message);
        }
    }

    private void LoadBasicStats()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            // 新闻总数和今日新增
            string sqlNews = @"
                -- 新闻总数
                SELECT COUNT(*) as TotalNews FROM News WHERE IsActive = 1;
                
                -- 今日新增新闻
                SELECT COUNT(*) as TodayNews FROM News 
                WHERE IsActive = 1 AND PublishDate >= @TodayStart AND PublishDate <= @TodayEnd;
                
                -- 用户总数
                SELECT COUNT(*) as TotalUsers FROM Users;
                
                -- 管理员数
                SELECT COUNT(*) as AdminCount FROM Users WHERE Role = 'Admin';
                
                -- 总浏览数和平均浏览
                SELECT 
                    ISNULL(SUM(Views), 0) as TotalViews,
                    ISNULL(AVG(CASE WHEN Views > 0 THEN CAST(Views AS FLOAT) ELSE NULL END), 0) as AvgViews
                FROM News WHERE IsActive = 1;
                
                -- 活跃度（有新闻的天数/总天数）
                DECLARE @TotalDays INT = DATEDIFF(DAY, @StartDate, @EndDate) + 1;
                DECLARE @ActiveDays INT = (
                    SELECT COUNT(DISTINCT CONVERT(DATE, PublishDate)) 
                    FROM News 
                    WHERE IsActive = 1 AND PublishDate >= @StartDate AND PublishDate <= @EndDate
                );
                SELECT 
                    CASE 
                        WHEN @TotalDays > 0 THEN CAST(@ActiveDays AS FLOAT) / @TotalDays * 100 
                        ELSE 0 
                    END as ActivityRate;
                
                -- 更新频率（天/篇）
                DECLARE @NewsCount INT = (SELECT COUNT(*) FROM News WHERE IsActive = 1 AND PublishDate >= @StartDate AND PublishDate <= @EndDate);
                SELECT 
                    CASE 
                        WHEN @NewsCount > 0 THEN CAST(@TotalDays AS FLOAT) / @NewsCount 
                        ELSE 0 
                    END as UpdateFreq;";

            SqlCommand cmd = new SqlCommand(sqlNews, conn);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@TodayStart", DateTime.Today);
            cmd.Parameters.AddWithValue("@TodayEnd", DateTime.Today.AddDays(1).AddSeconds(-1));

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            // 读取新闻总数
            if (reader.Read())
            {
                lblTotalNews.Text = reader["TotalNews"].ToString();
            }

            // 读取今日新增
            if (reader.NextResult() && reader.Read())
            {
                lblTodayNews.Text = reader["TodayNews"].ToString();
            }

            // 读取用户总数
            if (reader.NextResult() && reader.Read())
            {
                lblTotalUsers.Text = reader["TotalUsers"].ToString();
            }

            // 读取管理员数
            if (reader.NextResult() && reader.Read())
            {
                lblAdminCount.Text = reader["AdminCount"].ToString();
            }

            // 读取浏览统计
            if (reader.NextResult() && reader.Read())
            {
                lblTotalViews.Text = reader["TotalViews"].ToString();
                lblAvgViews.Text = Math.Round(Convert.ToDouble(reader["AvgViews"]), 1).ToString();
            }

            // 读取活跃度
            if (reader.NextResult() && reader.Read())
            {
                double activityRate = Math.Round(Convert.ToDouble(reader["ActivityRate"]), 1);
                lblActivityRate.Text = activityRate.ToString("0.0") + "%";
            }

            // 读取更新频率
            if (reader.NextResult() && reader.Read())
            {
                double updateFreq = Math.Round(Convert.ToDouble(reader["UpdateFreq"]), 1);
                lblUpdateFreq.Text = updateFreq.ToString("0.0") + "天/篇";
            }

            reader.Close();
        }
    }

    private void LoadCategoryStats()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                WITH CategoryStats AS (
                    SELECT 
                        ISNULL(Category, '未分类') as Category,
                        COUNT(*) as NewsCount,
                        SUM(Views) as TotalViews,
                        AVG(CAST(Views AS FLOAT)) as AvgViews
                    FROM News 
                    WHERE IsActive = 1 AND PublishDate >= @StartDate AND PublishDate <= @EndDate
                    GROUP BY Category
                )
                SELECT 
                    Category,
                    NewsCount,
                    TotalViews,
                    AvgViews,
                    CAST(NewsCount AS FLOAT) * 100 / SUM(NewsCount) OVER() as Percentage
                FROM CategoryStats
                ORDER BY NewsCount DESC";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@StartDate", startDate);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", endDate);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvCategoryStats.DataSource = dt;
            gvCategoryStats.DataBind();
        }
    }

    private void LoadAuthorStats()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT 
                    u.Username as AuthorName,
                    COUNT(n.NewsID) as NewsCount,
                    SUM(n.Views) as TotalViews,
                    AVG(CAST(n.Views AS FLOAT)) as AvgViews,
                    MAX(n.PublishDate) as LastPublish
                FROM News n
                INNER JOIN Users u ON n.AuthorID = u.UserID
                WHERE n.IsActive = 1 AND n.PublishDate >= @StartDate AND n.PublishDate <= @EndDate
                GROUP BY u.Username
                ORDER BY NewsCount DESC";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@StartDate", startDate);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", endDate);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvAuthorStats.DataSource = dt;
            gvAuthorStats.DataBind();
        }
    }

    private void LoadHotNews()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT TOP 10
                    n.NewsID,
                    n.Title,
                    n.Category,
                    u.Username as AuthorName,
                    n.PublishDate,
                    n.Views,
                    CASE 
                        WHEN n.PublishDate >= DATEADD(DAY, -7, GETDATE()) THEN 'up'
                        WHEN n.Views >= (SELECT AVG(Views) FROM News) THEN 'stable'
                        ELSE 'down'
                    END as ViewsTrend
                FROM News n
                INNER JOIN Users u ON n.AuthorID = u.UserID
                WHERE n.IsActive = 1
                ORDER BY n.Views DESC, n.PublishDate DESC";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvHotNews.DataSource = dt;
            gvHotNews.DataBind();
        }
    }

    private void LoadDailyStats()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                WITH DateSeries AS (
                    SELECT @StartDate as Date
                    UNION ALL
                    SELECT DATEADD(DAY, 1, Date)
                    FROM DateSeries
                    WHERE Date < @EndDate
                )
                SELECT 
                    CONVERT(DATE, ds.Date) as Date,
                    ISNULL(COUNT(n.NewsID), 0) as NewsCount,
                    ISNULL(SUM(n.Views), 0) as TotalViews,
                    ISNULL(AVG(CAST(n.Views AS FLOAT)), 0) as AvgViews
                FROM DateSeries ds
                LEFT JOIN News n ON CONVERT(DATE, n.PublishDate) = CONVERT(DATE, ds.Date) AND n.IsActive = 1
                GROUP BY CONVERT(DATE, ds.Date)
                ORDER BY Date DESC
                OPTION (MAXRECURSION 365)";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@StartDate", startDate);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", endDate);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvDailyStats.DataSource = dt;
            gvDailyStats.DataBind();
        }
    }

    // 获取排名徽章样式
    public string GetRankBadgeClass(int rankIndex)
    {
        switch (rankIndex)
        {
            case 0:
                return "badge-rank-1";
            case 1:
                return "badge-rank-2";
            case 2:
                return "badge-rank-3";
            default:
                return "badge-rank-other";
        }
    }

    // 获取趋势图标
    public string GetTrendIcon(object trend)
    {
        if (trend == null) return "";

        string trendStr = trend.ToString();
        switch (trendStr)
        {
            case "up":
                return "<i class='fas fa-arrow-up text-success' title='上升'></i>";
            case "stable":
                return "<i class='fas fa-minus text-warning' title='稳定'></i>";
            case "down":
                return "<i class='fas fa-arrow-down text-danger' title='下降'></i>";
            default:
                return "";
        }
    }

    protected void ddlTimeRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDateRange(ddlTimeRange.SelectedValue);
        LoadStatistics();
        lblUpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    protected void btnCustomRange_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
        {
            try
            {
                startDate = Convert.ToDateTime(txtStartDate.Text);
                endDate = Convert.ToDateTime(txtEndDate.Text).AddDays(1).AddSeconds(-1);
                LoadStatistics();
                lblUpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                // 日期格式错误
                SetDateRange("week");
                LoadStatistics();
            }
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadStatistics();
        lblUpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportStatistics();
    }

    private void ExportStatistics()
    {
        try
        {
            string fileName = $"新闻统计报表_{DateTime.Now:yyyyMMddHHmmss}.csv";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                sw.WriteLine("新闻发布系统统计报表");
                sw.WriteLine($"统计时间段: {startDate:yyyy-MM-dd} 至 {endDate:yyyy-MM-dd}");
                sw.WriteLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sw.WriteLine();

                // 基本统计数据
                sw.WriteLine("===== 基本统计 =====");
                sw.WriteLine("指标,数值");
                sw.WriteLine($"新闻总数,{lblTotalNews.Text}");
                sw.WriteLine($"今日新增,{lblTodayNews.Text}");
                sw.WriteLine($"用户总数,{lblTotalUsers.Text}");
                sw.WriteLine($"管理员数,{lblAdminCount.Text}");
                sw.WriteLine($"总浏览数,{lblTotalViews.Text}");
                sw.WriteLine($"平均浏览,{lblAvgViews.Text}");
                sw.WriteLine($"活跃度,{lblActivityRate.Text}");
                sw.WriteLine($"更新频率,{lblUpdateFreq.Text}");
                sw.WriteLine();

                // 分类统计
                sw.WriteLine("===== 分类统计 =====");
                sw.WriteLine("分类,新闻数,总浏览数,平均浏览,占比");

                string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = @"
                        SELECT 
                            ISNULL(Category, '未分类') as Category,
                            COUNT(*) as NewsCount,
                            SUM(Views) as TotalViews,
                            AVG(CAST(Views AS FLOAT)) as AvgViews,
                            CAST(COUNT(*) AS FLOAT) * 100 / SUM(COUNT(*)) OVER() as Percentage
                        FROM News 
                        WHERE IsActive = 1 AND PublishDate >= @StartDate AND PublishDate <= @EndDate
                        GROUP BY Category
                        ORDER BY NewsCount DESC";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string category = reader["Category"].ToString();
                        int newsCount = Convert.ToInt32(reader["NewsCount"]);
                        int totalViews = Convert.ToInt32(reader["TotalViews"]);
                        double avgViews = Math.Round(Convert.ToDouble(reader["AvgViews"]), 1);
                        double percentage = Math.Round(Convert.ToDouble(reader["Percentage"]), 1);

                        sw.WriteLine($"{category},{newsCount},{totalViews},{avgViews},{percentage}%");
                    }
                    reader.Close();
                }

                sw.WriteLine();
                sw.WriteLine("===== 报表结束 =====");

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            // 导出失败，不中断页面
            System.Diagnostics.Debug.WriteLine("导出统计时出错: " + ex.Message);
        }
    }
}