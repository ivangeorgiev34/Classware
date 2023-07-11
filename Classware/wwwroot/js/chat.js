

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

let button = document.querySelector(".sendBtn");
button.addEventListener("click", function (event) {

    event.preventDefault();

    let fullName = document.querySelector(".full-name-input").value;
    let email = document.querySelector(".email-input").value;
    let title = document.querySelector(".title-input").value;
    let description = document.querySelector(".description-input").value;

    connection.invoke("SendMessageToAdmins", fullName, email, title, description).catch(function (error) {
        return console.error(error.toString());
    })

    window.location.href = "https://localhost:7287/";

});

button.disabled = true;

connection.start().then(function () {
    button.disabled = false;
}).catch(function (error) {
    return console.error(error.toString());
});