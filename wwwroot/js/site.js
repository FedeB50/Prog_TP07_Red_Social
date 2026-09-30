let userValido = false;
let passwordValida = false;
//Le pregunté a la IA cómo hacer para no permitir determinados caracteres especiales y me dijo que este tipo de dato podía servirme
const caracteres = /^[a-zA-Z0-9._]+$/;
const regexPassword = /^(?=.*[0-9])(?=.*[A-Z])(?=.*[^A-Za-z0-9]).+$/;
function requisitosMinimosUsuario() {
    const username = document.getElementById("username").value;
    const requisitos = document.getElementById("requisitos-usuario");
    let requisito1 = false;
    let requisito2 = false;
    if (!caracteres.test(username)) {
        requisitos.innerHTML = "El nombre de usuario no puede contener caracteres especiales excepto '.' y '_'.";
    } else {requisito1 = true;}
    if (username.length < 5) {
        requisitos.innerHTML = "El nombre de usuario debe tener al menos 5 caracteres.";
    } else if (username.length > 10) {
        requisitos.innerHTML = "El nombre de usuario no puede tener más de 10 caracteres.";
    } else {requisito2 = true;}

    if (requisito1 && requisito2) {requisitos.innerHTML = ""; userValido = true;}
    else {userValido = false;}
    verificarBoton();
}
function requisitosMinimosContraseña() {
    const password = document.getElementById("password").value;
    const requisitos = document.getElementById("requisitos-contraseña");
    requisitos.innerHTML = "";
    let errores = [];
    let requisito1 = false;
    let requisito2 = false;
    
    if (password.length < 8) {
        errores.push("La contraseña debe tener al menos 8 caracteres.");
    } else {requisito1 = true;}
    if (!regexPassword.test(password)) {
        errores.push("La contraseña debe contener al menos un número, una mayúscula y un carácter especial.");
    } else {requisito2 = true;}
    
    requisitos.innerHTML = errores.join("<br>");

    if (requisito1 && requisito2) {
        requisitos.innerHTML = "";
        passwordValida = true;
    } else {
        passwordValida = false;
    }
    verificarBoton();
}
function verificarBoton() {
    const boton = document.getElementById("boton");
    if (userValido && passwordValida) {
        boton.disabled = false;
    } else {
        boton.disabled = true;
    }
}

function DarLike(idPublicacion)
{
    fetch( '/Home/Like?IdPublicacion=' + idPublicacion, {method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    })
    .then(response => response.json())
    .then(data => {
         document.getElementById("cantLikes_" + idPublicacion).innerHTML = data;
        })
    .catch((error) => {
        console.error('Error:', error);
    });
}

function HacerComentario(idPublicacion, comentario)
{
    fetch( '/Home/Like?IdPublicacion=' + idPublicacion, {method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    })
    .then(response => response.json())
    .then(data => {
         document.getElementById("cantLikes_" + idPublicacion).innerHTML = data;
        })
    .catch((error) => {
        console.error('Error:', error);
    });
}