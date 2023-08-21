var btnTabFilter = document.querySelector('#Main .FilterResults .Btn-Tab-Filter');
var filterlabel = document.querySelector('#Main .FilterResults #FilterLabel');
var filter = document.querySelector('#Main .FilterResults .Filter');



btnTabFilter.addEventListener('change', function() {
    if(this.checked) {
        filter.classList.remove('d-none');
        filter.classList.add('CustomCssFilter');
        filter.classList.add('col-9');
    }else {
        filter.classList.add('d-none');
        filter.classList.remove('CustomCssFilter');
    }
});

document.addEventListener('click', function (event) {
    if (!filter.contains(event.target) && !btnTabFilter.contains(event.target) && !filterlabel.contains(event.target)) {
        filter.classList.add('d-none');
        filter.classList.remove('CustomCssFilter');
        btnTabFilter.checked = false;
    }
});