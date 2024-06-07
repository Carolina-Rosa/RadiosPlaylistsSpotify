import "./App.css";
import { Routes, Route } from "react-router-dom";
import Footer from "./Components/footer";
import Header from "./Components/header";
import DashboardPage from "./pages/dashboardPage";
import StatsPage from "./pages/statsPage";
import NoPage from "./pages/noPage";
import WhatWasPlayingPage from "./pages/whatWasPlayingPage";

import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import RadiosPage from "./pages/RadiosPage";

function App() {
    return (
        <>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
                <Header />
                <Routes>
                    <Route path="/" element={<DashboardPage />} />
                    <Route
                        exact
                        path="radioPage/:name"
                        element={<RadiosPage />}
                    />
                    <Route path="stats" element={<StatsPage />} />
                    <Route
                        path="what-was-playing"
                        element={<WhatWasPlayingPage />}
                    />
                    <Route path="*" element={<NoPage />} />
                </Routes>
                <Footer />
            </LocalizationProvider>
        </>
    );
}

export default App;
