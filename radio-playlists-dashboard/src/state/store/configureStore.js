import { configureStore } from "@reduxjs/toolkit";
import logsMessageReducer from "../logsMessage/logsMessageSlice";
import timerMessageReducer from "../timerMessage/timerMessageSlice";
import playingNowMessageReducer from "../playingNowMessage/playingNowMessageSlice";
import signalRMiddleware from "../middlewares/signalRMiddleware";

export const store = configureStore({
    reducer: {
        LogsMessage: logsMessageReducer,
        TimerMessage: timerMessageReducer,
        PlayingNowMessage: playingNowMessageReducer
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(signalRMiddleware)
});
