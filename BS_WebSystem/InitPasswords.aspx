<%@ Page Title="初始化密码" Language="C#" AutoEventWireup="true" 
    CodeFile="InitPasswords.aspx.cs" Inherits="InitPasswords" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>密码初始化工具</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
</head>
<body>
    <form id="form1" runat="server"> <!-- 添加这个form标签 -->
        <div class="container mt-5">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="card">
                        <div class="card-header bg-warning">
                            <h4 class="mb-0"><i class="fas fa-key me-2"></i>密码哈希初始化工具</h4>
                        </div>
                        <div class="card-body">
                            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
                            
                            <div class="alert alert-info">
                                <h5><i class="fas fa-info-circle me-2"></i>说明：</h5>
                                <p>由于我们添加了密码哈希加盐功能，需要为现有用户设置新的哈希密码。</p>
                                <p>请为以下用户设置密码：</p>
                                <ul>
                                    <li>管理员账号：admin (默认密码：admin123)</li>
                                    <li>测试用户：user1 (默认密码：user123)</li>
                                </ul>
                                <p><strong>注意：</strong>操作完成后，请立即删除此页面！</p>
                            </div>
                            
                            <div class="text-center mt-4">
                                <asp:Button ID="btnInitPasswords" runat="server" Text="初始化密码" 
                                    CssClass="btn btn-primary btn-lg" OnClick="btnInitPasswords_Click" />
                                <br />
                                <small class="text-muted mt-2 d-block">点击后将为所有现有用户设置哈希密码</small>
                            </div>
                            
                            <div class="mt-4" id="results" runat="server" visible="false">
                                <h5><i class="fas fa-list-check me-2"></i>操作结果：</h5>
                                <div class="border rounded p-3 bg-light">
                                    <asp:Literal ID="litResults" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer text-muted text-center">
                            <a href="Default.aspx" class="btn btn-sm btn-outline-secondary">
                                <i class="fas fa-home me-1"></i>返回首页
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form> <!-- 结束form标签 -->
</body>
</html>