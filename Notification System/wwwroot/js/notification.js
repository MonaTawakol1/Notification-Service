//"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

//connection.start().then(function () {
//    console.log('SignalR connected');
//    var username = $('#Username').val();
//    connection.invoke("SaveUserConnection", username).catch(function (err) {
//        return console.error('SaveUserConnection error: ', err.toString());
//    });
//}).catch(function (err) {
//    return console.error('SignalR connection error: ', err.toString());
//});

//connection.on("ReceivedNotification", function (message) {
//    alert("General notification: " + message);
//    //DisplayGeneralNotification(message, 'General Message');
//});

//connection.on("ReceivedPersonalNotification", function (message, username) {
//    console.log("Personal notification received:", message, username);
//    alert("Personal notification: " + message + " - " + username);
//    DisplayPersonalNotification(message, 'Hey ' + username);
//});

//connection.on("OnConnected", function () {
//    var username = $('#Username').val();
//    connection.invoke("SaveUserConnection", username).catch(function (err) {
//        return console.error('SaveUserConnection error: ', err.toString());
//    });
//});

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