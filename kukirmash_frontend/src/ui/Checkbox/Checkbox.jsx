import React from "react";
import styles from "./Checkbox.module.css";

export const Checkbox = ({
  label, 
  checked, 
  onChange = () => {},
  className = "",
  disabled = false,
 }) => {

    // Собираем строку всех классов: базовый, вариант, дизаблед, внешний класс
  const checkboxClasses = [
    styles.container,
    className
  ].join(" ").trim();

  return (
    <label className={checkboxClasses}>
      <input
        className={styles.checkbox}
        type="checkbox"
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
        disabled = {disabled}
      />
      {label && <span className={styles.label}>{label}</span>}
    </label>
  );
};
