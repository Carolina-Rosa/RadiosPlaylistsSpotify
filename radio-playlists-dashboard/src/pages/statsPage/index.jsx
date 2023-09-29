import React, { useEffect, useState } from "react";
import axios from "axios";

import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import TopSection from "../../Components/top-section";

import "./styles.scss";

export default function StatsPage() {
    const [radiosTopArtists, setRadiosTopArtists] = useState([]);
    const [radiosTopSongs, setRadiosTopSongs] = useState([]);

    useEffect(() => {
        axios.get(`https://localhost:7269/api/artists/top5`).then((res) => {
            setRadiosTopArtists(res.data);
        });

        axios
            .get(`https://localhost:7269/api/musicSpotify/top5`)
            .then((res) => {
                setRadiosTopSongs(res.data);
            });
    }, []);

    return (
        <div className="stats-page">
            <h1 className="stats-title">Statistics</h1>
            <h2 className="stats-subtitle">Most played songs </h2>
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
                        topByRadio={radiosTopSongs}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title="Last 7 days"
                        topByRadio={radiosTopSongs}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title="Last 30 days"
                        topByRadio={radiosTopSongs}
                    />
                </TabPanel>
                <TabPanel>
                    <TopSection
                        title="From start"
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
