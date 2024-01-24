let lstNavButtons = document.querySelectorAll('.nav-button');
const containerRelatorios = document.querySelector('#navRelatorios');
const containerCadastros = document.querySelector('#navCadastros');
const containerChecklists = document.querySelector('#navChecklists');
const containerDashboards = document.querySelector('#navDashboards');
const containerLoader = document.querySelector('.center');


document.querySelector('#btnRelatorios').addEventListener("click", event => {
    event.preventDefault();
    containerRelatorios.classList.add('enter');
    containerCadastros.classList.remove('enter');
    containerChecklists.classList.remove('enter');
    containerDashboards.classList.remove('enter');
});

document.querySelector('#exitRelatorios').addEventListener("click", event => {
    event.preventDefault();
    containerRelatorios.classList.remove('enter');
});


document.querySelector('#btnCadastros').addEventListener("click", event => {
    event.preventDefault();
    containerCadastros.classList.add('enter');
    containerChecklists.classList.remove('enter');
    containerRelatorios.classList.remove('enter');
    containerDashboards.classList.remove('enter');
});

document.querySelector('#exitCadastros').addEventListener("click", event => {
    event.preventDefault();
    containerCadastros.classList.remove('enter');
});


document.querySelector('#btnChecklists').addEventListener('click', event => {
    event.preventDefault();
    containerChecklists.classList.add('enter');
    containerCadastros.classList.remove('enter');
    containerDashboards.classList.remove('enter');
    containerRelatorios.classList.remove('enter');

});

document.querySelector('#exitChecklists').addEventListener('click', event => {
    event.preventDefault();
    containerChecklists.classList.remove('enter');
});

document.querySelector('#btnDashboards').addEventListener('click', event => {
    event.preventDefault();
    containerDashboards.classList.add('enter');
    containerCadastros.classList.remove('enter');
    containerChecklists.classList.remove('enter');
    containerRelatorios.classList.remove('enter');
});

document.querySelector('#exitDashboards').addEventListener('click', event => {
    event.preventDefault();
    containerDashboards.classList.remove('enter');
});


lstNavButtons.forEach((item) => {
    item.addEventListener("click", event => {
        event.preventDefault();
        containerRelatorios.classList.remove('enter');
        containerCadastros.classList.remove('enter');
        containerChecklists.classList.remove('enter');
        containerDashboards.classList.remove('enter');
        containerLoader.setAttribute("style", "visibility:visible;");
    });
});
