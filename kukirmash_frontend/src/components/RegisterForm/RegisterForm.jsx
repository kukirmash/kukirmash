import React, { useState, useEffect } from "react"
import { Link, useNavigate } from "react-router-dom"

import { Input } from "../../ui/Input/Input"
import { Button } from "../../ui/Button/Button"
import { Dialog } from "../../ui/Dialog/Dialog"
import { AuthCard } from "../../ui/AuthCard/AuthCard"

import styles from "./RegisterForm.module.css"
import { UserService } from "../../services/UserService"
import { useForm } from "../../hooks/useForm"

export const RegisterForm = () => {
	//----------------------------------------------------------------------------------------------------------------------------
	// Правила для регистрации
	const validateRegister = (values) => {
		const errors = {}
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

		if (values.login.length < 4) 
			errors.login = "Минимум 4 символа"
		
		if (!values.email.trim() || !emailRegex.test(values.email)) {
			errors.email = "Некорректная почта"
		}

		if (values.password.length < 8) {
			errors.password = "Минимум 8 символов"
		}

		if (values.password !== values.confirmPassword) {
			errors.confirmPassword = "Пароли не совпадают"
		}

		return errors
	}

	//----------------------------------------------------------------------------------------------------------------------------
	// Инициализируем хук
	const {
		values: registerData,
		errors,
		isValid,
		handleChange,
		validateField,
		validateAllFields,
	} =
		useForm(
			{
				login: "",
				email: "",
				password: "",
				confirmPassword: "",
			},
			validateRegister
		)

	//----------------------------------------------------------------------------------------------------------------------------
	const [dialog, setDialog] = useState({
		isOpen: false,
		type: "ok",
		content: "",
	})

	const navigate = useNavigate()
	const [isSuccess, setIsSuccess] = useState(false)

	//----------------------------------------------------------------------------------------------------------------------------
	// Обработка форма после нажатия кнопки
	const handleSubmit = async (e) => {
		e.preventDefault()

		// Финальная проверка перед отправкой
		if (!validateAllFields()) 
			return

		try {
			const response = await UserService.register({
				login: registerData.login,
				email: registerData.email,
				password: registerData.password,
			})

			const status = response.status
			let errorMessage = "Неизвестная ошибка при регистрации"

			switch (status) {
				case 201: // Успешная регистрация (Results.Created)
					setIsSuccess(true)
					setDialog({
						isOpen: true,
						type: "ok",
						content: "Регистрация выполнена успешно",
					})
					return // Прерываем выполнение, чтобы не сработал код ошибки ниже

				case 400: // Ошибки валидации (Results.ValidationProblem)
					const errorJson = await response.json()
					errorMessage = "Неверный формат данных"
					// Достаем массив ошибок из поля errors
					if (errorJson.errors) {
						errorMessage = Object.values(errorJson.errors)
							.flat()
							.join("\n")
					}
					break

				case 409: // Конфликт (Results.Conflict) - логин/email уже заняты
					errorMessage = await response.text()
					break

				case 500: // Серверная ошибка (Results.Problem)
					errorMessage = "Произошла внутренняя ошибка на сервере"
					break

				default:
					break
			}

			// Если код дошел сюда (не сработал return в case 201), значит была ошибка
			setIsSuccess(false)
			setDialog({
				isOpen: true,
				type: "error",
				content: errorMessage,
			})

		} catch (err) {
			// Ошибка сети (сервер выключен, нет интернета)
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
			navigate("/login")
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
				title="Регистрация"
				footer={
					<div>
						Уже есть аккаунт?<Link to="/login">Войти</Link>
					</div>
				}
			>
				<form onSubmit={handleSubmit} className={styles.form}>
					<Input
						label="Логин*"
						value={registerData.login}
						onChange={(val) => handleChange("login", val)}
						onBlur={() => validateField("login")}
						placeholder="Придумайте логин"
						errorText={errors.login}
						name="username"
						autoComplete="username"
					/>
					<Input
						label="Почта*"
						type="email"
						value={registerData.email}
						onChange={(val) => handleChange("email", val)}
						onBlur={() => validateField("email")}
						placeholder="example@mail.com"
						errorText={errors.email}
						name="email"
						autoComplete="email"
					/>
					<Input
						label="Пароль*"
						type="password"
						value={registerData.password}
						onChange={(val) => handleChange("password", val)}
						onBlur={() => validateField("password")}
						placeholder="Минимум 8 символов"
						errorText={errors.password}
						name="new-password"
						autoComplete="new-password"
					/>
					<Input
						label="Повторите пароль*"
						type="password"
						value={registerData.confirmPassword}
						onChange={(val) => handleChange("confirmPassword", val)}
						onBlur={() => validateField("confirmPassword")}
						placeholder="Повторите пароль"
						errorText={errors.confirmPassword}
						name="confirm-password"
						autoComplete="new-password"
					/>

					<Button
						type="submit"
						disabled={!isValid}
						className={styles.submitButton}
					>
						Зарегистрироваться
					</Button>
				</form>
			</AuthCard>
		</>
	)
}
