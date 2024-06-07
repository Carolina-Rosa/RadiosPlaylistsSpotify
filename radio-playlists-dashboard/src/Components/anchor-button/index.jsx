import React from "react";
import "./styles.scss";

export default function AnchorButton({ link, content, color }) {
    return (
        <a
            className={`anchor-button ${color}`}
            href={link}
            target="_blank"
            rel="noopener noreferrer"
        >
            {content}
        </a>
    );
}
