import React, { useEffect, useState } from "react";
import axios from "axios";

import "./styles.scss";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import TopSection from "../../Components/top-section";

export default function TopStats({ title, request }) {
    const TimeRange = {
        Last24Hours: {
            displayName: "Last 24 hours",
            name: "Last24Hours",
            id: 0
        },
        Last7Days: { displayName: "Last 7 days", name: "Last7Days", id: 1 },
        Last30Days: { displayName: "Last 30 days", name: "Last30Days", id: 2 },
        FromStart: { displayName: "From start", name: "FromStart", id: 3 }
    };

    const [radiosTop, setRadiosTop] = useState([]);

    const [selectedTab, setSelectedTab] = useState(TimeRange.Last24Hours.id);

    useEffect(() => {
        console.log(radiosTop);
    }, [radiosTop]);

    useEffect(() => {
        axios
            .get(`https://localhost:7270/api/${request}/top5/${selectedTab}`)
            .then((res) => {
                setRadiosTop(res.data);
            });
    }, [request, selectedTab]);

    return (
        <>
            <h2 className="stats-subtitle">{title}</h2>
            <Tabs
                defaultIndex={TimeRange.Last24Hours.id}
                selectedTabClassName="selected-tab"
                onSelect={(tabIndex) => setSelectedTab(tabIndex)}
            >
                <TabList className="tabs-list">
                    <Tab>{TimeRange.Last24Hours.displayName}</Tab>
                    <Tab>{TimeRange.Last7Days.displayName}</Tab>
                    <Tab>{TimeRange.Last30Days.displayName}</Tab>
                    <Tab>{TimeRange.FromStart.displayName}</Tab>
                </TabList>
                <TabPanel>
                    <TopSection
                        title={TimeRange.Last24Hours.displayName}
                        topByRadio={radiosTop}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title={TimeRange.Last7Days.displayName}
                        topByRadio={radiosTop}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title={TimeRange.Last30Days.displayName}
                        topByRadio={radiosTop}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title={TimeRange.FromStart.displayName}
                        topByRadio={radiosTop}
                    />
                </TabPanel>
            </Tabs>
        </>
    );
}
