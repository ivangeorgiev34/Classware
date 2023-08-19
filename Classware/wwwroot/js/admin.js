var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    connection.invoke("AddAdmin").catch(function (error) {
        return console.error(error.toString())
    });
}).catch(function (error) {
    return console.error(error.toString());
});

function onIsAnsweredButtonHandler(e) {
    let tokens = e.currentTarget.previousElementSibling.textContent.split(": ");
    connection.invoke("SetMessageToAnswered", tokens[tokens.length - 1]).catch(function (error) {
        return console.error(error.toString());
    })

    window.location.href = "https://localhost:7287/administrator/Home/Index";
};

let buttons = Array.from(document.querySelectorAll("button")).forEach(x => {
    x.addEventListener("click", onIsAnsweredButtonHandler);
});

connection.on("RecieveMessage", function (fullName, email, title, description, messageId) {

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

    let messageIdSpan = document.createElement("span");
    messageIdSpan.textContent = `Message Id: ${messageId}`;

    messageIdSpan.style.display = "block";
    messageIdSpan.style.fontSize = "18px";
    messageIdSpan.style.marginBottom = "10px";

    li.appendChild(messageIdSpan);

    let isAnsweredBtn = document.createElement("button");

    isAnsweredBtn.style.all = "unset";
    isAnsweredBtn.style.padding = "10px 20px";
    isAnsweredBtn.style.color = "white";
    isAnsweredBtn.style.backgroundColor = "green";
    isAnsweredBtn.style.borderRadius = "10px";
    isAnsweredBtn.style.marginBottom = "10px";
    isAnsweredBtn.type = "button";
    isAnsweredBtn.textContent = "Is Answered"

    isAnsweredBtn.addEventListener("click", onIsAnsweredButtonHandler);

    li.appendChild(isAnsweredBtn);

    li.style.border = "1px solid black";
    li.style.listStyleType = "none";
    li.style.marginBottom = "20px";
    li.style.paddingLeft = "20px";
    li.style.paddingTop = "10px";

    document.querySelector(".messagesList").appendChild(li);
});


