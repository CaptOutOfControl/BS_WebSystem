<%@ Page Title="联系我们" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Contact" Codebehind="Contact.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <!-- 页面标题 -->
                <div class="text-center mb-5">
                    <h1 class="display-4 mb-3"><i class="fas fa-envelope-open-text me-3"></i>联系我们</h1>
                    <p class="lead text-muted">有任何问题或建议？欢迎与我们联系</p>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- 左侧联系信息 -->
            <div class="col-lg-6 mb-5">
                <div class="card h-100 border-0 shadow">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0"><i class="fas fa-info-circle me-2"></i>联系信息</h4>
                    </div>
                    <div class="card-body">
                        <!-- 公司信息 -->
                        <div class="text-center mb-4">
                            <i class="fas fa-newspaper fa-3x text-primary mb-3"></i>
                            <h3>环球讯报</h3>
                            <p class="text-muted">提供最新、最全面的新闻资讯</p>
                        </div>
                        
                        <!-- 联系卡片 -->
                        <div class="row g-3">
                            <div class="col-md-6">
                                <div class="contact-card text-center p-3 border rounded h-100">
                                    <div class="contact-icon mb-3">
                                        <i class="fas fa-map-marker-alt fa-2x text-primary"></i>
                                    </div>
                                    <h5 class="mb-2">办公地址</h5>
                                    <p class="text-muted mb-0">
                                        北京市丰台区花乡张家路口121号<br>
                                        首都经济贸易大学<br>
                                        邮编：100070
                                    </p>
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <div class="contact-card text-center p-3 border rounded h-100">
                                    <div class="contact-icon mb-3">
                                        <i class="fas fa-phone fa-2x text-success"></i>
                                    </div>
                                    <h5 class="mb-2">联系电话</h5>
                                    <p class="text-muted mb-0">
                                        客服热线：400-123-4567<br>
                                        办公电话：137-20071615<br>
                                        传真：010-87654321
                                    </p>
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <div class="contact-card text-center p-3 border rounded h-100">
                                    <div class="contact-icon mb-3">
                                        <i class="fas fa-envelope fa-2x text-danger"></i>
                                    </div>
                                    <h5 class="mb-2">电子邮箱</h5>
                                    <p class="text-muted mb-0">
                                        客服邮箱：service@newssystem.com<br>
                                        投稿邮箱：news@newssystem.com<br>
                                        合作邮箱：cooperation@newssystem.com
                                    </p>
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <div class="contact-card text-center p-3 border rounded h-100">
                                    <div class="contact-icon mb-3">
                                        <i class="fas fa-clock fa-2x text-info"></i>
                                    </div>
                                    <h5 class="mb-2">工作时间</h5>
                                    <p class="text-muted mb-0">
                                        周一至周五：9:00-18:00<br>
                                        周六：9:00-12:00<br>
                                        周日：休息
                                    </p>
                                </div>
                            </div>
                        </div>
                        
                        <!-- 地图占位 -->
                        <div class="mt-4">
                            <div class="map-placeholder border rounded p-3 text-center">
                                <i class="fas fa-map fa-2x text-muted mb-3"></i>
                                <h5>我们的位置</h5>
                                <p class="text-muted">北京市丰台区首经贸中街</p>
                                <div class="p-3 bg-light rounded">
                                    <small>地图显示位置</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 右侧联系表单 -->
            <div class="col-lg-6 mb-5">
                <div class="card border-0 shadow">
                    <div class="card-header bg-success text-white">
                        <h4 class="mb-0"><i class="fas fa-paper-plane me-2"></i>在线留言</h4>
                    </div>
                    <div class="card-body">
                        <!-- 消息提示 -->
                        <asp:Panel ID="pnlMessage" runat="server" CssClass="alert" Visible="false">
                            <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                        </asp:Panel>
                        
                        <!-- 联系表单 -->
                        <div class="mb-3">
                            <label for="txtName" class="form-label">
                                <i class="fas fa-user me-1"></i>姓名
                                <span class="text-danger">*</span>
                            </label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" 
                                placeholder="请输入您的姓名" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                                ControlToValidate="txtName" ErrorMessage="请输入姓名" 
                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtEmail" class="form-label">
                                <i class="fas fa-envelope me-1"></i>电子邮箱
                                <span class="text-danger">*</span>
                            </label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                placeholder="请输入您的邮箱地址" TextMode="Email"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                ControlToValidate="txtEmail" ErrorMessage="请输入邮箱地址" 
                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                ControlToValidate="txtEmail" ErrorMessage="邮箱格式不正确" 
                                ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                                CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtPhone" class="form-label">
                                <i class="fas fa-phone me-1"></i>联系电话
                            </label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" 
                                placeholder="请输入您的联系电话"></asp:TextBox>
                        </div>
                        
                        <div class="mb-3">
                            <label for="ddlSubject" class="form-label">
                                <i class="fas fa-tag me-1"></i>咨询类型
                                <span class="text-danger">*</span>
                            </label>
                            <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">请选择咨询类型</asp:ListItem>
                                <asp:ListItem Value="新闻投稿">新闻投稿</asp:ListItem>
                                <asp:ListItem Value="广告合作">广告合作</asp:ListItem>
                                <asp:ListItem Value="网站建议">网站建议</asp:ListItem>
                                <asp:ListItem Value="技术问题">技术问题</asp:ListItem>
                                <asp:ListItem Value="其他咨询">其他咨询</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvSubject" runat="server" 
                                ControlToValidate="ddlSubject" ErrorMessage="请选择咨询类型" 
                                CssClass="text-danger small" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtMessage" class="form-label">
                                <i class="fas fa-comment-dots me-1"></i>留言内容
                                <span class="text-danger">*</span>
                            </label>
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" 
                                placeholder="请详细描述您的问题或建议" TextMode="MultiLine" Rows="6"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" 
                                ControlToValidate="txtMessage" ErrorMessage="请输入留言内容" 
                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        
                        <!-- 验证码（可选） -->
                        <div class="mb-3">
                            <label for="txtCaptcha" class="form-label">
                                <i class="fas fa-shield-alt me-1"></i>验证码
                            </label>
                            <div class="row g-2 align-items-center">
                                <div class="col-6">
                                    <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control" 
                                        placeholder="请输入验证码" MaxLength="4"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <div class="captcha-container border rounded p-2 text-center bg-light">
                                        <span class="captcha-text" id="captchaText" runat="server">1234</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- 提交按钮 -->
                        <div class="d-grid gap-2 mt-4">
                            <asp:Button ID="btnSubmit" runat="server" Text="提交留言" 
                                CssClass="btn btn-success btn-lg" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnReset" runat="server" Text="重置表单" 
                                CssClass="btn btn-outline-secondary" OnClick="btnReset_Click" 
                                CausesValidation="false" />
                        </div>
                        
                        <!-- 服务承诺 -->
                        <div class="mt-4 pt-3 border-top text-center">
                            <p class="text-muted small">
                                <i class="fas fa-lock me-1"></i>
                                我们承诺保护您的隐私，您的信息将仅用于回复您的咨询
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 常见问题 -->
        <div class="row">
            <div class="col-12">
                <div class="card border-0 shadow mb-5">
                    <div class="card-header bg-info text-white">
                        <h4 class="mb-0"><i class="fas fa-question-circle me-2"></i>常见问题</h4>
                    </div>
                    <div class="card-body">
                        <div class="accordion" id="faqAccordion">
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#faq1">
                                        如何向网站投稿？
                                    </button>
                                </h2>
                                <div id="faq1" class="accordion-collapse collapse show" data-bs-parent="#faqAccordion">
                                    <div class="accordion-body">
                                        <p>您可以向我们的投稿邮箱 <strong>news@newssystem.com</strong> 发送稿件，或通过网站后台管理系统的"添加新闻"功能进行投稿。</p>
                                        <p>投稿要求：</p>
                                        <ul>
                                            <li>稿件内容真实可靠</li>
                                            <li>格式规范，图文并茂</li>
                                            <li>需注明作者和联系方式</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#faq2">
                                        如何成为网站管理员？
                                    </button>
                                </h2>
                                <div id="faq2" class="accordion-collapse collapse" data-bs-parent="#faqAccordion">
                                    <div class="accordion-body">
                                        <p>目前网站管理员由系统管理员分配，如需申请管理权限，请发送申请邮件至 <strong>admin@newssystem.com</strong>。</p>
                                        <p>申请要求：</p>
                                        <ol>
                                            <li>提供个人基本信息和联系方式</li>
                                            <li>说明申请管理权限的理由</li>
                                            <li>提供相关经验证明（如有）</li>
                                        </ol>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#faq3">
                                        发现网站错误如何反馈？
                                    </button>
                                </h2>
                                <div id="faq3" class="accordion-collapse collapse" data-bs-parent="#faqAccordion">
                                    <div class="accordion-body">
                                        <p>如果您发现网站存在错误或技术问题，可以通过以下方式反馈：</p>
                                        <ul>
                                            <li>通过本页面的在线留言表单提交问题</li>
                                            <li>发送邮件至 <strong>tech@newssystem.com</strong></li>
                                            <li>拨打技术支持热线：400-123-4567</li>
                                        </ul>
                                        <p>反馈时请尽量提供详细的信息，包括：问题描述、页面URL、截图等。</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- 自定义CSS样式 -->
    <style>
        .contact-card {
            transition: all 0.3s ease;
            height: 100%;
        }
        
        .contact-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            border-color: #0d6efd !important;
        }
        
        .contact-icon {
            width: 60px;
            height: 60px;
            line-height: 60px;
            border-radius: 50%;
            background-color: rgba(13, 110, 253, 0.1);
            margin: 0 auto;
        }
        
        .contact-icon i {
            line-height: 60px;
        }
        
        .map-placeholder {
            background-color: #f8f9fa;
            transition: all 0.3s ease;
        }
        
        .map-placeholder:hover {
            background-color: #e9ecef;
        }
        
        .captcha-container {
            height: 46px;
            line-height: 30px;
            font-size: 20px;
            font-weight: bold;
            letter-spacing: 5px;
            color: #0d6efd;
            user-select: none;
            cursor: pointer;
        }
        
        .accordion-button:not(.collapsed) {
            background-color: rgba(13, 110, 253, 0.1);
            color: #0d6efd;
        }
        
        .btn-lg {
            padding: 12px 24px;
            font-size: 18px;
        }
        
        /* 响应式调整 */
        @media (max-width: 768px) {
            .contact-card {
                margin-bottom: 15px;
            }
            
            .btn-lg {
                padding: 10px 20px;
                font-size: 16px;
            }
        }
    </style>
</asp:Content>