const formulario = document.getElementById('formulario');
const inputs = document.querySelectorAll('#formulario input');

const expresiones = {
    nombre: /^[a-zA-ZÀ-ÿ\s]{3,30}$/,
    apellidos: /^[a-zA-ZÀ-ÿ\s]{3,30}$/,
    documento: /^\d{6,15}$/,
}

const campos = {
    nombre: true,
    apellidos: true,
    documento: true
}

const validarFormulario = (e) => {
    switch (e.target.name) {
        case "nombre":
            validarCampo(expresiones.nombre, e.target, 'nombre');
            break;
        case "apellidos":
            validarCampo(expresiones.apellidos, e.target, 'apellidos');
            break;
        case "fechaNacimiento":
            console.log('');
            break;
        case "documento":
            validarCampo(expresiones.documento, e.target, 'documento');
            break;
    }
}

const validarCampo = (expresion, input, campo) => {
    if (expresion.test(input.value)) {
        document.querySelector(`#grupo__${campo} .formulario__input-error`).classList.remove('formulario__input-error-activo');
        campos[campo] = true;
    } else {
        document.querySelector(`#grupo__${campo} .formulario__input-error`).classList.add('formulario__input-error-activo');
        campos[campo] = false;
    }
}

inputs.forEach((input) => {
    input.addEventListener('keyup', validarFormulario);
    input.addEventListener('blur', validarFormulario);
});

formulario.addEventListener('submit', (e) => {

    if (campos.nombre && campos.apellidos && campos.documento) {

    } else {
        document.getElementById('formulario__mensaje').classList.add('formulario__mensaje-activo');
        e.preventDefault();
    }
});