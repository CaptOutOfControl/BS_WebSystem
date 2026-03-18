<%@ Page Title="登录" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0"><i class="fas fa-sign-in-alt me-2"></i>用户登录</h4>
                    </div>
                    <div class="card-body">
                        <!-- 错误信息 -->
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <div class="alert alert-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </div>
                        </asp:PlaceHolder>
                        
                        <!-- 登录表单 -->
                        <div class="mb-3">
                            <label for="UserName" class="form-label">用户名</label>
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" 
                                placeholder="请输入用户名" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="用户名不能为空" />
                        </div>
                        
                        <div class="mb-3">
                            <label for="Password" class="form-label">密码</label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" 
                                CssClass="form-control" placeholder="请输入密码" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="密码不能为空" />
                        </div>
                        
                        <div class="mb-3 form-check">
                            <asp:CheckBox runat="server" ID="RememberMe" CssClass="form-check-input" />
                            <label class="form-check-label" for="RememberMe">记住我</label>
                        </div>
                        
                        <!-- 登录按钮 -->
                        <div class="mb-3">
                            <asp:Button runat="server" OnClick="LogIn" Text="登录" 
                                CssClass="btn btn-primary w-100" />
                        </div>
                        
                        <!-- 注册链接 -->
                        <div class="text-center">
                            <p>还没有账号？ <a href="Register.aspx">立即注册</a></p>
                            <p><a href="Default.aspx">返回首页</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>