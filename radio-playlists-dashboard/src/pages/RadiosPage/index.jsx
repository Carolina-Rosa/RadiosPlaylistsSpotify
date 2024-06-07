import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

import "./styles.scss";
import AnchorButton from "../../Components/anchor-button";

export default function RadiosPage() {
    let params = useParams();
    console.log(params.name);

    const [playlist, setPlaylist] = useState({
        name: `Listening ${params.name}`,
        images: [
            {
                url: "https://images.pexels.com/photos/4110334/pexels-photo-4110334.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            }
        ],
        external_urls: {
            spotify: "https://open.spotify.com/playlist/4c61sN9HN8fQCx64ek6AAx"
        },
        description:
            "Automatically updated playlist from what is playing in Antena 3",
        followers: {
            total: 0
        },
        tracksTotal: 360
    });

    useEffect(() => {
        axios
            .get(`https://localhost:7270/api/Playlist/${params.name}`)
            .then((res) => {
                setPlaylist(res.data);
            });
    });

    return (
        <div className="radios-page">
            <h1 className="radios-title">{params.name}</h1>
            <div className="playlist-section">
                <div className="playlist-info">
                    <div>
                        <h1>{playlist.name}</h1>
                        <p>{playlist.description}</p>
                        <p>
                            <span style={{ fontWeight: "800" }}>
                                Followers:{" "}
                            </span>{" "}
                            {playlist.followers.total}
                        </p>
                        <p>
                            <span style={{ fontWeight: "800" }}>Tracks: </span>{" "}
                            {playlist.tracksTotal}
                        </p>
                    </div>
                    <AnchorButton
                        color="green-button"
                        link={playlist.external_urls.spotify}
                        content="Listen on Spotify"
                    />
                </div>
                <div className="playlist-image-section">
                    <img
                        className="playlist-image"
                        src={playlist.images[0].url}
                        alt="Playlist cover"
                    />
                </div>
            </div>
            <br />
        </div>
    );
}
