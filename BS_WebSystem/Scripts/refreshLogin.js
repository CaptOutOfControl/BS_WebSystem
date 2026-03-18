// 刷新登录状态的脚本
function refreshLoginStatus() {
    // 检查登录状态
    var isLoggedIn = document.cookie.indexOf('ASPXAUTH') !== -1;

    if (isLoggedIn) {
        // 如果已登录，更新页面显示
        updateLoginUI(true);
    } else {
        updateLoginUI(false);
    }
}

function updateLoginUI(isLoggedIn) {
    var loginContainer = document.getElementById('loginStatusContainer');
    if (!loginContainer) return;

    if (isLoggedIn) {
        // 这里可以通过AJAX获取用户名
        // 暂时使用占位符
        loginContainer.innerHTML = `
            <span class="me-3 text-white">欢迎，用户！</span>
            <a href="javascript:logout()" class="btn btn-outline-light btn-sm">退出</a>
        `;
    } else {
        loginContainer.innerHTML = `
            <a href="Account/Login.aspx" class="btn btn-outline-light btn-sm">登录</a>
            <a href="Account/Register.aspx" class="btn btn-light btn-sm ms-2">注册</a>
        `;
    }
}

function logout() {
    if (confirm('确定要退出登录吗？')) {
        // 提交退出表单
        document.getElementById('logoutForm').submit();
    }
}

// 页面加载完成后执行
document.addEventListener('DOMContentLoaded', function () {
    refreshLoginStatus();

    // 监听登录链接点击，添加时间戳防止缓存
    var loginLinks = document.querySelectorAll('a[href*="Login.aspx"]');
    loginLinks.forEach(function (link) {
        link.addEventListener('click', function (e) {
            var href = this.getAttribute('href');
            if (href.indexOf('?') === -1) {
                this.setAttribute('href', href + '?t=' + new Date().getTime());
            }
        });
    });
});

// 定时检查登录状态（可选，每30秒检查一次）
setInterval(refreshLoginStatus, 30000);