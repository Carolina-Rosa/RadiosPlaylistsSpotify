import React from "react";
import RadioComponent from "../radioComponent";
import "./styles.scss";

export default function RadioSection({ setRadio, radio, radiosList }) {
    const something = () => {
        console.log("add radio");
    };

    return (
        <div className="radio-section">
            {radiosList.map((radioObj, i) => (
                <RadioComponent
                    key={i}
                    radioName={radioObj.radio}
                    songTitle={radioObj.music}
                    songArtist={radioObj.artist}
                    isSelected={radioObj.radio === radio}
                    setRadio={setRadio}
                />
            ))}
            <div className="add-radio-button" onClick={something}>
                <p>+ Add Radio</p>
            </div>
        </div>
    );
}
