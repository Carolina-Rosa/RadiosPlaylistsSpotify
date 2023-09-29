import React from "react";
import "./styles.scss";
import { Link } from "react-router-dom";

export default function Header() {
    return (
        <div className="header">
            <div className="left"></div>
            <div className="right">
                <ul>
                    <li>
                        <Link to="/">Logs</Link>
                    </li>
                    <li>
                        <Link to="/stats">Statistics</Link>
                    </li>
                    <li>
                        <Link to="/something">Something else</Link>
                    </li>
                </ul>
            </div>
        </div>
    );
}
