import React from "react";
import styles from "./Button.module.css";

export const Button = ({ children, onClick, type = "button", variant = "primary" }) => {
  return (
    <button className={`${styles.button} ${styles[variant]}`} onClick={onClick} type={type}>
      {children}
    </button>
  );
};