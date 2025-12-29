import React from "react";
import styles from "./ServerButton.module.css";
import placeholder_server from "../../assets/placeholder_server.svg"
import { CircleButton } from "../../ui/CircleButton/CircleButton";

export const ServerButton = ({ onClick }) => {
  return (
    <CircleButton className={styles.serverButton} onClick={onClick}>
        <img src={placeholder_server} alt="" />
    </CircleButton>
  );
};