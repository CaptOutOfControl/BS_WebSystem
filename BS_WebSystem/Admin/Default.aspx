<%@ Page Title="后台管理" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Admin_Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h3 class="mb-0"><i class="fas fa-cog me-2"></i>后台管理系统</h3>
                    </div>
                    <div class="card-body">
                        <!-- 欢迎信息 -->
                        <div class="text-center mb-5">
                            <i class="fas fa-user-cog fa-4x text-primary mb-3"></i>
                            <h2>欢迎进入后台管理系统</h2>
                            <p class="text-muted">请选择要管理的功能</p>
                        </div>
                        
                        <!-- 功能卡片 -->
                        <div class="row">
                            <div class="col-md-4 mb-4">
                                <div class="card text-center h-100 border-primary">
                                    <div class="card-body">
                                        <i class="fas fa-newspaper fa-3x text-primary mb-3"></i>
                                        <h5 class="card-title">新闻管理</h5>
                                        <p class="card-text">发布、编辑、删除新闻</p>
                                        <a href="NewsManage.aspx" class="btn btn-primary">进入管理</a>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-md-4 mb-4">
                                <div class="card text-center h-100 border-success">
                                    <div class="card-body">
                                        <i class="fas fa-plus-circle fa-3x text-success mb-3"></i>
                                        <h5 class="card-title">添加新闻</h5>
                                        <p class="card-text">发布新的新闻内容</p>
                                        <a href="AddNews.aspx" class="btn btn-success">添加新闻</a>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-md-4 mb-4">
                                <div class="card text-center h-100 border-info">
                                    <div class="card-body">
                                        <i class="fas fa-chart-bar fa-3x text-info mb-3"></i>
                                        <h5 class="card-title">数据统计</h5>
                                        <p class="card-text">查看网站详细数据统计</p>
                                        <a href="Statistics.aspx" class="btn btn-info">查看统计</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- 系统信息 -->
                        <div class="card mt-4">
                            <div class="card-header">
                                <h5 class="mb-0"><i class="fas fa-info-circle me-2"></i>系统信息</h5>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <p><strong>登录用户：</strong><asp:LoginName ID="LoginName1" runat="server" /></p>
                                        <p><strong>登录时间：</strong><%= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %></p>
                                    </div>
                                    <div class="col-md-6">
                                        <p><strong>系统版本：</strong>v1.0.0</p>
                                        <p><strong>最后登录IP：</strong><%= Request.UserHostAddress %></p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- 返回前台 -->
                        <div class="text-center mt-4">
                            <a href="../Default.aspx" class="btn btn-outline-secondary">
                                <i class="fas fa-home me-2"></i>返回前台
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>