import { createSlice } from "@reduxjs/toolkit";

const initialState = { radiosList: [] };

export const playingNowMessageSlice = createSlice({
    name: "PlayingNowMessage",
    initialState,
    reducers: {
        addRadios: (state, action) => {
            state.radiosList = [action.payload, ...state.radiosList];
        },
        addPlayingNowToExistingRadio: (state, action) => {
            state.radiosList[action.payload.radio].music = action.payload.music;
            state.radiosList[action.payload.radio].artist =
                action.payload.artist;
        }
    }
});

export const { addRadios, addPlayingNowToExistingRadio } =
    playingNowMessageSlice.actions;

export default playingNowMessageSlice.reducer;
