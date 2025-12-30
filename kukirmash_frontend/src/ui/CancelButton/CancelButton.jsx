import React from "react";
import styles from "./CancelButton.module.css";
import close_black from "../../assets/close_black.svg"

export const CancelButton = ({ onClick }) => {
  return (
    <button className = {styles.cancelButton} onClick={onClick}>
        <img className={styles.cancelSvg} src={close_black}/>
    </button>
  );
};