import React from "react";
import "./styles.scss";
import AnchorButton from "../anchor-button";

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
                <AnchorButton
                    link={linkToSpotify}
                    content="Listen on Spotify"
                />
            </div>
        </div>
    );
}
