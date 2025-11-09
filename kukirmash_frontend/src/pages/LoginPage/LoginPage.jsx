import React, { useState } from "react";
import { AuthForm } from "../../components/AuthForm/AuthForm";
import { Logo } from "../../components/Logo/Logo";
import styles from "./LoginPage.module.css";

export const LoginPage = () => {
  const [error, setError] = useState(null);

  const handleLogin = async (data) => {

  };

  return (
    <div className={styles.page}>
      <div className={styles.container}>
        <Logo />
        <AuthForm onSubmit={handleLogin} />
        {error && <p className={styles.error}>{error}</p>}
      </div>
    </div>
  );
};
