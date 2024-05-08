import React, { useState } from "react";

import "./styles.scss";
import TopStats from "../../Components/top-stats";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";

export default function StatsPage() {
    const StatsType = {
        Musics: { displayName: "Musics", id: 0 },
        Artists: { displayName: "Artists", id: 1 }
    };

    return (
        <div className="stats-page">
            <h1 className="stats-title">Statistics</h1>
            <Tabs
                defaultIndex={StatsType.Musics.id}
                selectedTabClassName="selected-tab-statsType"
            >
                <TabList className="tabs-list-statsType">
                    <Tab>{StatsType.Musics.displayName}</Tab>
                    <Tab>{StatsType.Artists.displayName}</Tab>
                </TabList>
                <TabPanel className="tab-panel-statsType">
                    <TopStats
                        title="Most played songs"
                        request="musicSpotify"
                    />
                </TabPanel>
                <TabPanel className="tab-panel-statsType">
                    <TopStats title="Most played artists" request="artists" />
                </TabPanel>
            </Tabs>
        </div>
    );
}
