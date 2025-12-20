import React, { useState } from "react";
import styles from "./RegisterForm.module.css";
import { Input } from "../../ui/Input/Input";
import { Button } from "../../ui/Button/Button";
import { Link, useNavigate } from "react-router-dom";
import { UserService } from "../../services/UserService";
import { Dialog } from "../Dialog/Dialog";

export const RegisterForm = () => {
  //*----------------------------------------------------------------------------------------------------------------------------
  const navigate = useNavigate();
  const [login, setLogin] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [errors, setErrors] = useState({});
  const [serverMessage, setServerMessage] = useState("");

  const [isDialog, setIsDialog] = useState(false);
  const [typeDialog, setTypeDialog] = useState("");

  const [isRegistered, setIsRegistered] = useState(false);

  //*----------------------------------------------------------------------------------------------------------------------------
  // Проверка перед отправкой запроса (onSubmit у формы)
  const validate = () => {
    const newErrors = {};

    if (!login.trim())
      newErrors.login = "Логин обязателен";

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email))
      newErrors.email = "Некорректная почта";

    if (password.length < 8)
      newErrors.password = "Пароль должен быть минимум 8 символов";

    if (password !== confirmPassword)
      newErrors.confirmPassword = "Пароли не совпадают";

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  //*----------------------------------------------------------------------------------------------------------------------------
  const validateField = (name, value) => {
    const fieldErrors = {
      login: "",
      email: "",
      password: "",
      confirmPassword: ""
    };

    if (name === "login" && !value.trim()) {
      fieldErrors.login = "Логин обязателен";
    }

    if (name === "email") {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(value))
        fieldErrors.email = "Некорректная почта";
    }

    if (name === "password" && value.length < 8) {
      fieldErrors.password = "Пароль должен быть минимум 8 символов";
    }

    if (name === "confirmPassword" && value !== password) {
      fieldErrors.confirmPassword = "Пароли не совпадают";
    }

    setErrors(prev => ({ ...prev, [name]: fieldErrors[name] }));
  };

  //*----------------------------------------------------------------------------------------------------------------------------
  // TODO: делать кнопку регистрации неактивной, пока все параметры не станут валидными 
  // TODO: одновременно валидацию поля пароля и подтверждения пароля
  // TODO: ??? - запрашивать логин и email у всех пользователей заранее, чтобы проверять на уникальность - ??? - не срочно
  const handleSubmit = async (e) => {
    e.preventDefault(); // отключаем перезагрузку страницы
    setServerMessage("");

    if (!validate())
      return;

    try {
      const response = await UserService.register({ login, email, password });

      if (!response.ok) {
        const errorJson = await response.json();
        console.log("ОШИБКА РЕГИСТРАЦИИ");
        console.log("ERROR JSON:", errorJson);

        let message = "Ошибка регистрации";
        if (errorJson.detail)
          message = errorJson.detail;

        // Диалоговое окно об предупреждение
        setTypeDialog("warning");
        setServerMessage(message);
        setIsDialog(true);

        return;//ошибка регистрации
      }
      
      // Диалоговое окно успешно
      setTypeDialog("ok");
      setServerMessage("Регистрация завершена успешно");
      setIsDialog(true);

      // Регистрация прошла успешно
      setIsRegistered(true);
    }
    catch (err) {
      console.log("ОШИБКА СОЕДИНЕНИЯ С СЕРВЕРОМ")

      // Диалоговое окно об ошибке
      setTypeDialog("error");
      setServerMessage("Ошибка соединения с сервером");
      setIsDialog(true);

      return;//ошибка регистрации
    }
  };

  //*----------------------------------------------------------------------------------------------------------------------------
  return (
    <>
      {isDialog && <Dialog type = {typeDialog} content={serverMessage} onClose={() => { setIsDialog(false); isRegistered && navigate("/login"); }} />}

      <form className={styles.form} onSubmit={handleSubmit}>
        <h2>Регистрация</h2>

        <Input
          label="Логин*"
          value={login}
          onChange={setLogin}
          placeholder="Введите логин"
          onBlur={() => validateField("login", login)}
        />
        {errors.login && <p className={styles.error}>{errors.login}</p>}
        <Input
          label="Почта*"
          type="text"
          value={email}
          onChange={setEmail}
          placeholder="Введите почту"
          onBlur={() => validateField("email", email)}
        />
        {errors.email && <p className={styles.error}>{errors.email}</p>}
        <Input
          label="Пароль*"
          type="password"
          value={password}
          onChange={setPassword}
          placeholder="Введите пароль"
          onBlur={() => validateField("password", password)}
        />
        {errors.password && <p className={styles.error}>{errors.password}</p>}
        <Input
          label="Повторите пароль*"
          type="password"
          value={confirmPassword}
          onChange={setConfirmPassword}
          placeholder="Повторите пароль"
          onBlur={() => validateField("confirmPassword", confirmPassword)}
        />
        {errors.confirmPassword && (<p className={styles.error}>{errors.confirmPassword}</p>)}

        <Button type="submit">Зарегистрироваться</Button>

        <div className={styles.login} >
          Уже есть аккаунт?
          <Link to="/login">Войти</Link>
        </div>

      </form>
    </>

  );

  //*----------------------------------------------------------------------------------------------------------------------------
};