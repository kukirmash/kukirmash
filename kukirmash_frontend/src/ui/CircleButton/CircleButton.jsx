import React from "react";
import styles from "./CircleButton.module.css";

export const CircleButton = ({ children, onClick, className }) => {
  return (
    <button className={`${styles.circleButton} ${className || ""}`} onClick={onClick}>
        {children}
    </button>
  );
};