import React, { useState } from "react";
import { Input } from "../../ui/Input/Input";
import { Button } from "../../ui/Button/Button";
import { Checkbox } from "../../ui/Checkbox/Checkbox";
import { Link, useNavigate } from "react-router-dom";
import styles from "./AuthForm.module.css";
import { UserService } from "../../services/UserService";
import { Dialog } from "../Dialog/Dialog";

export const AuthForm = ({ onSubmit }) => {
  const navigate = useNavigate();
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [remember, setRemember] = useState(false);

  const [dialogContent, setDialogContent] = useState("");

  const [isDialog, setIsDialog] = useState(false);
  const [typeDialog, setTypeDialog] = useState("");

  const [isLogined, setIsLogined] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault(); // отключаем перезагрузку страницы

    try {
      const response = await UserService.login({ login, password });

      if (!response.ok) {
        const errorJson = await response.json();
        console.log("ОШИБКА АВТОРИЗАЦИИ");
        console.log("ERROR JSON:", errorJson);

        let message = "Ошибка авторизации";
        if (errorJson.detail)
          message = errorJson.detail;

        // Диалоговое окно об предупреждение
        setTypeDialog("warning");
        setDialogContent(message);
        setIsDialog(true);

        return;//ошибка авторизации
      }
      
      // Диалоговое окно успешно
      setTypeDialog("ok");
      setDialogContent("Авторизация завершена успешно");
      setIsDialog(true);

      // Регистрация прошла успешно
      setIsLogined(true);

    }
    catch (err) {
      console.log("ОШИБКА СОЕДИНЕНИЯ С СЕРВЕРОМ")

      // Диалоговое окно об ошибке
      setTypeDialog("error");
      setDialogContent("Ошибка соединения с сервером");
      setIsDialog(true);

      return;//ошибка регистрации
    }
  };

  return (
    <>
      {isDialog && <Dialog type={typeDialog} content={dialogContent} onClose={() => { setIsDialog(false); isLogined && navigate("/main"); }} />}

      <form className={styles.form} onSubmit={handleSubmit}>
        <h2>Авторизация</h2>

        <Input
          label="Логин/Почта"
          type="login"
          value={login}
          onChange={setLogin}
          placeholder="Введите логин/почту"
          onBlur={() => { }}
        />
        <Input
          label="Пароль"
          type="password"
          value={password}
          onChange={setPassword}
          placeholder="Введите пароль"
          onBlur={() => { }}
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
    </>
  );
};
