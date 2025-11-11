import React from "react";
import styles from "./Logo.module.css";
import logo from "../../assets/kukirmash_logo.svg"

export const Logo = ({imgSize = 70, fontSize = 48}) => {
  return (
    <div className={styles.logo}>
      <img className={styles.logoSvg} src={logo} width={imgSize} height={imgSize} alt="kukirmash"></img>
      <span className={styles.logoText } style={{ fontSize: `${fontSize}px` }}><text> kukirmash </text></span>
    </div>
  );
};
