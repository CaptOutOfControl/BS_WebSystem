<%@ Page Title="注册" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Register.aspx.cs" Inherits="Account_Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header bg-success text-white">
                        <h4 class="mb-0"><i class="fas fa-user-plus me-2"></i>用户注册</h4>
                    </div>
                    <div class="card-body">
                        <!-- 错误信息 -->
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <div class="alert alert-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </div>
                        </asp:PlaceHolder>
                        
                        <!-- 注册表单 -->
                        <div class="mb-3">
                            <label for="UserName" class="form-label">用户名</label>
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" 
                                placeholder="请输入用户名（3-20个字符）" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="用户名不能为空" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="UserName"
                                ValidationExpression="^[a-zA-Z0-9_]{3,20}$"
                                CssClass="text-danger" ErrorMessage="用户名必须为3-20个字母、数字或下划线" />
                        </div>
                        
                        <div class="mb-3">
                            <label for="Password" class="form-label">密码</label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" 
                                CssClass="form-control" placeholder="请输入密码（至少6位）" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="密码不能为空" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="Password"
                                ValidationExpression="^.{6,}$"
                                CssClass="text-danger" ErrorMessage="密码至少6位" />
                                <!-- 密码强度指示器 -->
                                <div class="mt-2">
                                    <div id="passwordStrengthBar" style="height: 5px; background-color: #e0e0e0; border-radius: 3px;">
                                        <div id="passwordStrengthProgress" style="height: 100%; width: 0%; border-radius: 3px;"></div>
                                    </div>
                                    <small id="passwordStrengthText" class="form-text"></small>
                                </div>
                            </div>
                        </div>
                        
                        <div class="mb-3">
                            <label for="ConfirmPassword" class="form-label">确认密码</label>
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" 
                                CssClass="form-control" placeholder="请再次输入密码" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" ErrorMessage="确认密码不能为空" />
                            <asp:CompareValidator runat="server" ControlToCompare="Password" 
                                ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" ErrorMessage="两次输入的密码不一致" />
                        </div>
                        
                        <div class="mb-3">
                            <label for="Email" class="form-label">邮箱（可选）</label>
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" 
                                placeholder="请输入邮箱地址" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="Email"
                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                CssClass="text-danger" ErrorMessage="邮箱格式不正确" Display="Dynamic" />
                        </div>
                        
                        <!-- 注册按钮 -->
                        <div class="mb-3">
                            <asp:Button runat="server" OnClick="CreateUser_Click" Text="注册" 
                                CssClass="btn btn-success w-100" />
                        </div>
                        
                        <!-- 登录链接 -->
                        <div class="text-center">
                            <p>已有账号？ <a href="Login.aspx">立即登录</a></p>
                            <p><a href="Default.aspx">返回首页</a></p>
                        </div>
                    </div>
            </div>
        </div>
    </div>

<script>
    function checkPasswordStrength(password) {
        if (!password) {
            document.getElementById('passwordStrengthProgress').style.width = '0%';
            document.getElementById('passwordStrengthProgress').style.backgroundColor = '#e0e0e0';
            document.getElementById('passwordStrengthText').textContent = '';
            return;
        }

        let score = 0;

        // 长度评分
        if (password.length >= 8) score++;
        if (password.length >= 12) score++;

        // 包含数字
        if (/\d/.test(password)) score++;

        // 包含小写字母
        if (/[a-z]/.test(password)) score++;

        // 包含大写字母
        if (/[A-Z]/.test(password)) score++;

        // 包含特殊字符
        if (/[^a-zA-Z0-9]/.test(password)) score++;

        // 限制最大分数
        score = Math.min(score, 4);

        // 更新进度条
        const progressBar = document.getElementById('passwordStrengthProgress');
        const strengthText = document.getElementById('passwordStrengthText');
        const width = (score / 4) * 100;

        progressBar.style.width = width + '%';

        // 根据分数设置颜色和文本
        switch (score) {
            case 0:
                progressBar.style.backgroundColor = '#ff0000';
                strengthText.textContent = '非常弱（至少8位，建议包含数字、字母和特殊字符）';
                strengthText.style.color = '#ff0000';
                break;
            case 1:
                progressBar.style.backgroundColor = '#ff6a00';
                strengthText.textContent = '弱';
                strengthText.style.color = '#ff6a00';
                break;
            case 2:
                progressBar.style.backgroundColor = '#ffd800';
                strengthText.textContent = '中等';
                strengthText.style.color = '#ffd800';
                break;
            case 3:
                progressBar.style.backgroundColor = '#00b050';
                strengthText.textContent = '强';
                strengthText.style.color = '#00b050';
                break;
            case 4:
                progressBar.style.backgroundColor = '#00a2e8';
                strengthText.textContent = '非常强';
                strengthText.style.color = '#00a2e8';
                break;
        }
    }
</script>

</asp:Content>

