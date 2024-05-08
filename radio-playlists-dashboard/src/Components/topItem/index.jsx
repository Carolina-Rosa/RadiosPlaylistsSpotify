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
            <h1 className="position-number">#{itemNo}</h1>

            {isHovering ? (
                <p className="times-played">{timesPlayed}x</p>
            ) : (
                <>
                    <h2>
                        {songName.length > 20
                            ? songName.substr(0, 20) + "\u2026"
                            : songName}
                    </h2>
                    <p>{songArtist}</p>
                </>
            )}
        </div>
    );
}
