import { HubConnectionBuilder, HttpTransportType } from "@microsoft/signalr";
import { setCountdown } from "../timerMessage/timerMessageSlice";

const WS_URL = "wss://localhost:7269/chatHub";

const signalRMiddleware = (store) => {
    const newConnection = new HubConnectionBuilder()
        .withUrl(WS_URL, {
            skipNegotiation: true,
            transport: HttpTransportType.WebSockets
        })
        .withAutomaticReconnect()
        .build();

    newConnection
        .start()
        .then((result) => {
            console.log("Connected!");

            newConnection.on("ReceiveMessage", (message) => {
                const messageFromSocket = JSON.parse(message);
                switch (messageFromSocket.MessageType) {
                    case 2:
                        store.dispatch(setCountdown(messageFromSocket.Message));
                        break;
                    default:
                        break;
                }
            });
        })
        .catch((e) => console.log("Connection failed: ", e));
    return (next) => (action) => next(action);
};

export default signalRMiddleware;
