import React, { useState, useEffect } from "react";
import "./styles.scss";
import TableLogs from "../../Components/tableLogs";
import RadioSection from "../../Components/radiosSection";
import Countdown from "../../Components/timer";

export default function DashboardPage({ messageFromSocket }) {
    const [radio, setRadio] = useState("Antena3");
    const [countdownValue, setCountdownValue] = useState("90");
    const [radiosList, setRadiosList] = useState([]);
    const [logsListByRadio, setLogsListByRadio] = useState([]);

    useEffect(() => {
        separateMessages(
            messageFromSocket,
            setCountdownValue,
            setRadiosList,
            radiosList,
            logsListByRadio,
            setLogsListByRadio
        );
    }, [messageFromSocket, logsListByRadio]);

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
    switch (messageFromSocket.MessageType) {
        case 0:
            var myObject = JSON.parse(messageFromSocket.Message);

            var indRadio = radiosList.findIndex((obj) => {
                return obj.radio === messageFromSocket.RadioName;
            });

            if (indRadio !== -1) {
                radiosList[indRadio].music = myObject.Music;
                radiosList[indRadio].artist = myObject.Artist;
            } else {
                setRadiosList([
                    ...radiosList,
                    {
                        music: myObject.Music,
                        artist: myObject.Artist,
                        radio: messageFromSocket.RadioName
                    }
                ]);
            }
            break;
        case 1:
            //TODO - separte logs by radio
            var index = logsListByRadio.findIndex((obj) => {
                return obj.radio === messageFromSocket.RadioName;
            });

            if (index !== -1) {
                logsListByRadio[index].logsList.push(messageFromSocket);
            } else {
                setLogsListByRadio([
                    ...logsListByRadio,
                    {
                        radio: messageFromSocket.RadioName,
                        logsList: [messageFromSocket]
                    }
                ]);
            }
            //setLogs([...logs, messageFromSocket]);
            break;
        case 2:
            setCountdownValue(messageFromSocket.Message);
            break;
        default:
            break;
    }
};
