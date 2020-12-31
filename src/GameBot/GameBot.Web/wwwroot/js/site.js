// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/user-updates").build();

connection.start().then(function () {
    console.log("Hub Started");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("SendMessage", function (message) {
    console.log("Message Recieved");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});