import { HubConnectionBuilder, HttpTransportType } from "@microsoft/signalr";
import { setCountdown } from "../timerMessage/timerMessageSlice";
import {
    addLogs,
    addLogsToExistingRadio
} from "../logsMessage/logsMessageSlice";
import {
    addRadios,
    addPlayingNowToExistingRadio
} from "../playingNowMessage/playingNowMessageSlice";

const WS_URL = "wss://localhost:7270/chatHub";

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
                separateMessages(message, store);
            });
        })
        .catch((e) => console.log("Connection failed: ", e));
    return (next) => (action) => next(action);
};

const separateMessages = (message, store) => {
    const messageFromSocket = JSON.parse(message);
    switch (messageFromSocket.MessageType) {
        case 0:
            const radioslist = store.getState().PlayingNowMessage.radiosList;
            var radIndex = radioslist.findIndex((obj) => {
                return obj.radio === messageFromSocket.RadioName;
            });

            var myObject = JSON.parse(messageFromSocket.Message);

            if (radIndex !== -1) {
                store.dispatch(
                    addPlayingNowToExistingRadio({
                        radio: radIndex,
                        music: myObject.Music,
                        artist: myObject.Artist
                    })
                );
            } else {
                store.dispatch(
                    addRadios({
                        radio: messageFromSocket.RadioName,
                        music: myObject.Music,
                        artist: myObject.Artist
                    })
                );
            }
            break;
        case 1:
            const logslist = store.getState().LogsMessage.logsList;
            var radioIndex = logslist.findIndex((obj) => {
                return obj.radio === messageFromSocket.RadioName;
            });
            if (radioIndex !== -1) {
                store.dispatch(
                    addLogsToExistingRadio({
                        radio: radioIndex,
                        logsList: messageFromSocket
                    })
                );
            } else {
                store.dispatch(
                    addLogs({
                        radio: messageFromSocket.RadioName,
                        logsList: [messageFromSocket]
                    })
                );
            }
            break;
        case 2:
            store.dispatch(setCountdown(messageFromSocket.Message));
            break;
        default:
            break;
    }
};

export default signalRMiddleware;
