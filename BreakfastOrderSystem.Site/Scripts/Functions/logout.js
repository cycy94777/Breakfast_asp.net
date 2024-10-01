function logout() {
    if (confirm('您確定要登出嗎？')) { // 確認登出
        var logoutUrl = document.querySelector('#logout-icon a').getAttribute('data-logout-url');
        window.location.href = logoutUrl;
    }
}