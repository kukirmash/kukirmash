import React from "react";
import styles from "./CancelButton.module.css";
import close from "../../assets/close.svg"

export const CancelButton = ({ onClick }) => {
  return (
    <button className = {styles.cancelButton} onClick={onClick}>
        <img className={styles.cancelSvg} src={close}/>
    </button>
  );
};