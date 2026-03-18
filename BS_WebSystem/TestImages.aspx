<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestImages.aspx.cs" Inherits="TestImages" %>

<!DOCTYPE html>
<html>
<head>
    <title>图片测试</title>
    <style>
        .image-container { margin: 10px; padding: 10px; border: 1px solid #ccc; }
        .image-item { margin-bottom: 20px; }
        img { max-width: 300px; max-height: 200px; border: 1px solid #ddd; }
        .error { color: red; }
        .success { color: green; }
    </style>
</head>
<body>
    <div class="container">
        <h2>图片路径测试</h2>
        
        <h3>物理路径测试</h3>
        <div class="image-container">
            <div class="image-item">
                <h4>默认图片路径测试</h4>
                <p>路径: ~/Images/default.jpg</p>
                <p>解析后: <%= ResolveUrl("~/Images/default.jpg") %></p>
                <p>物理路径: <%= Server.MapPath("~/Images/default.jpg") %></p>
                <p>文件存在: <span class="<%= System.IO.File.Exists(Server.MapPath("~/Images/default.jpg")) ? "success" : "error" %>">
                    <%= System.IO.File.Exists(Server.MapPath("~/Images/default.jpg")) %>
                </span></p>
                <img src="<%= ResolveUrl("~/Images/default.jpg") %>" alt="默认图片" 
                     onerror="this.onerror=null;this.src='<%= ResolveUrl("~/Images/default.jpg") %>';">
            </div>
            
            <div class="image-item">
                <h4>其他测试路径</h4>
                <p>~/Images/ 目录是否存在: <%= System.IO.Directory.Exists(Server.MapPath("~/Images/")) %></p>
                <p>~/Images/ 目录内容: 
                    <%
                        var imagesPath = Server.MapPath("~/Images/");
                        if (System.IO.Directory.Exists(imagesPath))
                        {
                            var files = System.IO.Directory.GetFiles(imagesPath, "*.jpg");
                            Response.Write($"找到 {files.Length} 个JPG文件");
                            if (files.Length > 0)
                            {
                                Response.Write(": " + string.Join(", ", files.Select(f => System.IO.Path.GetFileName(f))));
                            }
                        }
                        else
                        {
                            Response.Write("目录不存在");
                        }
                    %>
                </p>
            </div>
        </div>
        
        <h3>数据库中的图片</h3>
        <asp:Repeater ID="rptTestImages" runat="server">
            <ItemTemplate>
                <div class="image-container">
                    <div class="image-item">
                        <h4><%# Eval("Title") %></h4>
                        <p>ID: <%# Eval("NewsID") %></p>
                        <p>ImageUrl: <span class="<%# string.IsNullOrEmpty(Eval("ImageUrl")?.ToString()) ? "error" : "" %>">
                            <%# Eval("ImageUrl") ?? "(空)" %></span>
                        </p>
                        <p>Thumbnail: <span class="<%# string.IsNullOrEmpty(Eval("Thumbnail")?.ToString()) ? "error" : "" %>">
                            <%# Eval("Thumbnail") ?? "(空)" %></span>
                        </p>
                        
                        <h5>ImageUrl图片:</h5>
                        <p>解析后: <%# ResolveImageUrl(Eval("ImageUrl")?.ToString()) %></p>
                        <p>文件存在: 
                            <span class="<%# CheckFileExists(Eval("ImageUrl")?.ToString()) ? "success" : "error" %>">
                                <%# CheckFileExists(Eval("ImageUrl")?.ToString()) %>
                            </span>
                        </p>
                        <img src="<%# ResolveImageUrl(Eval("ImageUrl")?.ToString()) %>" 
                             alt="ImageUrl" onerror="markImageError(this);">
                        
                        <h5>Thumbnail图片:</h5>
                        <p>解析后: <%# ResolveImageUrl(Eval("Thumbnail")?.ToString()) %></p>
                        <p>文件存在: 
                            <span class="<%# CheckFileExists(Eval("Thumbnail")?.ToString()) ? "success" : "error" %>">
                                <%# CheckFileExists(Eval("Thumbnail")?.ToString()) %>
                            </span>
                        </p>
                        <img src="<%# ResolveImageUrl(Eval("Thumbnail")?.ToString()) %>" 
                             alt="Thumbnail" onerror="markImageError(this);">
                        
                        <h5>从内容提取的图片:</h5>
                        <p>提取的URL: <%# ExtractFirstImageFromContent(Eval("Content")?.ToString()) %></p>
                        <img src="<%# ResolveImageUrl(ExtractFirstImageFromContent(Eval("Content")?.ToString())) %>" 
                             alt="内容图片" onerror="markImageError(this);">
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
    <script type="text/javascript">
        function markImageError(img) {
            img.style.border = '2px solid red';
            img.title = '图片加载失败: ' + img.src;
            console.error('图片加载失败:', img.src);
        }
    </script>
</body>
</html>