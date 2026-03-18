class PageTransition {
    constructor() {
        this.init();
    }

    init() {
        // 拦截所有链接点击
        document.addEventListener('click', (e) => {
            const link = e.target.closest('a');

            if (link &&
                !link.target &&
                !link.href.startsWith('javascript:') &&
                !link.href.includes('#') &&
                link.href !== window.location.href) {
                e.preventDefault();
                this.navigateTo(link.href);
            }
        });

        // 处理浏览器前进/后退
        window.addEventListener('popstate', () => {
            this.handleNavigation(window.location.href, true);
        });
    }

    navigateTo(url, isPopState = false) {
        // 显示加载动画
        this.showLoading();

        // 获取新页面内容
        fetch(url, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Accept': 'text/html'
            }
        })
            .then(response => response.text())
            .then(html => {
                // 解析HTML，提取主要内容
                const parser = new DOMParser();
                const newDoc = parser.parseFromString(html, 'text/html');
                const newContent = newDoc.querySelector('[id$="MainContent"]') ||
                    newDoc.querySelector('.container') ||
                    newDoc.body;

                // 执行页面切换动画
                this.performTransition(newContent.innerHTML, url, isPopState);
            })
            .catch(error => {
                console.error('Navigation error:', error);
                window.location.href = url; // 回退到传统导航
            });
    }

    showLoading() {
        let loader = document.querySelector('.page-loading');
        if (!loader) {
            loader = document.createElement('div');
            loader.className = 'page-loading';
            loader.innerHTML = '<div class="loader"></div>';
            document.body.appendChild(loader);
        }
        loader.classList.remove('hidden');

        // 添加body类
        document.body.classList.add('page-changing');
    }

    hideLoading() {
        const loader = document.querySelector('.page-loading');
        if (loader) {
            setTimeout(() => {
                loader.classList.add('hidden');
                document.body.classList.remove('page-changing');
            }, 300);
        }
    }

    performTransition(newContent, url, isPopState = false) {
        const container = document.querySelector('.page-transition-container');
        const currentContent = container.querySelector('.current-page');

        if (currentContent) {
            // 标记当前页面为离开状态
            currentContent.classList.remove('current-page');
            currentContent.classList.add('leaving-page');

            // 添加新页面
            const newPage = document.createElement('div');
            newPage.className = 'new-page';
            newPage.innerHTML = newContent;
            container.appendChild(newPage);

            // 动画结束后移除旧页面
            setTimeout(() => {
                currentContent.remove();
                newPage.classList.remove('new-page');
                newPage.classList.add('current-page');

                // 更新URL和历史记录
                if (!isPopState) {
                    window.history.pushState({}, '', url);
                }

                // 重新初始化脚本和样式
                this.reinitializePage(newPage);
                this.hideLoading();

                // 滚动到顶部
                window.scrollTo(0, 0);
            }, 600);
        }
    }

    reinitializePage(container) {
        // 重新执行脚本
        const scripts = container.querySelectorAll('script');
        scripts.forEach(oldScript => {
            const newScript = document.createElement('script');
            Array.from(oldScript.attributes).forEach(attr => {
                newScript.setAttribute(attr.name, attr.value);
            });
            if (oldScript.innerHTML) {
                newScript.innerHTML = oldScript.innerHTML;
            }
            oldScript.parentNode.replaceChild(newScript, oldScript);
        });

        // 触发自定义事件，通知其他组件页面已更新
        window.dispatchEvent(new CustomEvent('pageupdated'));
    }
}

// 初始化页面切换
document.addEventListener('DOMContentLoaded', () => {
    window.pageTransition = new PageTransition();
});