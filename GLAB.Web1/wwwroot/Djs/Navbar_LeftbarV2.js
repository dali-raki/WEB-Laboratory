document.getElementById('resizeButton').addEventListener('click', function () {
    var div1 = document.querySelector('.D-left-nav');
    var div2 = document.querySelector('.D-logo-left-nav #part1');
    var div3 = document.querySelector('.D-logo');

    if (div1.style.width === '15vh' || div1.style.width === '') {
        div1.style.width = '30vh'; // New width when button is clicked
        div2.style.visibility = "visible"
        div3.style.visibility = "hidden"
    } else {
        div1.style.width = '15vh'; // Original width to toggle back
        div2.style.visibility = "hidden"
        div3.style.visibility = "visible"
    }
});