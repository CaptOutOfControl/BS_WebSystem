<%@ Page Title="认证修复" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="FixAuth" Codebehind="FixAuth.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header bg-danger text-white">
                        <h4 class="mb-0"><i class="fas fa-medkit me-2"></i>认证系统修复</h4>
                    </div>
                    <div class="card-body">
                        <!-- 当前状态 -->
                        <div class="alert alert-info">
                            <h5><i class="fas fa-info-circle me-2"></i>当前状态</h5>
                            <p>Session中有用户但User.Identity未认证。这表明FormsAuthentication票据没有正确创建。</p>
                        </div>
                        
                        <!-- 诊断信息 -->
                        <div class="card mb-3">
                            <div class="card-header">
                                <h5 class="mb-0">诊断信息</h5>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <p><strong>Session["CurrentUser"]:</strong> <code id="sessionUser" runat="server"></code></p>
                                        <p><strong>Session["CurrentRole"]:</strong> <code id="sessionRole" runat="server"></code></p>
                                    </div>
                                    <div class="col-md-6">
                                        <p><strong>User.Identity.Name:</strong> <code id="identityName" runat="server"></code></p>
                                        <p><strong>User.Identity.IsAuthenticated:</strong> <code id="isAuthenticated" runat="server"></code></p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- 修复选项 -->
                        <div class="card mb-3">
                            <div class="card-header bg-warning">
                                <h5 class="mb-0">修复选项</h5>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 mb-3">
                                        <asp:Button ID="btnManualAuth" runat="server" Text="手动设置认证" 
                                            CssClass="btn btn-primary w-100" OnClick="btnManualAuth_Click" 
                                            OnClientClick="return confirm('这将使用Session中的用户信息创建认证票据，继续吗？');" />
                                        <small class="form-text">使用Session中的用户创建认证票据</small>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <asp:Button ID="btnTestAuth" runat="server" Text="测试认证流程" 
                                            CssClass="btn btn-info w-100" OnClick="btnTestAuth_Click" />
                                        <small class="form-text">模拟完整的登录认证流程</small>
                                    </div>
                                    <div class="col-md-4 mb-3">
                                        <asp:Button ID="btnClearAndLogin" runat="server" Text="清除并重新登录" 
                                            CssClass="btn btn-danger w-100" OnClick="btnClearAndLogin_Click" />
                                        <small class="form-text">清除所有认证信息，重新登录</small>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Web.config检查 -->
                        <div class="card mb-3">
                            <div class="card-header">
                                <h5 class="mb-0">Web.config 配置检查</h5>
                            </div>
                            <div class="card-body">
                                <asp:Literal ID="litConfigCheck" runat="server"></asp:Literal>
                            </div>
                        </div>
                        
                        <!-- Cookies检查 -->
                        <div class="card mb-3">
                            <div class="card-header">
                                <h5 class="mb-0">Cookies 检查</h5>
                            </div>
                            <div class="card-body">
                                <asp:Repeater ID="rptCookies" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-bordered table-sm">
                                            <thead>
                                                <tr>
                                                    <th>Cookie名称</th>
                                                    <th>值</th>
                                                    <th>Domain</th>
                                                    <th>Path</th>
                                                    <th>Expires</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><strong><%# Eval("Name") %></strong></td>
                                            <td class="text-truncate" style="max-width: 300px;" title='<%# Eval("Value") %>'>
                                                <%# TruncateValue(Eval("Value").ToString(), 50) %>
                                            </td>
                                            <td><%# Eval("Domain") %></td>
                                            <td><%# Eval("Path") %></td>
                                            <td><%# Eval("Expires") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblNoCookies" runat="server" Text="未找到Cookies" 
                                    CssClass="alert alert-warning" Visible="false"></asp:Label>
                            </div>
                        </div>
                        
                        <!-- 操作结果 -->
                        <asp:Panel ID="pnlResult" runat="server" Visible="false" CssClass="alert alert-success mt-3">
                            <asp:Literal ID="litResult" runat="server"></asp:Literal>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>