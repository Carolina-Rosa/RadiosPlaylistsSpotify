import "./App.css";
import { Routes, Route } from "react-router-dom";
import Footer from "./Components/footer";
import Header from "./Components/header";
import DashboardPage from "./pages/dashboardPage";
import StatsPage from "./pages/statsPage";
import NoPage from "./pages/noPage";
import WhatWasPlayingPage from "./pages/whatWasPlayingPage";

function App() {
    return (
        <>
            <Header />
            <Routes>
                <Route path="/" element={<DashboardPage />} />
                <Route path="stats" element={<StatsPage />} />
                <Route
                    path="what-was-playing"
                    element={<WhatWasPlayingPage />}
                />
                <Route path="*" element={<NoPage />} />
            </Routes>
            <Footer />
        </>
    );
}

export default App;
