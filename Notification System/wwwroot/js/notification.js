"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.start().then(function () {
    console.log('connected to hub');
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("OnConnected", function () {
    OnConnected();
});

function OnConnected() {
    var username = $('#Username').val();
    connection.invoke("SaveUserConnection", username).catch(function (err) {
        return console.error(err.toString());
    })
}

connection.on("ReceivedNotification", function (message) {
    // alert(message);
    DisplayGeneralNotification(message, 'General Message');
});

connection.on("ReceivedPersonalNotification", function (message, username) {
    // alert(message + ' - ' + username);
    DisplayPersonalNotification(message, 'hey ' + username);
});

// Function to add a notification to the list
//function addNotificationToList(message, messageType, username, notificationDate) {
//    const notificationContainer = document.querySelector('.row');

//    // Create a new card for the notification
//    const card = document.createElement('div');
//    card.classList.add('col-md-12', 'col-lg-6', 'mb-4');

//    const cardBody = document.createElement('div');
//    cardBody.classList.add('card');

//    const cardTitle = document.createElement('div');
//    cardTitle.classList.add('card-body');

//    const h5 = document.createElement('h5');
//    h5.classList.add('card-title');
//    h5.textContent = message;

//    const smallText = document.createElement('small');
//    smallText.classList.add('card-text', 'text-muted');
//    smallText.textContent = `Date: ${new Date(notificationDate).toLocaleString()}`;

//    cardTitle.appendChild(h5);
//    cardTitle.appendChild(smallText);
//    cardBody.appendChild(cardTitle);
//    card.appendChild(cardBody);
//    notificationContainer.appendChild(card);

//    // Optionally, display a Toastr notification for visual feedback
//    toastr.info(message, 'New Notification');
//}




//// Establish a connection to the SignalR hub
//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/notificationHub")
//    .build();

//// Start the connection
//connection.start().catch(function (err) {
//    return console.error(err.toString());
//});

//// Function to add a notification to the list
//function addNotificationToList(message, messageType, username, notificationDate) {
//    const notificationList = document.getElementById("notification-list");
//    const noNotificationsMessage = document.getElementById("no-notifications-message");

//    // Create a new notification item
//    const notificationItem = document.createElement("tr");
//    const messageCell = document.createElement("td");
//    const dateCell = document.createElement("td");

//    messageCell.textContent = message;
//    dateCell.textContent = new Date(notificationDate).toLocaleString();

//    notificationItem.appendChild(messageCell);
//    notificationItem.appendChild(dateCell);

//    notificationList.appendChild(notificationItem);

//    // Hide the "No notifications found" message if it is visible
//    noNotificationsMessage.style.display = "none";
//}

//// Listen for general notifications
//connection.on("ReceivedNotification", function (message, messageType, username, notificationDate) {
//    addNotificationToList(message, messageType, username, notificationDate);
//    // Optionally, display a Toastr notification for general messages
//    toastr.info(message, 'General Message');
//});

//// Listen for personal notifications
//connection.on("ReceivedPersonalNotification", function (message, messageType, username, notificationDate) {
//    addNotificationToList(message, messageType, username, notificationDate);
//    // Optionally, display a Toastr notification for personal messages
//    toastr.success(message, 'Personal Message');
//});
