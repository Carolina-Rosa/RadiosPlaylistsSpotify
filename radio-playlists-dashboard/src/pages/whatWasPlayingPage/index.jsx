import React, { useState } from "react";

import "./styles.scss";
import MusicThatPlayed from "../../Components/musicThatPlayed";
import WWPlayingForm from "../../Components/WWPlayingForm";

export default function WhatWasPlayingPage() {
    const [song, setSong] = useState([]);

    const getArtistsInString = (artists) => {
        if (artists.length === 1) {
            return artists[0].name;
        } else {
            var artistsList = "";
            for (var i = 0; i < artists.length - 1; i++) {
                artistsList += artists[i].name + ", ";
            }
            artistsList += artists[artists.length - 1].name;
            return artistsList;
        }
    };

    return (
        <div className="ww-playing-page">
            <h1 className="wwplaying-title">What was playing?</h1>
            <div className="ww-playing-body">
                <WWPlayingForm setSong={setSong} />
                {song?.length === 0 ? (
                    <div></div>
                ) : (
                    <div className="show-music-section">
                        {song ? (
                            <MusicThatPlayed
                                songName={song.name}
                                songArtist={getArtistsInString(song.artists)}
                                linkToSpotify={song.external_urls.spotify}
                                image={song.album.images[0].url}
                            />
                        ) : (
                            <p>No Music Playing</p>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}
