const formulario = document.getElementById('formulario');
const inputs = document.querySelectorAll('#formulario input');

const expresiones = {
    nombre: /^[a-zA-ZÀ-ÿ\s]{3,30}$/,
    apellido: /^[a-zA-ZÀ-ÿ\s]{3,30}$/,
    documento: /^\d{6,15}$/,
    email: /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/,
    password: /^.{6,30}$/
}