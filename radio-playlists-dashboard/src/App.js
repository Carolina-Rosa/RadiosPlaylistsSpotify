import "./App.css";
import { useEffect, useState } from "react";
import { Routes, Route } from "react-router-dom";
import Footer from "./Components/footer";
import Header from "./Components/header";
import DashboardPage from "./pages/dashboardPage";
import StatsPage from "./pages/statsPage";
import NoPage from "./pages/noPage";

const WS_URL = "ws://localhost:5067/webSocket";

console.log("socket");

function App() {
    const [message, setMessage] = useState("");

    useEffect(() => {
        const ws = new WebSocket(WS_URL);

        ws.onopen = () => {
            console.log("Connection Established!");
        };
        ws.onmessage = (event) => {
            setMessage(JSON.parse(event.data));
            //ws.close();
        };
        ws.onclose = () => {
            console.log("Connection Closed!");
            //initWebsocket();
        };

        ws.onerror = () => {
            console.log("WS Error");
        };

        return () => {
            ws.close();
        };
    }, []);

    return (
        <>
            <Header />
            <Routes>
                <Route
                    path="/"
                    element={<DashboardPage messageFromSocket={message} />}
                />
                <Route path="stats" element={<StatsPage />} />
                <Route path="*" element={<NoPage />} />
            </Routes>
            <Footer />
        </>
    );
}

export default App;
