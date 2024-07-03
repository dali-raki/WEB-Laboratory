let isOpen = false; // Initial state is closed


function OpenCloseNavLeft() {
    const navLeft = document.getElementById('y-nav-left');
    const bigContainer = document.getElementById('y-big-container');
    const iconcontainer = document.getElementById('y-header-icon-navleft');
    const navItemTexts = document.querySelectorAll('.y-nav-item-text');
    const navItems = document.querySelectorAll('.y-nav-items-2');

    console.log("iam in ")
    if (isOpen) {
        navLeft.classList.remove('y-open');
        bigContainer.classList.remove('y-big-container-open');
        navItemTexts.forEach(item => item.style.display = 'none');
        navItems.forEach(item => item.style.width = 'fit-content');
        navLeft.style.alignItems = 'center';
        iconcontainer.style.width = 'fit-content';
        iconcontainer.style.justifyContent = 'center';
        console.log("close is donne  ")
    } else {

        navLeft.classList.add('y-open');

        bigContainer.classList.add('y-big-container-open');
        console.log("opening is donne  ")
        navItemTexts.forEach(item => item.style.display = 'block')
        navItems.forEach(item => item.style.width = '100%');
        navLeft.style.alignItems = 'flex-start';
        iconcontainer.style.width = '100%';
        iconcontainer.style.justifyContent = 'end';


    }
    isOpen = !isOpen; // Toggle the state
}