<%@ Page Title="编辑新闻" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="Admin_EditNews" Codebehind="EditNews.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-lg-10">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0"><i class="fas fa-edit me-2"></i>编辑新闻</h4>
                    </div>
                    <div class="card-body">
                        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert" Visible="false"></asp:Label>
                        
                        <asp:HiddenField ID="hdnNewsID" runat="server" />
                        
                        <!-- 标题 -->
                        <div class="mb-3">
                            <label for="txtTitle" class="form-label fw-bold">
                                <i class="fas fa-heading me-1"></i>新闻标题
                                <span class="text-danger">*</span>
                            </label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" 
                                placeholder="请输入新闻标题" MaxLength="200" required="true"></asp:TextBox>
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
                                Rows="15" CssClass="form-control" placeholder="请输入新闻内容"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvContent" runat="server" 
                                ControlToValidate="txtContent" ErrorMessage="内容不能为空" 
                                CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        
                        <!-- 当前图片 -->
                        <div class="mb-3">
                            <label class="form-label fw-bold">
                                <i class="fas fa-image me-1"></i>当前图片
                            </label>
                            <div class="border rounded p-3 bg-light">
                                <asp:Image ID="imgCurrent" runat="server" CssClass="img-thumbnail mb-2" 
                                    Width="200" Height="150" Visible="false" />
                                <asp:Label ID="lblNoImage" runat="server" Text="暂无图片" 
                                    CssClass="text-muted" Visible="false"></asp:Label>
                                <div class="form-text">如需修改图片，请上传新图片</div>
                            </div>
                        </div>
                        
                        <!-- 上传新图片 -->
                        <div class="mb-3">
                            <label for="fuImage" class="form-label">
                                <i class="fas fa-upload me-1"></i>上传新图片
                            </label>
                            <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" 
                                accept=".jpg,.jpeg,.png,.gif,.bmp" />
                            <div class="form-text">
                                支持格式：JPG, PNG, GIF, BMP，大小不超过2MB
                            </div>
                        </div>
                        
                        <!-- 删除现有图片 -->
                        <div class="mb-3">
                            <div class="form-check">
                                <asp:CheckBox ID="cbRemoveImage" runat="server" 
                                    CssClass="form-check-input" />
                                <label for="cbRemoveImage" class="form-check-label">
                                    删除当前图片（使用默认图片）
                                </label>
                            </div>
                        </div>
                        
                        <!-- 状态 -->
                        <div class="mb-4">
                            <label class="form-label fw-bold">
                                <i class="fas fa-eye me-1"></i>新闻状态
                            </label>
                            <div>
                                <asp:RadioButtonList ID="rblStatus" runat="server" 
                                    RepeatDirection="Horizontal" CssClass="form-check-inline">
                                    <asp:ListItem Value="1" Selected="True" class="form-check-label me-3">
                                        发布
                                    </asp:ListItem>
                                    <asp:ListItem Value="0" class="form-check-label">
                                        草稿
                                    </asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        
                        <!-- 按钮组 -->
                        <div class="d-flex justify-content-between border-top pt-4">
                            <div>
                                <asp:Button ID="btnSave" runat="server" Text="保存修改" 
                                    CssClass="btn btn-primary px-4" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="取消返回" 
                                    CssClass="btn btn-secondary ms-2" OnClick="btnCancel_Click" 
                                    CausesValidation="false" />
                            </div>
                            <asp:Button ID="btnDelete" runat="server" Text="删除新闻" 
                                CssClass="btn btn-danger" OnClick="btnDelete_Click" 
                                CausesValidation="false" />
                        </div>
                    </div>
                    
                    <!-- 卡片底部信息 -->
                    <div class="card-footer text-muted small">
                        <div class="row">
                            <div class="col-md-6">
                                <i class="fas fa-info-circle me-1"></i>
                                最后修改时间：
                                <asp:Label ID="lblUpdateInfo" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-6 text-end">
                                <i class="fas fa-eye me-1"></i>
                                浏览次数：
                                <asp:Label ID="lblViewCount" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>