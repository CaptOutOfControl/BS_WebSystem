<%@ Page Title="添加新闻" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Admin_AddNews" Codebehind="AddNews.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-lg-10">
                <div class="card">
                    <div class="card-header bg-success text-white">
                        <h4 class="mb-0"><i class="fas fa-plus-circle me-2"></i>添加新闻</h4>
                    </div>
                    <div class="card-body">
                        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert" Visible="false"></asp:Label>
                        
                        <!-- 标题 -->
                        <div class="mb-3">
                            <label for="txtTitle" class="form-label fw-bold">
                                <i class="fas fa-heading me-1"></i>新闻标题
                                <span class="text-danger">*</span>
                            </label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" 
                                placeholder="请输入新闻标题（最多200字）" MaxLength="200" required="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" 
                                ControlToValidate="txtTitle" ErrorMessage="标题不能为空" 
                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        
                        <!-- 分类 -->
                        <div class="mb-3">
                            <label for="ddlCategory" class="form-label fw-bold">
                                <i class="fas fa-tag me-1"></i>新闻分类
                                <span class="text-danger">*</span>
                            </label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">请选择分类</asp:ListItem>
                                <asp:ListItem Value="政治">政治</asp:ListItem>
                                <asp:ListItem Value="经济">经济</asp:ListItem>
                                <asp:ListItem Value="科技">科技</asp:ListItem>
                                <asp:ListItem Value="体育">体育</asp:ListItem>
                                <asp:ListItem Value="娱乐">娱乐</asp:ListItem>
                                <asp:ListItem Value="生活">生活</asp:ListItem>
                                <asp:ListItem Value="教育">教育</asp:ListItem>
                                <asp:ListItem Value="健康">健康</asp:ListItem>
                                <asp:ListItem Value="国际">国际</asp:ListItem>
                                <asp:ListItem Value="系统公告">系统公告</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                                ControlToValidate="ddlCategory" ErrorMessage="请选择分类" 
                                CssClass="text-danger small" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        
                        <!-- 内容 -->
                        <div class="mb-3">
                            <label for="txtContent" class="form-label fw-bold">
                                <i class="fas fa-file-alt me-1"></i>新闻内容
                                <span class="text-danger">*</span>
                            </label>
                            <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" 
                                Rows="15" CssClass="form-control" placeholder="请输入新闻内容，支持HTML格式"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvContent" runat="server" 
                                ControlToValidate="txtContent" ErrorMessage="内容不能为空" 
                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            <div class="form-text">提示：可以使用HTML标签格式化内容，如 &lt;p&gt;、&lt;h2&gt;、&lt;ul&gt; 等</div>
                        </div>
                        
                        <!-- 图片上传 -->
                        <div class="mb-3">
                            <label for="fuImage" class="form-label fw-bold">
                                <i class="fas fa-image me-1"></i>新闻图片
                            </label>
                            <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" 
                                accept=".jpg,.jpeg,.png,.gif,.bmp" />
                            <div class="form-text">
                                支持格式：JPG, PNG, GIF, BMP，大小不超过2MB。如果不上传图片，将使用默认图片。
                            </div>
                        </div>
                        
                        <!-- 状态 -->
                        <div class="mb-4">
                            <label class="form-label fw-bold">
                                <i class="fas fa-eye me-1"></i>发布状态
                            </label>
                            <div>
                                <div class="form-check form-check-inline">
                                    <asp:RadioButton ID="rbPublish" runat="server" GroupName="status" 
                                        Checked="true" CssClass="form-check-input" />
                                    <label class="form-check-label" for="rbPublish">立即发布</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <asp:RadioButton ID="rbDraft" runat="server" GroupName="status" 
                                        CssClass="form-check-input" />
                                    <label class="form-check-label" for="rbDraft">保存为草稿</label>
                                </div>
                            </div>
                        </div>
                        
                        <!-- 按钮组 -->
                        <div class="d-flex justify-content-between border-top pt-4">
                            <asp:Button ID="btnSave" runat="server" Text="保存新闻" 
                                CssClass="btn btn-success px-4" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消返回" 
                                CssClass="btn btn-secondary" OnClick="btnCancel_Click" 
                                CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>