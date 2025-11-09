import React from "react";
import styles from "./Logo.module.css";
import logo from "../../assets/kukirmash_logo.svg"

export const Logo = () => {
  return (
    <div className={styles.logo}>
      <img className={styles.logoSvg} src={logo} width="70" height="70" alt="kukirmash"></img>
      <text className={styles.logoText}> kukirmash </text>
    </div>
  );
};
