import React, { useState } from "react"
import { Link, useNavigate } from "react-router-dom"

import { Input } from "../../ui/Input/Input"
import { Button } from "../../ui/Button/Button"
import { Checkbox } from "../../ui/Checkbox/Checkbox"
import { Dialog } from "../../ui/Dialog/Dialog"
import { AuthCard } from "../../ui/AuthCard/AuthCard"

import styles from "./LoginForm.module.css"
import { UserService } from "../../services/UserService"
import { useForm } from "../../hooks/useForm"

export const LoginForm = () => {
	//----------------------------------------------------------------------------------------------------------------------------
	// Правила валидации для формы логина
	const validateLoginForm = (values) => {
		const errors = {}
		if (!values.login.trim())
			errors.login = "Логин обязателен"

		if (!values.password)
			errors.password = "Пароль обязателен"
		return errors
	}

	//----------------------------------------------------------------------------------------------------------------------------
	// Инициализируем хук
	const {
		values: loginData,
		errors,
		isValid,
		handleChange,
		validateField,
		validateAllFields,
	} = 
	useForm(
		{
			login: "",
			password: "",
			remember: false
		},
		validateLoginForm)

	//----------------------------------------------------------------------------------------------------------------------------
	const [dialog, setDialog] = useState({
		isOpen: false,
		type: "ok",
		content: "",
	})

	const [isSuccess, setIsSuccess] = useState(false)
	const navigate = useNavigate()
	//----------------------------------------------------------------------------------------------------------------------------
	const handleSubmit = async (e) => {
		e.preventDefault()

		// Финальная проверка перед отправкой
		if (!validateAllFields()) return

		try {
			const response = await UserService.login({
				login: loginData.login,
				password: loginData.password,
			})

			const status = response.status
			let errorMessage = "Неизвестная ошибка при авторизации"

			switch (status) {
				case 200:
					setIsSuccess(true)
					setDialog({
						isOpen: true, // true - убрал
						type: "ok",
						content: "Вход выполнен успешно",
					})
					return

				case 400: // Bad Request
					const errorJson = await response.json()
					errorMessage = "Неверный формат"
					// Проверяем, пришел ли именно ValidationProblem с полем errors
					if (errorJson.errors) {
						errorMessage = Object.values(errorJson.errors)
							.flat()
							.join("\n")
					}
					break

				case 404: // Not Found
				case 409: // Conflict
					errorMessage = await response.text()
					break

				default:
					break
			}

			setIsSuccess(false)
			setDialog({
				isOpen: true,
				type: "error",
				content: errorMessage,
			})

			// TODO: Логика "Запомнить меня"
		} catch (err) {
			// Ошибка сети (интернет пропал или сервер лежит полностью)
			setDialog({
				isOpen: true,
				type: "error",
				content: "Не удалось соединиться с сервером",
			})
		}
	}

	//----------------------------------------------------------------------------------------------------------------------------
	const closeDialog = () => {
		setDialog({ ...dialog, isOpen: false })

		if (isSuccess)
			navigate("/main")
	}

	//----------------------------------------------------------------------------------------------------------------------------
	return (
		<>
			{dialog.isOpen && (
				<Dialog
					type={dialog.type}
					content={dialog.content}
					onClose={closeDialog}
				/>
			)}

			<AuthCard
				title="Авторизация"
				footer={
					<div>
						Нет учётной записи?{" "}
						<Link to="/register">Зарегистрироваться</Link>
					</div>
				}
			>
				<form onSubmit={handleSubmit} className={styles.form}>
					<Input
						label="Логин/Почта"
						value={loginData.login}
						onChange={(val) => handleChange("login", val)}
						onBlur={() => validateField("login")}
						placeholder="Введите логин/почту"
						errorText={errors.login}
						name="username"
						autoComplete="username"
					/>

					<Input
						label="Пароль"
						type="password"
						value={loginData.password}
						onChange={(val) => handleChange("password", val)}
						onBlur={() => validateField("password")}
						placeholder="Введите пароль"
						errorText={errors.password}
						name="password"
						autoComplete="current-password"
					/>

					<div className={styles.rowBetween}>
						<Checkbox
							label="Запомнить меня"
							checked={loginData.remember}
							onChange={(val) => handleChange("remember", val)}
						/>
						<a href="#" className={styles.forgotLink}>
							Забыли пароль?
						</a>
					</div>

					<Button
						type="submit"
						disabled={!isValid}
						className={styles.submitButton}
					>
						Войти
					</Button>
				</form>
			</AuthCard>
		</>
	)
}
