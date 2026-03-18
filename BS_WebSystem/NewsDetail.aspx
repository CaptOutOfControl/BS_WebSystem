<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="NewsDetail" Codebehind="NewsDetail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- 面包屑导航 -->
        <div class="col-12 mb-3">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="Default.aspx">首页</a></li>
                    <li class="breadcrumb-item"><a href="NewsList.aspx?category=<%= Server.UrlEncode(ViewState["CurrentCategory"]?.ToString() ?? "新闻") %>">
                        <%= ViewState["CurrentCategory"]?.ToString() ?? "新闻" %></a></li>
                    <li class="breadcrumb-item active" aria-current="page">新闻详情</li>
                </ol>
            </nav>
        </div>
    </div>
        
    <div class="row">
        <!-- 左侧主要内容 -->
        <div class="col-lg-8">
            <div class="card">
                <div class="card-body">
                    <!-- 新闻标题 -->
                    <h1 class="card-title mb-3" id="newsTitle" runat="server"></h1>
                        
                    <!-- 新闻元信息 -->
                    <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 text-muted border-bottom pb-3">
                        <div>
                            <span class="me-3" id="authorInfo" runat="server"></span>
                            <span class="me-3" id="publishDate" runat="server"></span>
                            <span class="me-3" id="categoryInfo" runat="server"></span>
                            <span id="viewCount" runat="server"></span>
                        </div>
                        <div class="mt-2 mt-md-0">
                            <button class="btn btn-outline-primary btn-sm" onclick="window.print()">
                                <i class="fas fa-print me-1"></i>打印
                            </button>
                        </div>
                    </div>
                        
                    <!-- 新闻图片 -->
                    <div class="text-center mb-4">
                        <asp:Image ID="imgNews" runat="server" CssClass="img-fluid rounded" 
                            Style="max-height: 500px; object-fit: contain;" Visible="false" />
                    </div>
                        
                    <!-- 新闻内容 -->
                    <div class="news-content mb-5" id="newsContent" runat="server">
                    </div>
                        
                    <!-- 上一篇/下一篇导航 -->
                    <div class="row mb-4">
                        <div class="col-md-6">
                            <div class="card">
                                <div class="card-body">
                                    <small class="text-muted">上一篇</small>
                                    <asp:HyperLink ID="lnkPrev" runat="server" CssClass="d-block text-truncate fw-bold">
                                        没有了
                                    </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="card">
                                <div class="card-body text-end">
                                    <small class="text-muted">下一篇</small>
                                    <asp:HyperLink ID="lnkNext" runat="server" CssClass="d-block text-truncate fw-bold">
                                        没有了
                                    </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
            
        <!-- 右侧侧边栏 -->
        <div class="col-lg-4">
            <!-- 相关新闻 -->
        <div class="card mb-4">
            <div class="card-header bg-info text-white">
                <h5 class="mb-0"><i class="fas fa-newspaper me-2"></i>相关新闻</h5>
            </div>
            <div class="card-body">
                <asp:PlaceHolder ID="phRelatedNewsEmpty" runat="server" Visible="false">
                    <div class="text-center text-muted py-3">
                        <i class="fas fa-info-circle me-1"></i>暂无相关新闻
                    </div>
                </asp:PlaceHolder>
        
                <asp:Repeater ID="rptRelatedNews" runat="server">
                    <ItemTemplate>
                        <div class="mb-3 pb-2 border-bottom">
                            <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="text-decoration-none">
                                <h6 class="mb-1"><%# Eval("Title") %></h6>
                            </a>
                            <div class="text-muted small">
                                <i class="far fa-calendar-alt me-1"></i>
                                <%# Eval("PublishDate", "{0:yyyy-MM-dd}") %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <!-- 热门新闻 -->
        <div class="card mb-4">
            <div class="card-header bg-danger text-white">
                <h5 class="mb-0"><i class="fas fa-fire me-2"></i>热门新闻</h5>
            </div>
            <div class="card-body">
                <asp:PlaceHolder ID="phHotNewsEmpty" runat="server" Visible="false">
                    <div class="text-center text-muted py-3">
                        <i class="fas fa-info-circle me-1"></i>暂无热门新闻
                    </div>
                </asp:PlaceHolder>
        
                <asp:Repeater ID="rptHotNewsSide" runat="server">
                    <ItemTemplate>
                        <div class="mb-3">
                            <a href="NewsDetail.aspx?id=<%# Eval("NewsID") %>" class="text-decoration-none">
                                <h6 class="mb-1 text-truncate"><%# Eval("Title") %></h6>
                            </a>
                            <div class="text-muted small">
                                <span class="me-3"><i class="far fa-calendar-alt"></i> <%# Eval("PublishDate", "{0:MM-dd}") %></span>
                                <span><i class="far fa-eye"></i> <%# Eval("Views") %></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
                
            <!-- 返回按钮 -->
            <div class="card">
                <div class="card-body text-center">
                    <a href="Default.aspx" class="btn btn-outline-primary btn-lg w-100">
                        <i class="fas fa-home me-2"></i>返回首页
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>