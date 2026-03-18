using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 禁用缓存
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();

            // 检查是否有退出消息
            if (Request.QueryString["logout"] != null)
            {
                // 可以在页面上显示退出成功的消息
                // 这里可以添加一个Label控件来显示消息
            }

            // 绑定数据
            BindTopNews();
            BindNewsList();
            BindHotNews();
            BindCategories();


        }
    }
    private void BindTopNews()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT NewsID, Title, Content, Category, PublishDate, Views, ImageUrl, Thumbnail 
                FROM News 
                WHERE IsActive = 1 
                ORDER BY PublishDate DESC";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // 处理轮播图数据，提取或设置图片
            // 如果没有Thumbnail，从Content中提取第一张图片
            foreach (DataRow row in dt.Rows)
            {
                string thumbnail = row["Thumbnail"] as string;

                // 如果没有设置缩略图，从内容中提取
                if (string.IsNullOrEmpty(thumbnail) || thumbnail == "~/Images/default.jpg")
                {
                    string content = row["Content"] as string;
                    string firstImage = ExtractFirstImageFromContent(content);

                    if (!string.IsNullOrEmpty(firstImage))
                    {
                        row["Thumbnail"] = firstImage;
                    }
                    else if (!string.IsNullOrEmpty(row["ImageUrl"] as string))
                    {
                        // 如果内容中没有图片，使用新闻主图
                        row["Thumbnail"] = row["ImageUrl"];
                    }
                    else
                    {
                        // 都没有，使用默认图片
                        row["Thumbnail"] = "~/Images/default.jpg";
                    }
                }
            }

            rptTopNews.DataSource = dt;
            rptTopNews.DataBind();
        }
    }

    private void BindNewsList()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT NewsID, Title, Content, Category, PublishDate, Views, ImageUrl, Thumbnail 
                FROM News 
                WHERE IsActive = 1 
                ORDER BY PublishDate DESC";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // 如果没有Thumbnail，从Content中提取第一张图片
            foreach (DataRow row in dt.Rows)
            {
                string thumbnail = row["Thumbnail"] as string;

                // 如果没有设置缩略图，从内容中提取
                if (string.IsNullOrEmpty(thumbnail) || thumbnail == "~/Images/default.jpg")
                {
                    string content = row["Content"] as string;
                    string firstImage = ExtractFirstImageFromContent(content);

                    if (!string.IsNullOrEmpty(firstImage))
                    {
                        row["Thumbnail"] = firstImage;
                    }
                    else if (!string.IsNullOrEmpty(row["ImageUrl"] as string))
                    {
                        // 如果内容中没有图片，使用新闻主图
                        row["Thumbnail"] = row["ImageUrl"];
                    }
                    else
                    {
                        // 都没有，使用默认图片
                        row["Thumbnail"] = "~/Images/default.jpg";
                    }
                }
            }

            lvNews.DataSource = dt;
            lvNews.DataBind();
        }
    }

    private void BindHotNews()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT TOP 5 NewsID, Title, PublishDate, Views 
                FROM News 
                WHERE IsActive = 1 
                ORDER BY Views DESC";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptHotNews.DataSource = dt;
            rptHotNews.DataBind();
        }
    }

    private void BindCategories()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT DISTINCT Category 
                FROM News 
                WHERE IsActive = 1 AND Category IS NOT NULL AND Category != ''
                ORDER BY Category";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptCategories.DataSource = dt;
            rptCategories.DataBind();
        }
    }

    public string GetImageUrl(object imageUrlObj)
    {
        if (imageUrlObj == null || string.IsNullOrEmpty(imageUrlObj.ToString()))
        {
            return "Images/default.jpg";
        }

        string imageUrl = imageUrlObj.ToString();

        // 确保路径正确
        if (imageUrl.StartsWith("~/"))
        {
            return imageUrl.Substring(2); // 去掉~/
        }

        return imageUrl;
    }


    // 从HTML内容中提取第一张图片的URL
    private string ExtractFirstImageFromContent(string htmlContent)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return null;

        try
        {
            // 使用正则表达式匹配img标签
            string pattern = @"<img[^>]*src\s*=\s*[""']?([^""'>]+)[""']?[^>]*>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(htmlContent);

            if (match.Success && match.Groups.Count > 1)
            {
                string imgUrl = match.Groups[1].Value;

                // 处理相对路径
                if (imgUrl.StartsWith("/"))
                {
                    return "~" + imgUrl;
                }
                else if (!imgUrl.StartsWith("http") && !imgUrl.StartsWith("~/"))
                {
                    return "~/" + imgUrl;
                }

                return imgUrl;
            }

            // 尝试匹配其他格式的图片标签
            pattern = @"src\s*=\s*[""']([^""'>]+\.(?:jpg|jpeg|png|gif|bmp))[""']";
            regex = new Regex(pattern, RegexOptions.IgnoreCase);
            match = regex.Match(htmlContent);

            if (match.Success && match.Groups.Count > 1)
            {
                string imgUrl = match.Groups[1].Value;

                // 处理相对路径
                if (imgUrl.StartsWith("/"))
                {
                    return "~" + imgUrl;
                }
                else if (!imgUrl.StartsWith("http") && !imgUrl.StartsWith("~/"))
                {
                    return "~/" + imgUrl;
                }

                return imgUrl;
            }
        }
        catch (Exception ex)
        {
            // 记录错误但不影响程序运行
            System.Diagnostics.Debug.WriteLine("提取图片时出错: " + ex.Message);
        }

        return null;
    }

    // 截断内容，但保留部分HTML标签（如<strong>, <em>等）
    public string TruncateContent(string content, int length)
    {
        if (string.IsNullOrEmpty(content))
            return "";
        
        // 首先提取纯文本，用于预览
        string plainText = StripHtmlTags(content);
        
        if (plainText.Length > length)
        {
            return plainText.Substring(0, length) + "...";
        }
        return plainText;
    }

    // 去除HTML标签，只保留文本
    private string StripHtmlTags(string html)
    {
        if (string.IsNullOrEmpty(html))
            return "";

        // 移除所有HTML标签
        string plainText = Regex.Replace(html, "<.*?>", string.Empty);

        // 替换HTML实体
        plainText = System.Web.HttpUtility.HtmlDecode(plainText);

        // 移除多余的空格和换行
        plainText = Regex.Replace(plainText, @"\s+", " ").Trim();

        return plainText;
    }

    // 在发布新闻时自动提取缩略图
    public static string AutoExtractThumbnail(string content, string imageUrl)
    {
        if (!string.IsNullOrEmpty(imageUrl) && imageUrl != "~/Images/default.jpg")
        {
            return imageUrl; // 优先使用上传的图片
        }

        // 从内容中提取第一张图片
        if (!string.IsNullOrEmpty(content))
        {
            string pattern = @"<img[^>]*src\s*=\s*[""']?([^""'>]+)[""']?[^>]*>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(content);

            if (match.Success && match.Groups.Count > 1)
            {
                string imgUrl = match.Groups[1].Value;

                // 处理相对路径
                if (imgUrl.StartsWith("/"))
                {
                    return "~" + imgUrl;
                }
                else if (!imgUrl.StartsWith("http") && !imgUrl.StartsWith("~/"))
                {
                    return "~/" + imgUrl;
                }

                return imgUrl;
            }
        }

        return "~/Images/default.jpg"; // 默认图片
    }

    // 获取缩略图URL
    public string GetThumbnailUrl(object dataItem)
    {
        DataRowView rowView = dataItem as DataRowView;
        if (rowView != null)
        {
                    // 调试输出
        string debugInfo = $"RowView类型: {rowView.GetType().Name}, ";
        
        // 检查字段
        debugInfo += $"Thumbnail字段存在: {rowView.DataView.Table.Columns.Contains("Thumbnail")}, ";
        debugInfo += $"ImageUrl字段存在: {rowView.DataView.Table.Columns.Contains("ImageUrl")}, ";
        debugInfo += $"Content字段存在: {rowView.DataView.Table.Columns.Contains("Content")}";
        
        System.Diagnostics.Debug.WriteLine(debugInfo);
        

            // 优先使用Thumbnail字段
            string thumbnail = rowView["Thumbnail"] as string;
            if (!string.IsNullOrEmpty(thumbnail))
            {
                return thumbnail;
            }

            // 其次使用ImageUrl字段
            string imageUrl = rowView["ImageUrl"] as string;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return imageUrl;
            }

            // 从内容中提取
            string content = rowView["Content"] as string;
            string extractedImage = ExtractFirstImageFromContent(content);
            if (!string.IsNullOrEmpty(extractedImage))
            {
                return extractedImage;
            }
        }

        // 默认图片
        return "~/Images/default.jpg";
    }

    public string GetImageUrl(object dbImageUrl, object content)
    {
        string imageUrl = dbImageUrl?.ToString();
        string htmlContent = content?.ToString();

        // 如果数据库中有图片URL，直接使用
        if (!string.IsNullOrEmpty(imageUrl) && imageUrl != "~/Images/default.jpg")
        {
            return imageUrl;
        }

        // 否则从内容中提取
        return ExtractFirstImageFromContent(htmlContent);
    }

}