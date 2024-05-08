import { createSlice } from "@reduxjs/toolkit";

const initialState = { value: "84" };

export const timerMessageSlice = createSlice({
    name: "TimerMessage",
    initialState,
    reducers: {
        setCountdown: (state, action) => {
            state.value = action.payload;
        }
    }
});

export const { setCountdown } = timerMessageSlice.actions;

export default timerMessageSlice.reducer;
