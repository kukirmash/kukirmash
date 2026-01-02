import React from "react";
import styles from "./CircleButton.module.css";

export const CircleButton = ({ 
  children, 
  onClick, 
  className = "",
}) => {
  return (
    <button 
      type="button"
      className={`${styles.circleButton} ${className}`.trim()} 
      onClick={onClick}
    >
      {children}
    </button>
  );
};