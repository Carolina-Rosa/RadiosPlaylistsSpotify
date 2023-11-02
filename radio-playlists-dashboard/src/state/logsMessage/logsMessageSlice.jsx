import { createSlice } from "@reduxjs/toolkit";

const initialState = { logsList: [] };

export const logsMessageSlice = createSlice({
    name: "LogsMessage",
    initialState,
    reducers: {
        addLogs: (state, action) => {
            state.logsList = [action.payload, ...state.logsList];
        },
        addLogsToExistingRadio: (state, action) => {
            state.logsList[action.payload.radio].logsList = [
                action.payload.logsList,
                ...state.logsList[action.payload.radio].logsList
            ];
        }
    }
});

export const { addLogs, addLogsToExistingRadio } = logsMessageSlice.actions;

export default logsMessageSlice.reducer;
