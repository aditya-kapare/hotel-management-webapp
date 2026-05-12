//function toggleSidebar() {
//    const sidebar = document.querySelector('.sidebar');
//    const icon = document.querySelector('#sidebarToggle i');

//    if (!sidebar) return;

//    const isCollapsed = sidebar.classList.toggle('sidebar-collapsed');

//    // ✅ Persist state
//    localStorage.setItem('sidebarCollapsed', isCollapsed ? 'true' : 'false');

//    // ✅ Update icon
//    if (isCollapsed) {
//        icon.classList.remove('bi-chevron-left');
//        icon.classList.add('bi-chevron-right');
//    } else {
//        icon.classList.remove('bi-chevron-right');
//        icon.classList.add('bi-chevron-left');
//    }
//}

//document.addEventListener('DOMContentLoaded', function () {
//    const sidebar = document.querySelector('.sidebar');
//    const icon = document.querySelector('#sidebarToggle i');

//    if (!sidebar) return;

//    const isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';

//    if (isCollapsed) {
//        sidebar.classList.add('sidebar-collapsed');

//        if (icon) {
//            icon.classList.remove('bi-chevron-left');
//            icon.classList.add('bi-chevron-right');
//        }
//    }
//});