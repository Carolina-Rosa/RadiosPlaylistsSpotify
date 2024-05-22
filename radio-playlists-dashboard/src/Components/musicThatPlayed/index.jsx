import React from "react";
import "./styles.scss";

export default function MusicThatPlayed({
    songName,
    songArtist,
    linkToSpotify,
    image
}) {
    return (
        <div className="music-that-played-container">
            <div className="image">
                <img src={image} alt="album cover" />
            </div>

            <div className="music-info">
                <h2>{songName}</h2>
                <p>{songArtist}</p>
                <a
                    className="button-to-spotify"
                    href={linkToSpotify}
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    Listen on Spotify
                </a>
            </div>
        </div>
    );
}
