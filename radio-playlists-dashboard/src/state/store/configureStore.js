import { configureStore } from "@reduxjs/toolkit";
import logsReducer from "../logs/logsSlice";
import timerMessageReducer from "../timerMessage/timerMessageSlice";
import signalRMiddleware from "../middlewares/signalRMiddleware";

export const store = configureStore({
    reducer: {
        Logs: logsReducer,
        TimerMessage: timerMessageReducer
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(signalRMiddleware)
});
