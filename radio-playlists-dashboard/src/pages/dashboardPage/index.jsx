import React, { useState } from "react";
import { useSelector } from "react-redux";
import "./styles.scss";
import TableLogs from "../../Components/tableLogs";
import RadioSection from "../../Components/radiosSection";
import Countdown from "../../Components/timer";
import { Link } from "react-router-dom";

export default function DashboardPage() {
    const logs = useSelector((state) => state.LogsMessage.logsList);
    const radiosList = useSelector(
        (state) => state.PlayingNowMessage.radiosList
    );
    const [radio, setRadio] = useState("MegaHits");
    return (
        <div className="dashboard-page">
            <div className="left-section">
                <h1 className="logs-title">
                    Logs -{" "}
                    <Link className="logs-title" to={`/radioPage/${radio}`}>
                        {" "}
                        {radio}{" "}
                    </Link>
                </h1>
                <TableLogs
                    logs={logs.find((log) => log.radio === radio)?.logsList}
                />
            </div>
            <div className="right-section">
                <Countdown />
                <RadioSection
                    setRadio={setRadio}
                    radio={radio}
                    radiosList={radiosList}
                />
            </div>
        </div>
    );
}
