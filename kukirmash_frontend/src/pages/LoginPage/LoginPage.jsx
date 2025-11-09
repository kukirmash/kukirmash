import React, { useState } from "react";
import { AuthForm } from "../../components/AuthForm/AuthForm";
import { Logo } from "../../components/Logo/Logo";
import styles from "./LoginPage.module.css";

export const LoginPage = () => {
  const [error, setError] = useState(null);

  const handleLogin = async (data) => {
    // setError(null);
    // try {
    //   const res = await login(data);
    //   console.log("Успешный вход:", res);
    //   // Здесь можно добавить переход на другую страницу
    // } catch (err) {
    //   setError(err.message);
    // }
  };

  return (
    <div className={styles.page}>
      <div className={styles.container}>
        {/* Логотип над формой */}
        <Logo />
        <AuthForm onSubmit={handleLogin} />
        {error && <p className={styles.error}>{error}</p>}
      </div>
    </div>
  );
};
