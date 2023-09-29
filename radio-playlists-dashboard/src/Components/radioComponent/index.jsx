import React from "react";
import "./styles.scss";

export default function RadioComponent({
    radioName,
    songTitle,
    songArtist,
    isSelected,
    setRadio
}) {
    return (
        <div
            className={`radio-component ${isSelected ? "is-selected" : ""}`}
            onClick={() => setRadio(radioName)}
        >
            <p className="radio-name">{radioName}</p>
            <p className="song-artist">{songArtist}</p>
            <p className="song-title">{songTitle}</p>
        </div>
    );
}
