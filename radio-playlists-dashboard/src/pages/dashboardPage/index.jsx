import React, { useState, useEffect } from "react";
import "./styles.scss";
import TableLogs from "../../Components/tableLogs";
import RadioSection from "../../Components/radiosSection";
import Countdown from "../../Components/timer";
import { HubConnectionBuilder, HttpTransportType } from "@microsoft/signalr";

const WS_URL = "wss://localhost:7269/chatHub";

export default function DashboardPage() {
    const [radio, setRadio] = useState("Antena 3");
    const [countdownValue, setCountdownValue] = useState("90");
    const [radiosList, setRadiosList] = useState([]);
    const [logsListByRadio, setLogsListByRadio] = useState([]);

    const [messageFromSocket, setMessageFromSocket] = useState("");

    useEffect(() => {
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

                newConnection.on("ReceiveMessage", (message) =>
                    setMessageFromSocket(JSON.parse(message))
                );
            })
            .catch((e) => console.log("Connection failed: ", e));
    }, []);

    useEffect(() => {
        console.log(messageFromSocket);
        separateMessages(
            messageFromSocket,
            setCountdownValue,
            setRadiosList,
            radiosList,
            logsListByRadio,
            setLogsListByRadio
        );
    }, [messageFromSocket]); //

    return (
        <div className="dashboard-page">
            <div className="left-section">
                <h1 className="logs-title">Logs - {radio}</h1>
                <TableLogs
                    logs={
                        logsListByRadio.find((log) => log.radio === radio)
                            ?.logsList
                    }
                    radio={radio}
                />
            </div>
            <div className="right-section">
                <Countdown countdownValue={countdownValue} />
                <RadioSection
                    setRadio={setRadio}
                    radio={radio}
                    radiosList={radiosList}
                />
            </div>
        </div>
    );
}

const separateMessages = (
    messageFromSocket,
    setCountdownValue,
    setRadiosList,
    radiosList,
    logsListByRadio,
    setLogsListByRadio
) => {
    console.log("SEPARATE MESSAGES");
    switch (messageFromSocket.MessageType) {
        case 0:
            var myObject = JSON.parse(messageFromSocket.Message);

            var radioInd = radiosList.findIndex((obj) => {
                return obj.radio === messageFromSocket.RadioName;
            });

            if (radioInd !== -1) {
                radiosList[radioInd].music = myObject.Music;
                radiosList[radioInd].artist = myObject.Artist;
            } else {
                setRadiosList([
                    {
                        music: myObject.Music,
                        artist: myObject.Artist,
                        radio: messageFromSocket.RadioName
                    },
                    ...radiosList
                ]);
            }
            break;
        case 1:
            var radioIndex = logsListByRadio.findIndex((obj) => {
                return obj.radio === messageFromSocket.RadioName;
            });

            if (radioIndex !== -1) {
                logsListByRadio[radioIndex].logsList = [
                    messageFromSocket,
                    ...logsListByRadio[radioIndex].logsList
                ];
            } else {
                setLogsListByRadio([
                    {
                        radio: messageFromSocket.RadioName,
                        logsList: [messageFromSocket]
                    },
                    ...logsListByRadio
                ]);
            }
            break;
        case 2:
            setCountdownValue(messageFromSocket.Message);
            break;
        default:
            break;
    }
};
