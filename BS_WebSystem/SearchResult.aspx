<%@ Page Title="搜索结果" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SearchResult" Codebehind="SearchResult.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <!-- 搜索框 -->
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="row g-3 align-items-center">
                            <div class="col-md-10">
                                <div class="input-group">
                                    <span class="input-group-text"><i class="fas fa-search"></i></span>
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                                        placeholder="请输入搜索关键词..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSearch" runat="server" Text="搜索" 
                                    CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                
                <!-- 搜索结果标题 -->
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <h2>
                        <asp:Label ID="lblSearchTerm" runat="server" Text="搜索结果"></asp:Label>
                    </h2>
                    <div class="text-muted">
                        <asp:Label ID="lblResultCount" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                
                <!-- 没有结果的提示 -->
                <asp:Panel ID="pnlNoResults" runat="server" Visible="false" CssClass="alert alert-info">
                    <i class="fas fa-info-circle me-2"></i>没有找到相关新闻，请尝试其他关键词。
                </asp:Panel>
                
                <!-- 搜索结果列表 -->
                <asp:ListView ID="lvSearchResults" runat="server" GroupPlaceholderID="groupPlaceHolder" 
                    ItemPlaceholderID="itemPlaceHolder" GroupItemCount="1">
                    <LayoutTemplate>
                        <div>
                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                            
                            <!-- 分页 -->
                            <div class="d-flex justify-content-center mt-4">
                                <asp:DataPager ID="dpSearchResults" runat="server" PagedControlID="lvSearchResults" PageSize="10">
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
                            <div class="row g-0">
                                <div class="col-md-3">
                                    <img src='<%# Eval("ImageUrl") %>' class="img-fluid rounded-start" 
                                         alt='<%# Eval("Title") %>' style="height: 180px; object-fit: cover;">
                                </div>
                                <div class="col-md-9">
                                    <div class="card-body">
                                        <h5 class="card-title">
                                            <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="text-decoration-none">
                                                <%# Eval("Title") %>
                                            </a>
                                        </h5>
                                        <p class="card-text text-muted small">
                                            <i class="far fa-calendar-alt me-1"></i>
                                            <%# Eval("PublishDate", "{0:yyyy-MM-dd HH:mm}") %>
                                            <span class="ms-3"><i class="far fa-folder me-1"></i>
                                            <%# Eval("Category") %></span>
                                            <span class="ms-3"><i class="far fa-eye me-1"></i>
                                            <%# Eval("Views") %></span>
                                        </p>
                                        <p class="card-text">
                                            <%# TruncateContent(Eval("Content").ToString(), 150) %>
                                        </p>
                                        <div class="d-flex justify-content-between align-items-center">
                                            <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="btn btn-primary btn-sm">
                                                查看详情
                                            </a>
                                            <small class="text-muted">
                                                <i class="far fa-clock"></i> <%# GetShortTime(Eval("PublishDate")) %>
                                            </small>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    
                    <EmptyDataTemplate>
                        <div class="alert alert-warning text-center py-4">
                            <i class="fas fa-search fa-2x mb-3"></i>
                            <h4>没有找到相关新闻</h4>
                            <p>请尝试其他搜索关键词</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:ListView>
                
                <!-- 返回首页 -->
                <div class="text-center mt-4">
                    <a href="Default.aspx" class="btn btn-outline-secondary">
                        <i class="fas fa-home me-2"></i>返回首页
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>