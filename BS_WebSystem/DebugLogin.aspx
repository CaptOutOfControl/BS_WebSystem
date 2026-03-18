<%@ Page Title="登录调试" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DebugLogin" Codebehind="DebugLogin.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header bg-warning text-dark">
                    <h4 class="mb-0"><i class="fas fa-bug me-2"></i>登录状态调试信息</h4>
                </div>
                <div class="card-body">
                    <!-- User.Identity 信息 -->
                    <div class="card mb-3">
                        <div class="card-header bg-info text-white">
                            <h5 class="mb-0"><i class="fas fa-user me-2"></i>User.Identity 信息</h5>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <table class="table table-bordered">
                                    <tbody>
                                        <tr>
                                            <th width="30%">IsAuthenticated</th>
                                            <td><code id="tdIsAuthenticated" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>Name</th>
                                            <td><code id="tdName" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>AuthenticationType</th>
                                            <td><code id="tdAuthenticationType" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>Identity Type</th>
                                            <td><code id="tdIdentityType" runat="server"></code></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <!-- Session 信息 -->
                    <div class="card mb-3">
                        <div class="card-header bg-success text-white">
                            <h5 class="mb-0"><i class="fas fa-server me-2"></i>Session 信息</h5>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <table class="table table-bordered">
                                    <tbody>
                                        <tr>
                                            <th width="30%">SessionID</th>
                                            <td><code id="tdSessionID" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>CurrentUser</th>
                                            <td><code id="tdCurrentUser" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>CurrentRole</th>
                                            <td><code id="tdCurrentRole" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>Session 状态</th>
                                            <td><code id="tdSessionState" runat="server"></code></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <!-- Cookies 信息 -->
                    <div class="card mb-3">
                        <div class="card-header bg-danger text-white">
                            <h5 class="mb-0"><i class="fas fa-cookie me-2"></i>Cookies 信息</h5>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="rptCookies" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-bordered">
                                            <thead>
                                                <tr>
                                                    <th width="30%">Cookie 名称</th>
                                                    <th>值</th>
                                                    <th width="20%">过期时间</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><strong><%# Eval("Key") %></strong></td>
                                            <td><code><%# Eval("Value") %></code></td>
                                            <td><%# Eval("Expires") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblNoCookies" runat="server" Text="无Cookies" 
                                    CssClass="alert alert-warning" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- FormsAuthentication 信息 -->
                    <div class="card mb-3">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0"><i class="fas fa-lock me-2"></i>FormsAuthentication 信息</h5>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <table class="table table-bordered">
                                    <tbody>
                                        <tr>
                                            <th width="30%">FormsCookieName</th>
                                            <td><code id="tdFormsCookieName" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>LoginUrl</th>
                                            <td><code id="tdLoginUrl" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>DefaultUrl</th>
                                            <td><code id="tdDefaultUrl" runat="server"></code></td>
                                        </tr>
                                        <tr>
                                            <th>Cookie 模式</th>
                                            <td><code id="tdCookieMode" runat="server"></code></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <!-- 修复建议 -->
                    <asp:Panel ID="pnlFixSuggestions" runat="server" Visible="false">
                        <div class="card mb-3 border-danger">
                            <div class="card-header bg-danger text-white">
                                <h5 class="mb-0"><i class="fas fa-wrench me-2"></i>登录状态修复建议</h5>
                            </div>
                            <div class="card-body">
                                <div id="fixSuggestions" runat="server"></div>
                                <asp:Button ID="btnFixLogin" runat="server" Text="尝试修复登录状态" 
                                    CssClass="btn btn-warning mt-2" OnClick="btnFixLogin_Click" />
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- 操作按钮 -->
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fas fa-cogs me-2"></i>操作</h5>
                        </div>
                        <div class="card-body text-center">
                            <div class="row">
                                <div class="col-md-3 mb-2">
                                    <a href="Account/Login.aspx" class="btn btn-primary w-100">
                                        <i class="fas fa-sign-in-alt me-1"></i>去登录
                                    </a>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <a href="Account/Register.aspx" class="btn btn-success w-100">
                                        <i class="fas fa-user-plus me-1"></i>去注册
                                    </a>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <a href="Default.aspx" class="btn btn-secondary w-100">
                                        <i class="fas fa-home me-1"></i>回首页
                                    </a>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <asp:Button ID="btnClearSession" runat="server" Text="清空Session" 
                                        CssClass="btn btn-danger w-100" OnClick="btnClearSession_Click" />
                                </div>
                            </div>
                            <div class="row mt-2">
                                <div class="col-md-4 mb-2">
                                    <asp:Button ID="btnTestAdminLogin" runat="server" Text="测试Admin登录" 
                                        CssClass="btn btn-info w-100" OnClick="btnTestAdminLogin_Click" />
                                </div>
                                <div class="col-md-4 mb-2">
                                    <asp:Button ID="btnLogout" runat="server" Text="退出登录" 
                                        CssClass="btn btn-outline-danger w-100" OnClick="btnLogout_Click" />
                                </div>
                                <div class="col-md-4 mb-2">
                                    <asp:Button ID="btnRefresh" runat="server" Text="刷新页面" 
                                        CssClass="btn btn-outline-primary w-100" OnClick="btnRefresh_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>