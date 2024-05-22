import React, { useEffect, useState } from "react";
import axios from "axios";

import "./styles.scss";
import MusicThatPlayed from "../../Components/musicThatPlayed";

export default function WhatWasPlayingPage() {
    const [radios, setRadios] = useState([]);
    const [song, setSong] = useState([]);
    const [showMusicCard, setShowMusicCard] = useState(false);
    const [selectedRadio, setSelectedRadio] = useState("Antena3");
    const [selectedDateTime, setSelectedDateTime] = useState("");

    useEffect(() => {
        axios.get(`https://localhost:7270/api/radios/`).then((res) => {
            setRadios(res.data);
        });
    }, []);

    const getWhatWasPlaying = async (event) => {
        event.preventDefault();
        const res = await axios.get(
            `https://localhost:7270/api/musicSpotify/${selectedRadio}/${selectedDateTime}:00.000+00:00`
        );

        setSong(res.data);
        setShowMusicCard(true);
    };

    const handleSelectedRadioChange = (e) => {
        console.log(selectedRadio);
        setSelectedRadio(e.currentTarget.value);
    };

    const handleChangeDate = (e) => {
        console.log(selectedDateTime);
        setSelectedDateTime(e.currentTarget.value);
    };

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
                <div className="form-section">
                    <form className="form-ww-playing">
                        <label>
                            Radio:
                            <select
                                value={selectedRadio}
                                onChange={handleSelectedRadioChange}
                            >
                                {radios.map((radioObj, i) => (
                                    <option key={i} value={radioObj.name}>
                                        {radioObj.displayName}
                                    </option>
                                ))}
                            </select>
                        </label>
                        <label>
                            Date & time:
                            <input
                                type="datetime-local"
                                name="name"
                                onChange={handleChangeDate}
                            />
                        </label>
                        <input
                            className="form-button"
                            type="submit"
                            value="What was playing?"
                            onClick={getWhatWasPlaying}
                        />
                    </form>
                </div>
                {showMusicCard ? (
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
                ) : (
                    <div></div>
                )}
            </div>
        </div>
    );
}
