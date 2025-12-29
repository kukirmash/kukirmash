import React from "react";
import { AuthForm } from "../../components/AuthForm/AuthForm";
import { Logo } from "../../ui/Logo/Logo";
import styles from "./LoginPage.module.css";

export const LoginPage = () => {

  const handleLogin = async (data) => {

  };

  return (
    <div className={styles.page}>
      <div className={styles.container}>
        <Logo />
        <AuthForm onSubmit={handleLogin} />
      </div>
    </div>
  );
};
