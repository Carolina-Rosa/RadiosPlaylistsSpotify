import React from "react";
import TopItem from "../../Components/topItem";
import "./styles.scss";

export default function Top5({ title, content }) {
    return (
        <div className="top-5">
            <h1>{title}</h1>
            <div className="tops-section">
                {content.map((artist, i) => (
                    <TopItem
                        key={i}
                        itemNo={i + 1}
                        timesPlayed={artist.timesPlayed}
                        songName={artist.songName ?? artist.artistName}
                        songArtist={artist.songName ? artist.artistName : ""}
                    />
                ))}
            </div>
        </div>
    );
}
