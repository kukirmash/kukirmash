import React from "react";
import styles from "./Logo.module.css";
import logo from "../../assets/kukirmash_logo.svg";

export const Logo = ({
  size = "4rem",     // Единый размер для управления масштабом
  showText = true,   // Флаг: показывать текст или нет
  className = ""
}) => {

  return (
    <div className={`${styles.logo} ${className}`.trim()}>
      <img
        className={styles.logoSvg}
        src={logo}
        alt="kukirmash"
        style={{ width: size, height: size }} /* Задаем размер через переменную */
      />

      {showText && (
        <span
          className={styles.logoText}
          style={{ fontSize: `calc(${size} * 0.8)` }}/* Текст будет пропорционален размеру иконки */
        >
          kukirmash
        </span>
      )}
    </div>
  );
};