import React, { useState } from "react";
import "./styles.scss";

export default function TopItem({ itemNo, timesPlayed, songName, songArtist }) {
    const [isHovering, setIsHovering] = useState(false);

    const handleMouseOver = () => {
        setIsHovering(true);
    };

    const handleMouseOut = () => {
        setIsHovering(false);
    };

    return (
        <div
            className="top-item-container"
            onMouseOver={handleMouseOver}
            onMouseOut={handleMouseOut}
        >
            <h1>#{itemNo}</h1>
            {isHovering ? (
                <p className="times-played">{timesPlayed}x</p>
            ) : (
                <>
                    <h2>
                        {songName.length > 30
                            ? songName.substr(0, 30) + "\u2026"
                            : songName}
                    </h2>
                    <p>{songArtist}</p>
                </>
            )}
        </div>
    );
}
