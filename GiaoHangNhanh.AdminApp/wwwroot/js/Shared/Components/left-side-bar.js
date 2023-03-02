$(function () {
    $('.aside-menu a.menu-link').each(function () {
        let trangThaiMenu = sessionStorage.getItem('trangThaiMenuId_' + $(this).data('menuid'));
        
        if (trangThaiMenu && trangThaiMenu == '1') {

            $(this).addClass('active');

            $(this).parents('.menu-accordion').each(function () {
                $(this).addClass('hover show');
            });
        }
    })
})

$(document).ready(function () {
    $('.aside-menu a.menu-link').click(function (e) {
        sessionStorage.clear();
        sessionStorage.setItem('trangThaiMenuId_' + $(this).data('menuid'), '1');
    });
});