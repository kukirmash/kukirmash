import React from "react";
import styles from "./Checkbox.module.css";

export const Checkbox = ({ label, checked, onChange }) => {
  return (
    <label className={styles.checkbox}>
      <input
        type="checkbox"
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
      />
      <span className={styles.label}>{label}</span>
    </label>
  );
};
