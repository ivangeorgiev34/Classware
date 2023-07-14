var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    connection.invoke("AddAdmin").catch(function (error) {
        return console.error(error.toString())
    });
}).catch(function (error) {
    return console.error(error.toString());
});

connection.on("RecieveMessage", function (fullName, email, title, description) {

    let li = document.createElement("li");
    li.classList.add("messageListItem");
    li.style.display = "flex";
    li.style.flexDirection = "column";
    li.style.alignItems = "center";

    let titleHeading = document.createElement("h2");
    titleHeading.textContent = title;
    li.appendChild(titleHeading);

    let descriptionParagraph = document.createElement("p");
    descriptionParagraph.textContent = `Description: ${description}`;

    descriptionParagraph.style.fontSize = "22px";

    li.appendChild(descriptionParagraph);

    let fullNameSpan = document.createElement("span");
    fullNameSpan.textContent = `Full name: ${fullName}`;

    fullNameSpan.style.display = "block";
    fullNameSpan.style.fontSize = "18px";
    fullNameSpan.style.marginBottom = "10px";
    li.appendChild(fullNameSpan);


    let emailSpan = document.createElement("span");
    emailSpan.textContent = `Email: ${email}`;

    emailSpan.style.display = "block";
    emailSpan.style.fontSize = "18px";
    emailSpan.style.marginBottom = "10px";

    li.appendChild(emailSpan);

    li.style.border = "1px solid black";
    li.style.listStyleType = "none";
    li.style.marginBottom = "20px";
    li.style.paddingLeft = "20px";
    li.style.paddingTop = "10px";

    document.querySelector(".messagesList").appendChild(li);
});
