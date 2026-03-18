<%@ Page Title="数据统计" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Admin_Statistics" Codebehind="Statistics.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        <div class="row mb-4">
            <div class="col-12">
                <div class="card">
                    <div class="card-header bg-info text-white">
                        <h4 class="mb-0"><i class="fas fa-chart-bar me-2"></i>数据统计中心</h4>
                    </div>
                    <div class="card-body">
                        <!-- 统计时间段选择 -->
                        <div class="row mb-4">
                            <div class="col-md-6">
                                <label class="form-label">选择统计时间段：</label>
                                <asp:DropDownList ID="ddlTimeRange" runat="server" CssClass="form-select" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTimeRange_SelectedIndexChanged">
                                    <asp:ListItem Value="today">今日</asp:ListItem>
                                    <asp:ListItem Value="yesterday">昨日</asp:ListItem>
                                    <asp:ListItem Value="week" Selected="True">本周</asp:ListItem>
                                    <asp:ListItem Value="month">本月</asp:ListItem>
                                    <asp:ListItem Value="year">今年</asp:ListItem>
                                    <asp:ListItem Value="all">全部时间</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">自定义时间段：</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" 
                                        TextMode="Date" placeholder="开始日期"></asp:TextBox>
                                    <span class="input-group-text">至</span>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" 
                                        TextMode="Date" placeholder="结束日期"></asp:TextBox>
                                    <asp:Button ID="btnCustomRange" runat="server" Text="查询" 
                                        CssClass="btn btn-primary" OnClick="btnCustomRange_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 总体概览卡片 -->
        <div class="row mb-4">
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-start border-primary border-4 shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs fw-bold text-primary text-uppercase mb-1">
                                    新闻总数</div>
                                <div class="h5 mb-0 fw-bold text-gray-800">
                                    <asp:Label ID="lblTotalNews" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="mt-2">
                                    <span class="badge bg-success">
                                        <i class="fas fa-arrow-up me-1"></i>
                                        今日新增: <asp:Label ID="lblTodayNews" runat="server" Text="0"></asp:Label>
                                    </span>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-newspaper fa-2x text-primary"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-start border-success border-4 shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs fw-bold text-success text-uppercase mb-1">
                                    用户总数</div>
                                <div class="h5 mb-0 fw-bold text-gray-800">
                                    <asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="mt-2">
                                    <span class="badge bg-info">
                                        <i class="fas fa-user-shield me-1"></i>
                                        管理员: <asp:Label ID="lblAdminCount" runat="server" Text="0"></asp:Label>
                                    </span>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-users fa-2x text-success"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-start border-warning border-4 shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs fw-bold text-warning text-uppercase mb-1">
                                    总浏览数</div>
                                <div class="h5 mb-0 fw-bold text-gray-800">
                                    <asp:Label ID="lblTotalViews" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="mt-2">
                                    <span class="badge bg-warning">
                                        平均浏览: <asp:Label ID="lblAvgViews" runat="server" Text="0"></asp:Label>
                                    </span>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-eye fa-2x text-warning"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-start border-danger border-4 shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs fw-bold text-danger text-uppercase mb-1">
                                    活跃度</div>
                                <div class="h5 mb-0 fw-bold text-gray-800">
                                    <asp:Label ID="lblActivityRate" runat="server" Text="0%"></asp:Label>
                                </div>
                                <div class="mt-2">
                                    <span class="badge bg-danger">
                                        更新频率: <asp:Label ID="lblUpdateFreq" runat="server" Text="0天/篇"></asp:Label>
                                    </span>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-chart-line fa-2x text-danger"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 详细统计 -->
        <div class="row">
            <!-- 新闻分类统计 -->
            <div class="col-lg-6 mb-4">
                <div class="card shadow">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0"><i class="fas fa-tags me-2"></i>新闻分类统计</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvCategoryStats" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered" ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:BoundField DataField="Category" HeaderText="分类" />
                                    <asp:BoundField DataField="NewsCount" HeaderText="新闻数" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="TotalViews" HeaderText="总浏览数" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="AvgViews" HeaderText="平均浏览" ItemStyle-Width="100px" DataFormatString="{0:F1}" />
                                    <asp:BoundField DataField="Percentage" HeaderText="占比" ItemStyle-Width="80px" DataFormatString="{0:P0}" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="text-center text-muted py-3">暂无分类数据</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 作者发布统计 -->
            <div class="col-lg-6 mb-4">
                <div class="card shadow">
                    <div class="card-header bg-success text-white">
                        <h5 class="mb-0"><i class="fas fa-user-edit me-2"></i>作者发布统计</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAuthorStats" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered" ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:BoundField DataField="AuthorName" HeaderText="作者" />
                                    <asp:BoundField DataField="NewsCount" HeaderText="发布数" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="TotalViews" HeaderText="总浏览" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="AvgViews" HeaderText="平均浏览" ItemStyle-Width="100px" DataFormatString="{0:F1}" />
                                    <asp:BoundField DataField="LastPublish" HeaderText="最后发布" ItemStyle-Width="120px" DataFormatString="{0:yyyy-MM-dd}" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="text-center text-muted py-3">暂无作者数据</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 热门新闻排行 -->
            <div class="col-lg-12 mb-4">
                <div class="card shadow">
                    <div class="card-header bg-warning">
                        <h5 class="mb-0"><i class="fas fa-fire me-2"></i>热门新闻排行榜</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvHotNews" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-striped" ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="排名" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <span class="badge <%# GetRankBadgeClass(Container.DataItemIndex) %>">
                                                <%# Container.DataItemIndex + 1 %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="新闻标题">
                                        <ItemTemplate>
                                            <a href='../NewsDetail.aspx?id=<%# Eval("NewsID") %>' target="_blank" class="text-decoration-none">
                                                <%# Eval("Title") %>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Category" HeaderText="分类" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="AuthorName" HeaderText="作者" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="PublishDate" HeaderText="发布时间" ItemStyle-Width="120px" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="Views" HeaderText="浏览数" ItemStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="趋势" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <%# GetTrendIcon(Eval("ViewsTrend")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="text-center text-muted py-3">暂无热门新闻数据</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 时间趋势统计 -->
            <div class="col-lg-12 mb-4">
                <div class="card shadow">
                    <div class="card-header bg-info text-white">
                        <h5 class="mb-0"><i class="fas fa-chart-line me-2"></i>新闻发布趋势</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvDailyStats" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-bordered" ShowHeaderWhenEmpty="True">
                                        <Columns>
                                            <asp:BoundField DataField="Date" HeaderText="日期" DataFormatString="{0:yyyy-MM-dd}" />
                                            <asp:BoundField DataField="NewsCount" HeaderText="发布数" />
                                            <asp:BoundField DataField="TotalViews" HeaderText="浏览总数" />
                                            <asp:BoundField DataField="AvgViews" HeaderText="平均浏览" DataFormatString="{0:F1}" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="text-center text-muted py-3">暂无趋势数据</div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card">
                                    <div class="card-body">
                                        <h6 class="card-title">统计说明：</h6>
                                        <ul class="list-unstyled">
                                            <li><i class="fas fa-circle text-primary me-2"></i>蓝色卡片：新闻相关统计</li>
                                            <li><i class="fas fa-circle text-success me-2"></i>绿色卡片：用户相关统计</li>
                                            <li><i class="fas fa-circle text-warning me-2"></i>黄色卡片：浏览相关统计</li>
                                            <li><i class="fas fa-circle text-danger me-2"></i>红色卡片：活跃度统计</li>
                                            <li class="mt-3">
                                                <small class="text-muted">
                                                    <i class="fas fa-info-circle me-1"></i>
                                                    数据更新时间：<asp:Label ID="lblUpdateTime" runat="server" Text=""></asp:Label>
                                                </small>
                                            </li>
                                        </ul>
                                        <div class="mt-3">
                                            <asp:Button ID="btnExport" runat="server" Text="导出统计报表" 
                                                CssClass="btn btn-outline-primary" OnClick="btnExport_Click" />
                                            <asp:Button ID="btnRefresh" runat="server" Text="刷新数据" 
                                                CssClass="btn btn-outline-success ms-2" OnClick="btnRefresh_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- 图表区域 -->
    <div class="row mt-4">
        <div class="col-lg-6">
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0"><i class="fas fa-chart-pie me-2"></i>新闻分类分布</h5>
                </div>
                <div class="card-body">
                    <canvas id="categoryChart" height="300"></canvas>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="card shadow">
                <div class="card-header bg-success text-white">
                    <h5 class="mb-0"><i class="fas fa-chart-line me-2"></i>浏览趋势</h5>
                </div>
                <div class="card-body">
                    <canvas id="viewsChart" height="300"></canvas>
                </div>
            </div>
        </div>
    </div>

    <!-- 添加图表库 -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script type="text/javascript">
    // 页面加载完成后初始化图表
    document.addEventListener('DOMContentLoaded', function() {
        initializeCharts();
    });
    
    function initializeCharts() {
        // 分类分布饼图
        var categoryCtx = document.getElementById('categoryChart').getContext('2d');
        var categoryChart = new Chart(categoryCtx, {
            type: 'pie',
            data: {
                labels: ['政治', '经济', '科技', '体育', '娱乐', '其他'],
                datasets: [{
                    data: [12, 19, 8, 15, 10, 7],
                    backgroundColor: [
                        '#FF6384',
                        '#36A2EB',
                        '#FFCE56',
                        '#4BC0C0',
                        '#9966FF',
                        '#FF9F40'
                    ],
                    hoverOffset: 4
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'bottom'
                    },
                    title: {
                        display: true,
                        text: '新闻分类分布图'
                    }
                }
            }
        });
        
        // 浏览趋势折线图
        var viewsCtx = document.getElementById('viewsChart').getContext('2d');
        var viewsChart = new Chart(viewsCtx, {
            type: 'line',
            data: {
                labels: ['1月', '2月', '3月', '4月', '5月', '6月'],
                datasets: [{
                    label: '浏览数',
                    data: [1200, 1900, 800, 1500, 1000, 1700],
                    borderColor: '#36A2EB',
                    backgroundColor: 'rgba(54, 162, 235, 0.1)',
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4
                }, {
                    label: '发布数',
                    data: [12, 19, 8, 15, 10, 17],
                    borderColor: '#FF6384',
                    backgroundColor: 'rgba(255, 99, 132, 0.1)',
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: '月度浏览和发布趋势'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
    
    // 重新加载图表数据（在后台数据更新后调用）
    function reloadCharts() {
        // 这里可以添加AJAX调用获取实际数据
        console.log('重新加载图表数据');
    }
    </script>

    <style>
        .card {
            border: none;
            border-radius: 10px;
            box-shadow: 0 0.15rem 1.75rem 0 rgba(58, 59, 69, 0.15);
        }
        
        .border-start {
            border-left-width: 0.25rem !important;
        }
        
        .text-xs {
            font-size: 0.7rem;
        }
        
        .text-gray-800 {
            color: #5a5c69;
        }
        
        .badge-rank-1 {
            background: linear-gradient(135deg, #ffd700 0%, #ffa500 100%);
            color: #000;
        }
        
        .badge-rank-2 {
            background: linear-gradient(135deg, #c0c0c0 0%, #a9a9a9 100%);
            color: #000;
        }
        
        .badge-rank-3 {
            background: linear-gradient(135deg, #cd7f32 0%, #a0522d 100%);
            color: #fff;
        }
        
        .badge-rank-other {
            background: #6c757d;
            color: #fff;
        }
        
        .table th {
            font-weight: 600;
            background-color: #f8f9fc;
            border-bottom: 2px solid #e3e6f0;
        }
    </style>
</asp:Content>