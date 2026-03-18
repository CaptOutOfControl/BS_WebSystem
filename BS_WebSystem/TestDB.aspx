<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestDB.aspx.cs" Inherits="TestDB" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据库测试页面</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-5">
        <h1 class="mb-4">数据库连接测试</h1>
        <div class="card">
            <div class="card-header bg-primary text-white">
                <h4 class="mb-0">测试结果</h4>
            </div>
            <div class="card-body">
                <div id="testResults" runat="server">
                    <!-- 测试结果将在这里显示 -->
                </div>
            </div>
        </div>
        <div class="mt-3">
            <a href="Default.aspx" class="btn btn-secondary">返回首页</a>
        </div>
    </div>
</body>
</html>