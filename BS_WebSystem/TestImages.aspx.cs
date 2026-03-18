using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.UI;

public partial class TestImages : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTestImages();
        }
    }

    private void BindTestImages()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = @"
                SELECT TOP 10 NewsID, Title, Content, ImageUrl, Thumbnail 
                FROM News 
                ORDER BY NewsID DESC";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptTestImages.DataSource = dt;
            rptTestImages.DataBind();
        }
    }

    // 供数据绑定使用的方法 - 解析图片URL
    public string ResolveImageUrl(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            return ResolveUrl("~/Images/default.jpg");
        }

        // 处理相对路径
        if (imageUrl.StartsWith("~/") || imageUrl.StartsWith("/"))
        {
            return ResolveUrl(imageUrl);
        }

        // 如果已经是完整URL，直接返回
        if (imageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            imageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return imageUrl;
        }

        // 处理没有~或/开头的相对路径
        if (!imageUrl.Contains("://"))
        {
            return ResolveUrl("~/Images/" + imageUrl);
        }

        return ResolveUrl("~/Images/default.jpg");
    }

    // 供数据绑定使用的方法 - 检查文件是否存在
    public bool CheckFileExists(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return false;

        try
        {
            string resolvedPath = ResolveImageUrl(imageUrl);

            // 如果是完整的URL（http/https），无法直接检查文件存在
            if (resolvedPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                resolvedPath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return true; // 假设网络图片存在
            }

            // 转换服务器路径
            string physicalPath = Server.MapPath(resolvedPath);
            return System.IO.File.Exists(physicalPath);
        }
        catch
        {
            return false;
        }
    }

    // 从HTML内容中提取第一张图片的URL（供数据绑定使用）
    public string ExtractFirstImageFromContent(string htmlContent)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return null;

        try
        {
            // 使用正则表达式匹配img标签
            string pattern = @"<img[^>]*?src\s*=\s*[""']?([^""'>\s]+)[""']?[^>]*?>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match = regex.Match(htmlContent);

            if (match.Success && match.Groups.Count > 1)
            {
                string imgUrl = match.Groups[1].Value.Trim();

                // 处理相对路径
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    // 如果是以//开头的协议相对URL，添加http:
                    if (imgUrl.StartsWith("//"))
                    {
                        return "http:" + imgUrl;
                    }
                    // 如果是相对路径但没有~，添加~
                    else if (imgUrl.StartsWith("/") && !imgUrl.StartsWith("~/"))
                    {
                        return "~" + imgUrl;
                    }
                    // 如果既不是http/https，也不是以/或~开头，可能是相对路径
                    else if (!imgUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) &&
                             !imgUrl.StartsWith("~/") && !imgUrl.StartsWith("/"))
                    {
                        return "~/" + imgUrl;
                    }

                    return imgUrl;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            return $"提取错误: {ex.Message}";
        }
    }
}