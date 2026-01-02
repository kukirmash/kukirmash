import React from "react";
import styles from "./CancelButton.module.css";
import close_black from "../../assets/close_black.svg"

export const CancelButton = ({ 
  onClick, 
  className = "", 
}) => {
  
    // Собираем строку всех классов: базовый, внешний класс
    const buttonClasses = [
      styles.cancelButton,
      className
    ].join(" ").trim();
  
  return (
    <button 
      className={buttonClasses}
      onClick={onClick}
      type = "button"     // всегда button
    >
      
        <img 
          className={styles.closeSvg} 
          src={close_black} 
          alt="Закрыть"/>

    </button>
  );
};