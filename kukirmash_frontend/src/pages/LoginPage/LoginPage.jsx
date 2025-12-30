import React from "react";
import { LoginForm } from "../../components/LoginForm/LoginForm";
import { Logo } from "../../ui/Logo/Logo";
import styles from "./LoginPage.module.css";

export const LoginPage = () => {

  const handleLogin = async (data) => {

  };

  return (
    <div className={styles.page}>
      <div className={styles.container}>
        <Logo />
        <LoginForm onSubmit={handleLogin} />
      </div>
    </div>
  );
};
