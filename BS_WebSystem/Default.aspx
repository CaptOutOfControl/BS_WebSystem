<%@ Page Title="首页" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- 轮播图 -->
        <div class="col-12 mb-4">
            <div class="card border-0 shadow-lg animate-on-load">
                <div class="card-body p-0">
            <div id="newsCarousel" class="carousel slide" data-bs-ride="carousel">
                <div class="carousel-indicators">
                    <button type="button" data-bs-target="#newsCarousel" data-bs-slide-to="0" class="active"></button>
                    <button type="button" data-bs-target="#newsCarousel" data-bs-slide-to="1"></button>
                    <button type="button" data-bs-target="#newsCarousel" data-bs-slide-to="2"></button>
                </div>
                <div class="carousel-inner rounded-3">
                    <asp:Repeater ID="rptTopNews" runat="server">
                        <ItemTemplate>
                            <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                       <img src='<%# GetImageUrl(Eval("ImageUrl")) %>' 
                         class="img-fluid h-100 w-100" 
                         alt='<%# Eval("Title") %>'
                         style="object-fit:cover;"
                         onerror="this.src='Images/default.jpg';" />
                                <div class="carousel-caption d-none d-md-block">
                                    <h5><%# Eval("Title") %></h5>
                                    <p><%# Eval("PublishDate", "{0:yyyy-MM-dd}") %></p>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <button class="carousel-control-prev" type="button" data-bs-target="#newsCarousel" data-bs-slide="prev">
                    <span class="carousel-control-prev-icon"></span>
                </button>
                <button class="carousel-control-next" type="button" data-bs-target="#newsCarousel" data-bs-slide="next">
                    <span class="carousel-control-next-icon"></span>
                </button>
        </div>
     </div>
            </div>
        </div>
    </div>

    <div class="row">
        <!-- 新闻列表 -->
        <div class="col-lg-8">
            <div class="card shadow-lg mb-4 animate-on-load delay-1">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0"><i class="fas fa-newspaper me-2"></i>最新新闻</h4>
                </div>
                <div class="card-body">
                    <asp:ListView ID="lvNews" runat="server" GroupPlaceholderID="groupPlaceHolder" 
                        ItemPlaceholderID="itemPlaceHolder" GroupItemCount="1">
                        <LayoutTemplate>
                            <div>
                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                                
                                <!-- 分页 -->
                                <div class="d-flex justify-content-center mt-4 animate-on-scroll">
                                    <asp:DataPager ID="dpNews" runat="server" PagedControlID="lvNews" PageSize="5">
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
                            <div class="card mb-3 border-0 shadow-hover animate-on-scroll">
                                <div class="row g-0">
                                    <div class="col-md-4">
                                                        <div class="position-relative" style="height:200px;overflow:hidden;">
                    <!-- 直接使用ImageUrl字段 -->
                    <img src='<%# GetImageUrl(Eval("ImageUrl")) %>' 
                         class="img-fluid h-100 w-100" 
                         alt='<%# Eval("Title") %>'
                         style="object-fit:cover;"
                         onerror="this.src='Images/default.jpg';" />
                </div>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="card-body">
                                            <h5 class="card-title">
                                                <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="text-decoration-none">
                                                    <%# Eval("Title") %>
                                                </a>
                                            </h5>
                                            <p class="card-text text-muted small">
                                                <i class="far fa-calendar-alt me-1"></i><%# Eval("PublishDate", "{0:yyyy-MM-dd HH:mm}") %>
                                                <span class="ms-3"><i class="far fa-folder me-1"></i><%# Eval("Category") %></span>
                                                <span class="ms-3"><i class="far fa-eye me-1"></i><%# Eval("Views") %></span>
                                            </p>
                                            <p class="card-text"><%# TruncateContent(Eval("Content").ToString(), 100) %></p>
                                            <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="btn btn-primary btn-sm">
                                                <i class="fas fa-book-reader me-1"></i>阅读全文
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center py-4 animate-on-scroll">
                                <i class="fas fa-info-circle me-2"></i>暂无新闻
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
        
        <!-- 侧边栏 -->
        <div class="col-lg-4">
            <!-- 热门新闻 -->
            <div class="card shadow-lg mb-4 animate-on-load delay-2">
                <div class="card-header bg-danger text-white">
                    <h5 class="mb-0"><i class="fas fa-fire me-2"></i>热门新闻</h5>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="rptHotNews" runat="server">
                        <ItemTemplate>
                            <div class="mb-3 pb-2 border-bottom animate-on-scroll">
                                <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="text-decoration-none">
                                    <h6 class="mb-1"><%# Eval("Title") %></h6>
                                </a>
                                <div class="text-muted small">
                                    <i class="far fa-calendar-alt me-1"></i><%# Eval("PublishDate", "{0:MM-dd}") %>
                                    <span class="ms-3"><i class="far fa-eye me-1"></i><%# Eval("Views") %></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            
            <!-- 新闻分类 -->
            <div class="card shadow-lg mb-4 animate-on-load delay-3">
                <div class="card-header bg-info text-white">
                    <h5 class="mb-0"><i class="fas fa-tags me-2"></i>新闻分类</h5>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="rptCategories" runat="server">
                        <ItemTemplate>
                            <a href="NewsList.aspx?category=<%# Server.UrlEncode(Eval("Category").ToString()) %>" 
                               class="badge bg-primary text-decoration-none me-2 mb-2 p-2 animate-on-scroll">
                                <%# Eval("Category") %>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            
            <!-- 网站信息 -->
            <div class="card shadow-lg animate-on-load delay-4">
                <div class="card-header">
                    <h5 class="mb-0"><i class="fas fa-info-circle me-2"></i>网站信息</h5>
                </div>
                <div class="card-body">
                    <p>本站是一个新闻发布系统，提供最新的新闻资讯。</p>
                    <p>管理员可以登录后台发布新闻，普通用户可以浏览和搜索新闻。</p>
                    <div class="text-center mt-3">
                        <i class="fas fa-newspaper fa-3x text-primary"></i>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>