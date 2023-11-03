"use strict";

//SignalR連線
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;



//取得使用者與訊息
connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

///連線後取得該使用者Id
connection.on("GetConnectionId", function (ConnectionId) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `id: ${ConnectionId}`;
});

///密語
connection.on("SendSpecOne", function (userName, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${userName} 對你說： ${message}`;
});


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


//送出至Hub
document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var connectionId = document.getElementById("connectionId").value;

    try {
        //如果有填寫特定人的ID則為密語
        if (connectionId != "") {
            //有逗點則為多人傳訊
            if (connectionId.includes(',')) { 
                const array = connectionId.split(','); 
                connection.invoke("SendMessageToOthers", array,user, message).catch(function (err) {
                    return console.error(err.toString());
                })
                return;
            }


            connection.invoke("SendMessageToSpec", connectionId, user, message).catch(function (err) {
                return console.error(err.toString());
            })
            return;
        }
        //如果沒有填寫特定人的ID則為全體發話
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    } catch (err) {
        console.error(err);
    }

    
});