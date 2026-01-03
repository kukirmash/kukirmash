import React, { useState, useEffect } from "react"
import { Link, useNavigate } from "react-router-dom"

import { Input } from "../../ui/Input/Input"
import { Button } from "../../ui/Button/Button"
import { Dialog } from "../../ui/Dialog/Dialog"
import { AuthCard } from "../../ui/AuthCard/AuthCard"

import styles from "./RegisterForm.module.css"
import { UserService } from "../../services/UserService"
import { useAuthValidation } from "../../hooks/useAuthValidation"

export const RegisterForm = () => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const navigate = useNavigate()

	//*----------------------------------------------------------------------------------------------------------------------------
	const {
		values: registerData,
		errors,
		isValid,
		handleChange,
		validateField,
		validateAllFields, //проверка всех полей
	} = useAuthValidation({
		login: "",
		email: "",
		password: "",
		confirmPassword: "",
	})

	const [dialog, setDialog] = useState({
		isOpen: false,
		type: "ok",
		content: "",
	})

	const [isSuccess, setIsSuccess] = useState(false)

	//*----------------------------------------------------------------------------------------------------------------------------
	// Обработка форма после нажатия кнопки
	const handleSubmit = async (e) => {
		e.preventDefault()

		// Финальная проверка перед отправкой
		if (!validateAllFields()) return

		try {
			const response = await UserService.register({
				login: registerData.login,
				email: registerData.email,
				password: registerData.password,
			})

			if (!response.ok) {
				const errorJson = await response.json()

				setDialog({
					isOpen: true,
					type: "error",
					content: errorJson.detail || "Ошибка регистрации",
				})

				return
			}

			setIsSuccess(true)
			setDialog({
				isOpen: true,
				type: "ok",
				content: "Регистрация выполнена успешна",
			})
		} catch (err) {
			setDialog({
				isOpen: true,
				type: "error",
				content: "Неизвестная ошибка",
			})
		}
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	const closeDialog = () => {
		setDialog({ ...dialog, isOpen: false })

		if (isSuccess) navigate("/login")
	}

	//*----------------------------------------------------------------------------------------------------------------------------
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
