<%@ Page Title="关于我们" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="About" Codebehind="About.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- 页面标题 -->
        <div class="row mb-5">
            <div class="col-12 text-center">
                <h1 class="display-4 fw-bold text-primary mb-3">关于我们</h1>
                <p class="lead text-muted">了解新闻发布系统的背景、使命和团队</p>
                <div class="d-flex justify-content-center">
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="Default.aspx">首页</a></li>
                            <li class="breadcrumb-item active" aria-current="page">关于我们</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
     </div>

        <!-- 网站介绍 -->
        <div class="row mb-5">
            <div class="col-lg-6 mb-4 mb-lg-0">
                <div class="card border-0 shadow-lg h-100">
                    <div class="card-body p-5">
                        <div class="text-center mb-4">
                            <i class="fas fa-newspaper fa-3x text-primary mb-3"></i>
                            <h3 class="card-title">新闻发布系统介绍</h3>
                        </div>
                        <p class="card-text">
                            新闻发布系统是一个基于ASP.NET Web Forms开发的现代化新闻管理平台，旨在为用户提供最新、最全面的新闻资讯。
                        </p>
                        <p class="card-text">
                            本系统采用B/S架构，前端使用Bootstrap 5和Font Awesome图标库，后端使用C#语言和SQL Server数据库，实现了新闻的发布、浏览、搜索和管理等功能。
                        </p>
                        <p class="card-text">
                            系统设计充分考虑了用户体验和操作便捷性，界面美观大方，功能完善，适合作为课程设计项目或小型新闻网站使用。
                        </p>
                        
                        <div class="mt-4">
                            <h5 class="mb-3"><i class="fas fa-bullseye me-2"></i>我们的使命</h5>
                            <ul class="list-unstyled">
                                <li class="mb-2"><i class="fas fa-check-circle text-success me-2"></i>提供高质量的新闻内容</li>
                                <li class="mb-2"><i class="fas fa-check-circle text-success me-2"></i>建立用户友好的新闻阅读体验</li>
                                <li class="mb-2"><i class="fas fa-check-circle text-success me-2"></i>支持多样化的新闻分类和搜索</li>
                                <li class="mb-2"><i class="fas fa-check-circle text-success me-2"></i>确保系统稳定性和安全性</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-lg-6">
                <div class="card border-0 shadow-lg h-100">
                    <div class="card-body p-5">
                        <div class="text-center mb-4">
                            <i class="fas fa-cogs fa-3x text-info mb-3"></i>
                            <h3 class="card-title">主要功能特性</h3>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-6 mb-4">
                                <div class="text-center p-3 bg-light rounded-3 h-100">
                                    <i class="fas fa-eye fa-2x text-primary mb-3"></i>
                                    <h5>新闻浏览</h5>
                                    <p class="small">分类浏览、搜索、热门推荐</p>
                                </div>
                            </div>
                            <div class="col-md-6 mb-4">
                                <div class="text-center p-3 bg-light rounded-3 h-100">
                                    <i class="fas fa-edit fa-2x text-success mb-3"></i>
                                    <h5>新闻管理</h5>
                                    <p class="small">发布、编辑、删除新闻</p>
                                </div>
                            </div>
                            <div class="col-md-6 mb-4">
                                <div class="text-center p-3 bg-light rounded-3 h-100">
                                    <i class="fas fa-user-shield fa-2x text-warning mb-3"></i>
                                    <h5>权限管理</h5>
                                    <p class="small">用户注册、登录、角色控制</p>
                                </div>
                            </div>
                            <div class="col-md-6 mb-4">
                                <div class="text-center p-3 bg-light rounded-3 h-100">
                                    <i class="fas fa-mobile-alt fa-2x text-danger mb-3"></i>
                                    <h5>响应式设计</h5>
                                    <p class="small">适配各种设备屏幕</p>
                                </div>
                            </div>
                        </div>
                        
                        <div class="mt-4">
                            <h5 class="mb-3"><i class="fas fa-chart-line me-2"></i>技术栈</h5>
                            <div class="d-flex flex-wrap gap-2">
                                <span class="badge bg-primary p-2">ASP.NET Web Forms</span>
                                <span class="badge bg-secondary p-2">C#</span>
                                <span class="badge bg-success p-2">SQL Server</span>
                                <span class="badge bg-info p-2">Bootstrap 5</span>
                                <span class="badge bg-warning p-2">JavaScript</span>
                                <span class="badge bg-danger p-2">Font Awesome</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 开发团队 -->
        <div class="row mb-5">
            <div class="col-12">
                <div class="card border-0 shadow-lg">
                    <div class="card-body p-5">
                        <div class="text-center mb-5">
                            <h2 class="fw-bold"><i class="fas fa-users me-2"></i>开发团队</h2>
                            <p class="text-muted">我们的团队成员</p>
                        </div>
                        
                        <div class="row">
                            <!-- 团队成员1 -->
                            <div class="col-md-4 mb-4">
                                <div class="card border-0 shadow-sm h-100 text-center">
                                    <div class="card-body p-4">
                                        <div class="team-img mb-3">
                                            <div class="rounded-circle bg-primary d-inline-flex align-items-center justify-content-center" 
                                                 style="width: 120px; height: 120px; font-size: 3rem; color: white;">
                                                <i class="fas fa-user"></i>
                                            </div>
                                        </div>
                                        <h4 class="card-title">孙嘉赓</h4>
                                        <p class="text-primary mb-2">项目经理 & 全栈开发</p>
                                        <p class="card-text small">负责系统架构设计和后端开发、用户界面设计和前端交互开发、数据库设计等</p>
                                        <div class="mt-3">
                                            <a href="mailto:zhangsan@newssystem.com" class="text-decoration-none me-3">
                                                <i class="fas fa-envelope text-primary"></i>
                                            </a>
                                            <span class="text-muted">m13720071615@163.com</span>
                                        </div>
                                    </div>
                                </div>
                            </div>     
                    </div>
                </div>
            </div>
        </div>

        <!-- 项目时间线 -->
        <div class="row mb-5">
            <div class="col-12">
                <div class="card border-0 shadow-lg">
                    <div class="card-body p-5">
                        <div class="text-center mb-5">
                            <h2 class="fw-bold"><i class="fas fa-history me-2"></i>项目发展历程</h2>
                            <p class="text-muted">从构思到实现的时间线</p>
                        </div>
                        
                        <div class="timeline">
                            <div class="timeline-item">
                                <div class="timeline-date">2025年10月</div>
                                <div class="timeline-content">
                                    <h5>项目启动</h5>
                                    <p>需求分析和项目规划，确定技术栈和功能模块。</p>
                                </div>
                            </div>
                            <div class="timeline-item">
                                <div class="timeline-date">2025年11月</div>
                                <div class="timeline-content">
                                    <h5>数据库设计</h5>
                                    <p>设计数据库表结构，创建Users表和News表。</p>
                                </div>
                            </div>
                            <div class="timeline-item">
                                <div class="timeline-date">2025年12月</div>
                                <div class="timeline-content">
                                    <h5>前端开发</h5>
                                    <p>使用Bootstrap 5开发用户界面，创建母版页和首页。</p>
                                </div>
                            </div>
                            <div class="timeline-item">
                                <div class="timeline-date">2025年12月</div>
                                <div class="timeline-content">
                                    <h5>后端开发</h5>
                                    <p>实现新闻管理、用户认证、搜索等核心功能。</p>
                                </div>
                            </div>
                            <div class="timeline-item">
                                <div class="timeline-date">2025年12月</div>
                                <div class="timeline-content">
                                    <h5>测试优化</h5>
                                    <p>进行系统测试，修复Bug，优化用户体验。</p>
                                </div>
                            </div>
                            <div class="timeline-item">
                                <div class="timeline-date">2025年12月</div>
                                <div class="timeline-content">
                                    <h5>正式上线</h5>
                                    <p>系统部署上线，进入维护和更新阶段。</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 联系方式 -->
        <div class="row">
            <div class="col-12">
                <div class="card border-0 shadow-lg">
                    <div class="card-body p-5">
                        <div class="text-center mb-5">
                            <h2 class="fw-bold"><i class="fas fa-address-card me-2"></i>联系我们</h2>
                            <p class="text-muted">如有任何问题或建议，欢迎联系我们</p>
                        </div>
                        
                        <div class="row">
                            <div class="col-lg-4 mb-4 mb-lg-0">
                                <div class="text-center p-4">
                                    <div class="icon-circle bg-primary mb-3">
                                        <i class="fas fa-map-marker-alt fa-2x text-white"></i>
                                    </div>
                                    <h5>办公地址</h5>
                                    <p class="text-muted">北京市丰台区花乡张家路口121号</p>
                                </div>
                            </div>
                            
                            <div class="col-lg-4 mb-4 mb-lg-0">
                                <div class="text-center p-4">
                                    <div class="icon-circle bg-success mb-3">
                                        <i class="fas fa-phone fa-2x text-white"></i>
                                    </div>
                                    <h5>联系电话</h5>
                                    <p class="text-muted">+86 137-2007 1615<br>周一至周五 9:00-18:00</p>
                                </div>
                            </div>
                            
                            <div class="col-lg-4">
                                <div class="text-center p-4">
                                    <div class="icon-circle bg-info mb-3">
                                        <i class="fas fa-envelope fa-2x text-white"></i>
                                    </div>
                                    <h5>电子邮箱</h5>
                                    <p class="text-muted">m13720071615@163.com<br>support@newssystem.com</p>
                                </div>
                            </div>
                        </div>
                        
                        <div class="text-center mt-5">
                            <a href="Contact.aspx" class="btn btn-primary btn-lg px-5">
                                <i class="fas fa-paper-plane me-2"></i>发送消息
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 返回首页按钮 -->
        <div class="row mt-5">
            <div class="col-12 text-center">
                <a href="Default.aspx" class="btn btn-outline-secondary">
                    <i class="fas fa-home me-2"></i>返回首页
                </a>
            </div>
        </div>
    </div>

    <!-- 添加时间线样式 -->
    <style>
        .timeline {
            position: relative;
            max-width: 800px;
            margin: 0 auto;
        }
        
        .timeline::after {
            content: '';
            position: absolute;
            width: 6px;
            background-color: #e9ecef;
            top: 0;
            bottom: 0;
            left: 50%;
            margin-left: -3px;
        }
        
        .timeline-item {
            padding: 10px 40px;
            position: relative;
            width: 50%;
            box-sizing: border-box;
        }
        
        .timeline-item:nth-child(odd) {
            left: 0;
        }
        
        .timeline-item:nth-child(even) {
            left: 50%;
        }
        
        .timeline-item::after {
            content: '';
            position: absolute;
            width: 20px;
            height: 20px;
            background-color: white;
            border: 4px solid #0d6efd;
            border-radius: 50%;
            top: 15px;
            z-index: 1;
        }
        
        .timeline-item:nth-child(odd)::after {
            right: -10px;
        }
        
        .timeline-item:nth-child(even)::after {
            left: -10px;
        }
        
        .timeline-content {
            padding: 20px;
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        
        .timeline-date {
            font-weight: bold;
            color: #0d6efd;
            margin-bottom: 10px;
        }
        
        .icon-circle {
            width: 80px;
            height: 80px;
            border-radius: 50%;
            display: inline-flex;
            align-items: center;
            justify-content: center;
        }
        
        @media (max-width: 768px) {
            .timeline::after {
                left: 31px;
            }
            
            .timeline-item {
                width: 100%;
                padding-left: 70px;
                padding-right: 25px;
            }
            
            .timeline-item:nth-child(even) {
                left: 0;
            }
            
            .timeline-item::after {
                left: 21px;
            }
            
            .timeline-item:nth-child(even)::after {
                left: 21px;
            }
        }
        
        /* 卡片悬停效果 */
        .card.hover-shadow:hover {
            transform: translateY(-5px);
            transition: transform 0.3s ease;
        }
        
        /* 团队图片效果 */
        .team-img .rounded-circle {
            transition: all 0.3s ease;
        }
        
        .team-img .rounded-circle:hover {
            transform: scale(1.1);
            box-shadow: 0 5px 15px rgba(0,0,0,0.2);
        }
    </style>
</asp:Content>