import React from "react";
import styles from "./SearchServerButton.module.css";
import { CircleButton } from "../../ui/CircleButton/CircleButton";

export const SearchServerButton = ({ onClick }) => {
  return (
    <CircleButton className={styles.searchServerButton} onClick={onClick}>
        <img src="" alt="поиск" />
    </CircleButton>
  );
};