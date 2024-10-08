const readline = require('readline');
const signalR = require('@microsoft/signalr');

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/chatHub")
    .build();

connection.on("ReceiveMessage", function (message) {
    console.log("New message: ", message);
})

connection.start().then(function () {
    console.log("Connected to SignalR");

    connection.invoke("JoinRoom", "1").catch(function (err) {
        console.error(err.toString());
    })
}).catch(function (err) {
    return console.error(err.toString());
});

// Start SignalR connection

// Close until input
let rl = readline.createInterface(process.stdin, process.stdout);

rl.question("Press any key to exit...", (answer) => rl.close());