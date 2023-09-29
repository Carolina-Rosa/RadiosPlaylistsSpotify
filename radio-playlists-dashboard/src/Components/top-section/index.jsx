import React from "react";

import "./styles.scss";
import Top5 from "../../Components/top5";

export default function TopSection({ title, topByRadio }) {
    return (
        <div className="top-section">
            <h1>{title}</h1>
            {topByRadio.map((top, i) => (
                <Top5
                    key={i}
                    title={top["topType"]}
                    content={top["topArtists"] ?? top["topSongs"]}
                />
            ))}
        </div>
    );
}
