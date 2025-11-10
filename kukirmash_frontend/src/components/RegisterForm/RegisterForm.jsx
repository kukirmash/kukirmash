import React, { useState } from "react";
import styles from "./RegisterForm.module.css";
import { Input } from "../../ui/Input/Input";
import { Button } from "../../ui/Button/Button";
import { Link } from "react-router-dom";

export const RegisterForm = () => {
  const [login, setLogin] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [error, setError] = useState("");

  const handleSubmit = (e) => {

  };

  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <h2>Регистрация</h2>

      <Input
        label="Логин"
        value={login}
        onChange={setLogin}
        placeholder="Введите логин"
      />
      <Input
        label="Почта"
        type="email"
        value={email}
        onChange={setEmail}
        placeholder="Введите почту"
      />
      <Input
        label="Пароль"
        type="password"
        value={password}
        onChange={setPassword}
        placeholder="Введите пароль"
      />
      <Input
        label="Повторите пароль"
        type="password"
        value={confirmPassword}
        onChange={setConfirmPassword}
        placeholder="Повторите пароль"
      />

      {error && <p className={styles.error}>{error}</p>}

      <Button type="submit">Зарегистрироваться</Button>

      <div className={styles.login} >
        Уже есть аккаунт?
        <Link to="/login">Войти</Link>
      </div>

    </form>
  );
};