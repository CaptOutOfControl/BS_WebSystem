<%@ Page Title="分类新闻" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="NewsList" Codebehind="NewsList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <!-- 页面标题 -->
                <div class="card mb-4">
                    <div class="card-body text-center">
                        <h2 class="mb-0">
                            <i class="fas fa-folder-open me-2"></i>
                            <asp:Label ID="lblCategory" runat="server" Text="新闻列表"></asp:Label>
                        </h2>
                    </div>
                </div>
                
                <!-- 新闻列表 -->
                <asp:Label ID="lblNoData" runat="server" Text="" CssClass="alert alert-warning" 
                    Visible="false"></asp:Label>
                
                <asp:ListView ID="lvCategoryNews" runat="server" GroupPlaceholderID="groupPlaceHolder" 
                    ItemPlaceholderID="itemPlaceHolder" GroupItemCount="1">
                    <LayoutTemplate>
                        <div>
                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                            
                            <div class="d-flex justify-content-center mt-4">
                                <asp:DataPager ID="dpCategoryNews" runat="server" PagedControlID="lvCategoryNews" PageSize="10">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                            ShowNextPageButton="False" ShowPreviousPageButton="True" 
                                            ButtonCssClass="btn btn-outline-primary me-2" />
                                        <asp:NumericPagerField ButtonType="Button" 
                                            NumericButtonCssClass="btn btn-outline-primary me-2" 
                                            CurrentPageLabelCssClass="btn btn-primary me-2" />
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" 
                                            ShowNextPageButton="True" ShowPreviousPageButton="False" 
                                            ButtonCssClass="btn btn-outline-primary" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </div>
                    </LayoutTemplate>
                    
                    <GroupTemplate>
                        <div>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </div>
                    </GroupTemplate>
                    
                    <ItemTemplate>
                        <div class="card mb-3">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="text-decoration-none">
                                        <%# Eval("Title") %>
                                    </a>
                                </h5>
                                <p class="card-text text-muted small">
                                    <i class="far fa-calendar-alt me-1"></i><%# Eval("PublishDate", "{0:yyyy-MM-dd HH:mm}") %>
                                    <span class="ms-3"><i class="far fa-eye me-1"></i><%# Eval("Views") %></span>
                                    <span class="ms-3"><i class="far fa-user me-1"></i><%# Eval("Username") %></span>
                                </p>
                                <p class="card-text"><%# TruncateContent(Eval("Content").ToString(), 200) %></p>
                                <div class="d-flex justify-content-between align-items-center">
                                    <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="btn btn-outline-primary btn-sm">查看详情</a>
                                    <span class="badge bg-info"><%# Eval("Category") %></span>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    
                    <EmptyDataTemplate>
                        <div class="alert alert-info text-center py-4">
                            <i class="fas fa-newspaper fa-2x mb-3"></i>
                            <h4>暂无新闻</h4>
                            <p>该分类下暂时没有新闻</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:ListView>
                
                <!-- 返回按钮 -->
                <div class="text-center mt-4">
                    <a href="Default.aspx" class="btn btn-outline-secondary">
                        <i class="fas fa-arrow-left me-2"></i>返回首页
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>