import React from "react";
import styles from "./RegisterPage.module.css";
import { RegisterForm } from "../../components/RegisterForm/RegisterForm";
import { Logo } from "../../components/Logo/Logo";

export const RegisterPage = () => {
  return (
    <div className={styles.page}>
      <div className={styles.container}>
        <Logo />
        <RegisterForm />
      </div>
    </div>
  );
};
