import React, { useState } from "react";
import { Input } from "../../ui/Input/Input";
import { Button } from "../../ui/Button/Button";
import { Checkbox } from "../../ui/Checkbox/Checkbox";
import { Link } from "react-router-dom";
import styles from "./AuthForm.module.css";

export const AuthForm = ({ onSubmit }) => {
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [remember, setRemember] = useState(false);

  const handleSubmit = (e) => {
    
  };

  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <h2>Авторизация</h2>

      <Input 
        label="Логин/Почта" 
        type="login" 
        value={login} 
        onChange={setLogin} 
        placeholder="Введите логин/почту"
      />
      <Input 
        label="Пароль" 
        type="password" 
        value={password} 
        onChange={setPassword} 
        placeholder="Введите пароль"
      />

      <div className={styles.underPassword} >
        <Checkbox 
          label="Запомнить меня" 
          checked={remember} 
          onChange={setRemember}
        />

        <a href="#">Забыли пароль?</a>
      </div>
      
      <Button type="submit">Войти</Button>

      <div className={styles.register} >
        Нет учётной записи?
        <Link to="/register" >Зарегистрироваться</Link>
      </div>

    </form>
  );
};
