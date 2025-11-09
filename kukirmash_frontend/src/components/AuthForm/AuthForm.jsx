import React, { useState } from "react";
import { Input } from "../../ui/Input/Input";
import { Button } from "../../ui/Button/Button";
import { Checkbox } from "../../ui/Checkbox/Checkbox";
import styles from "./AuthForm.module.css";

export const AuthForm = ({ onSubmit }) => {
  const [login, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [remember, setRemember] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ login, password });
  };

  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <h2>Вход в учетную запись</h2>

      <Input label="Логин/Почта" type="login" value={login} onChange={setEmail} placeholder="Введите логин/почту" />
      <Input label="Пароль" type="password" value={password} onChange={setPassword} placeholder="Введите пароль" />

      <div className={styles.underPassword} >
        <Checkbox label="Запомнить меня" checked={remember} onChange={setRemember}/>
        <a href="#">Забыли пароль?</a>
      </div>
      
      <Button type="submit">Войти</Button>
      <div className={styles.register} >
        Нет учётной записи?<a href="#" >Зарегистрироваться</a>
      </div>
    </form>
  );
};
