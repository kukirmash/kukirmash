import React from "react";
import styles from "./Input.module.css";

export const Input = ({ label, type = "text", value, onChange, placeholder, onBlur }) => (
  <div className={styles.wrapper}>
    {label && <label className={styles.label}>{label}</label>}
    <input
      className={styles.input}
      type={type}
      value={value}
      onChange={(e) => onChange(e.target.value)}
      placeholder={placeholder}
      onBlur={(e)=> onBlur(e.target.value)}
    />
  </div>
);