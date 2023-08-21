

// start js click nav
var navItemCollapse = document.querySelectorAll('#Header .nav .nav-item .nav-link');
navItemCollapse.forEach(element => {
    element.addEventListener("click", function() {

        // Remove the "active" class from all list items
        navItemCollapse.forEach(function (item) {
            item.classList.remove("active");
        });

        // Add the "active" class to the clicked item
        this.classList.add("active");
    })
});
// end js click nav

// open dropMenu
btnTheLoai = document.querySelector('#Header .nav .dropdown');
var dropMenu = document.querySelector('#Header .nav .dropdown .Dropdown-Menu .container');
console.log(dropMenu)
btnTheLoai.addEventListener("click", function() {
    dropMenu.classList.add("opacity-100");
})