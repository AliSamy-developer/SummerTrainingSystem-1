"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
connection.start();

debugger
connection.on("ReceivePrivateNotification", function (senderName, content, when) {

    console.log("Received Notification");

    var RecieverEmail = document.getElementById("CompanyName").value;
    var myEmail = document.getElementById("MyEmail").value;
    //var myPicUrl = document.querySelector(".profile-picture").getAttribute("src");
    debugger
    if (myEmail === RecieverEmail) {
        showNotification(senderName, content, when);
    } else {
        var notyf = new Notyf();
    }
});
function applyForTrainingAndSendMessage(event, url, trainingId) {
    event.preventDefault(); 

    sendPrivateMessage(event);
    debugger

    fetch(url + '?trid=' + trainingId, {
        method: 'GET'
    }).then(response => {
        if (response.ok) {
            notyf.success("Applied Successfully!");
            setTimeout(() => {
                window.location.reload();
            }, 1000);
           
        } else {
            console.error("Error applying for training");
        }
    }).catch(error => console.error(error));
}

function sendPrivateMessage(event) {

    event.preventDefault();

    var receiver = document.getElementById("CompanyName").value;
    var message = "Has Applied Succesfully";

    if (receiver != "" && message.trim() !== "") {

        connection.invoke("SendNotificationToGroup", receiver, message)
            .catch(function (err) {
                return console.error(err.toString());
            });
    }

    message = "";
}
function showNotification(senderName, content, when) {
    var wrapper = document.getElementById("notificationWrapper");

    if (!wrapper) {
        console.error("notificationWrapper not found!");
        return;
    }

    // Create notification element
    var notification = document.createElement("div");
    notification.classList.add("notification-body");
    notification.innerHTML = `
        <div class="notification-info">
            
            <div class="notification-details">
                <h5>${senderName}</h5>
                <div class="notification-time">${when}</div>
            </div>
        </div>
        <div class="notification-content">
            ${content}
        </div>
    `;

    wrapper.appendChild(notification);

    // Show notification with animation
    setTimeout(() => {
        notification.style.opacity = "1";
        notification.style.transform = "translateY(0)";
    }, 100);

    // Hide notification after 5 seconds
    setTimeout(() => {
        notification.style.opacity = "0";
        notification.style.transform = "translateY(-20px)";
        setTimeout(() => {
            notification.remove();
        }, 500);
    }, 5000);
}
