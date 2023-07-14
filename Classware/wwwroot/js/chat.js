

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

let button = document.querySelector(".sendBtn");

button.addEventListener("click", function (event) {

    event.preventDefault();

    let hasErrors = false;

    let fullName = document.querySelector(".full-name-input").value;
    let email = document.querySelector(".email-input").value;
    let title = document.querySelector(".title-input").value;
    let description = document.querySelector(".description-input").value;

    let fullNameValidationSpan = document.querySelector(".fullNameValidation");
    let emailValidationSpan = document.querySelector(".emailValidation");
    let titleValidationSpan = document.querySelector(".titleValidation");
    let descriptionValidationSpan = document.querySelector(".descriptionValidation");

    //full name validation
    if (fullName.length === 0) {
        hasErrors = true;

        fullNameValidationSpan.textContent = "Full name is required!";
    } else {
        fullNameValidationSpan.textContent = "";
    }

    //email validation
    if (email.length === 0) {
        hasErrors = true;

        emailValidationSpan.textContent = "Email is required!";
    } else if (new RegExp('^[A-Za-z0-9_\.]+@[A-Za-z]+\.[A-Za-z]{2,3}$').test(email) === false) {
        hasErrors = true;

        emailValidationSpan.textContent = "Invalid email!";
    } else {
        emailValidationSpan.textContent = "";
    }

    //title validation
    if (title.length === 0) {
        hasErrors = true;

        titleValidationSpan.textContent = "Title is required!";
    } else {
        titleValidationSpan.textContent = "";
    }

    //description validation
    if (description.length === 0) {
        hasErrors = true;

        descriptionValidationSpan.textContent = "Description is required!";
    } else {
        descriptionValidationSpan.textContent = "";
    }

    if (hasErrors === false) {
        connection.invoke("SendMessageToAdmins", fullName, email, title, description).catch(function (error) {
            return console.error(error.toString());
        })

        window.location.href = "https://localhost:7287/";

    }

});

button.disabled = true;

connection.start().then(function () {
    button.disabled = false;
}).catch(function (error) {
    return console.error(error.toString());
});