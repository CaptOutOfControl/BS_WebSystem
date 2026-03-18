<%@ Page Title="新闻管理" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Admin_NewsManage" Codebehind="NewsManage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <div class="d-flex justify-content-between align-items-center">
                        <h4 class="mb-0"><i class="fas fa-newspaper me-2"></i>新闻管理</h4>
                        <asp:Button ID="btnAddNews" runat="server" Text="添加新闻" 
                            CssClass="btn btn-light" OnClick="btnAddNews_Click" />
                    </div>
                </div>
                <div class="card-body">
                    <!-- 消息提示 -->
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert" Visible="false"></asp:Label>
                        
                    <!-- 新闻列表 -->
                    <div class="table-responsive">
                        <asp:GridView ID="gvNews" runat="server" AutoGenerateColumns="False" 
                            DataKeyNames="NewsID" CssClass="table table-striped table-hover" 
                            OnRowCommand="gvNews_RowCommand" OnRowDataBound="gvNews_RowDataBound"
                            AllowPaging="True" PageSize="10" OnPageIndexChanging="gvNews_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="NewsID" HeaderText="ID" ItemStyle-Width="60px" />
                                <asp:TemplateField HeaderText="标题">
                                    <ItemTemplate>
                                        <a href='../NewsDetail.aspx?id=<%# Eval("NewsID") %>' target="_blank">
                                            <%# TruncateTitle(Eval("Title").ToString(), 30) %>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Category" HeaderText="分类" />
                                <asp:BoundField DataField="PublishDate" HeaderText="发布时间" 
                                    DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="Views" HeaderText="浏览数" ItemStyle-Width="80px" />
                                <asp:TemplateField HeaderText="状态" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <span class='badge <%# Convert.ToBoolean(Eval("IsActive")) ? "bg-success" : "bg-secondary" %>'>
                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "发布" : "草稿" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlEdit" runat="server" 
                                            NavigateUrl='<%# "EditNews.aspx?id=" + Eval("NewsID") %>'
                                            CssClass="btn btn-sm btn-primary" Text="编辑" />
                                        <asp:LinkButton ID="btnDelete" runat="server" 
                                            CommandName="DeleteNews" 
                                            CommandArgument='<%# Eval("NewsID") %>'
                                            CssClass="btn btn-sm btn-danger" Text="删除" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-info text-center py-4">
                                    <i class="fas fa-info-circle fa-2x mb-3"></i>
                                    <h4>暂无新闻数据</h4>
                                    <p>点击"添加新闻"按钮发布第一篇新闻</p>
                                </div>
                            </EmptyDataTemplate>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="pagination" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>