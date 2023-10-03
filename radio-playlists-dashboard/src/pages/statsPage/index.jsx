import React, { useEffect, useState } from "react";
import axios from "axios";

import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import TopSection from "../../Components/top-section";

import "./styles.scss";

export default function StatsPage() {
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

    const [radiosTopArtists, setRadiosTopArtists] = useState([]);
    const [radiosTopSongs, setRadiosTopSongs] = useState([]);
    const [selectedTab, setSelectedTab] = useState(TimeRange.Last24Hours.id);

    useEffect(() => {
        axios.get(`https://localhost:7269/api/artists/top5`).then((res) => {
            setRadiosTopArtists(res.data);
        });

        axios
            .get(`https://localhost:7269/api/musicSpotify/top5/${selectedTab}`)
            .then((res) => {
                setRadiosTopSongs(res.data);
            });
    }, [selectedTab]);

    return (
        <div className="stats-page">
            <h1 className="stats-title">Statistics</h1>
            <h2 className="stats-subtitle">Most played songs </h2>
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
                        topByRadio={radiosTopSongs}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title={TimeRange.Last7Days.displayName}
                        topByRadio={radiosTopSongs}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title={TimeRange.Last30Days.displayName}
                        topByRadio={radiosTopSongs}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title={TimeRange.FromStart.displayName}
                        topByRadio={radiosTopSongs}
                    />
                </TabPanel>
            </Tabs>

            <h2 className="stats-subtitle">Most played artists </h2>
            <Tabs defaultIndex={0} selectedTabClassName="selected-tab">
                <TabList className="tabs-list">
                    <Tab>Last 24 hours</Tab>
                    <Tab>Last 7 days</Tab>
                    <Tab>Last 30 days</Tab>
                    <Tab>From start</Tab>
                </TabList>
                <TabPanel>
                    <TopSection
                        title="Last 24 hours"
                        topByRadio={radiosTopArtists}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title="Last 7 days"
                        topByRadio={radiosTopArtists}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title="Last 30 days"
                        topByRadio={radiosTopArtists}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title="From start"
                        topByRadio={radiosTopArtists}
                    />
                </TabPanel>
            </Tabs>
        </div>
    );
}
