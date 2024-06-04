import React, { useEffect, useState } from "react";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { TimePicker } from "@mui/x-date-pickers/TimePicker";
import dayjs from "dayjs";
import axios from "axios";

import "./styles.scss";

export default function WWPlayingForm({ setSong }) {
    const [radios, setRadios] = useState([]);
    const [selectedRadio, setSelectedRadio] = useState("Antena3");
    const [selectedDate, setSelectedDate] = useState(dayjs());
    const [selectedTime, setSelectedTime] = useState(dayjs());

    useEffect(() => {
        axios.get(`https://localhost:7270/api/radios/`).then((res) => {
            setRadios(res.data);
        });
    }, []);

    const getWhatWasPlaying = async (event) => {
        event.preventDefault();

        console.log(
            dayjs(selectedDate).format("YYYY-MM-DD") +
                "++++" +
                dayjs(selectedTime).format("HH:mm")
        );
        const res = await axios.get(
            `https://localhost:7270/api/musicSpotify/${selectedRadio}/
            ${dayjs(selectedDate).format("YYYY-MM-DD")}
            ${dayjs(selectedTime).format("HH:mm")}:00.000+00:00`
        );

        setSong(res.data);
    };

    const handleSelectedRadioChange = (e) => {
        console.log(selectedRadio);
        setSelectedRadio(e.currentTarget.value);
    };

    return (
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
                    Date:
                    <DatePicker
                        format="YYYY-MM-DD"
                        value={selectedDate}
                        onChange={(newValue) => {
                            setSelectedDate(newValue);
                        }}
                    />
                </label>
                <label>
                    Time:
                    <TimePicker
                        format="HH:mm"
                        ampm={false}
                        value={selectedTime}
                        onChange={(newValue) => {
                            setSelectedTime(newValue);
                        }}
                    />
                </label>
                {/*  <input
                            className="form-button"
                            type="submit"
                            value="What was playing?"
                            onClick={getWhatWasPlaying}
                        /> */}
            </form>
        </div>
    );
}
