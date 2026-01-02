import React from "react";
import styles from "./Button.module.css";

export const Button = ({
  children,
  onClick,
  type = "button",  // submit - применить форму, reset - сбростиь форму
  variant = "primary",
  disabled = false, 
  className = "",   // внешние классы
  ...props
}) => {

  // Собираем строку всех классов: базовый, вариант, внешний класс
  const buttonClasses = [
    styles.button,
    styles[variant],
    className
  ].join(" ").trim();

  return (
    <button 
      className={buttonClasses} 
      onClick={onClick} 
      type={type}
      disabled = {disabled}
      {...props}
    >
      
      {children}

    </button>
  );
};