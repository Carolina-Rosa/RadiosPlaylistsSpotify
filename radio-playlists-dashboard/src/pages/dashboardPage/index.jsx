import React from "react";
import "./styles.scss";
import TableLogs from "../../Components/tableLogs";
import RadioSection from "../../Components/radiosSection";
import Countdown from "../../Components/timer";

export default function DashboardPage() {
    return (
        <div className="dashboard-page">
            <div className="left-section">
                <h1 className="logs-title">Logs - {/* {radio} */}</h1>
                <TableLogs
                /* logs={logs.find((log) => log.radio === radio)?.logsList}
                    radio={radio} */
                />
            </div>
            <div className="right-section">
                <Countdown />
                {/*
                <RadioSection
                setRadio={setRadio}
                    radio={radio}
                    radiosList={radiosList} 
                    />
                    */}
            </div>
        </div>
    );
}
