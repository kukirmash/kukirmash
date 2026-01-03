import React, { useState } from "react"
import { Link, useNavigate } from "react-router-dom"

import { Input } from "../../ui/Input/Input"
import { Button } from "../../ui/Button/Button"
import { Checkbox } from "../../ui/Checkbox/Checkbox"
import { Dialog } from "../../ui/Dialog/Dialog"
import { AuthCard } from "../../ui/AuthCard/AuthCard"

import styles from "./LoginForm.module.css"
import { UserService } from "../../services/UserService"
import { useAuthValidation } from "../../hooks/useAuthValidation"

export const LoginForm = () => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const navigate = useNavigate()

	//*----------------------------------------------------------------------------------------------------------------------------
	const {
		values: loginData,
		errors,
		isValid,
		handleChange,
		validateField,
		validateAllFields,
	} = useAuthValidation({
		login: "",
		password: "",
	})

	const [dialog, setDialog] = useState({
		isOpen: false,
		type: "ok",
		content: "",
	})
	const [isSuccess, setIsSuccess] = useState(false)

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleSubmit = async (e) => {
		e.preventDefault()

		// Финальная проверка перед отправкой
		if (!validateAllFields()) return

		try {
			const response = await UserService.login({
				login: loginData.login,
				password: loginData.password,
			})

			if (!response.ok) {
				const errorJson = await response.json()
				setDialog({
					isOpen: true,
					type: "warning",
					content: errorJson.detail || "Ошибка авторизации",
				})

				return
			}

			// TODO: Логика "Запомнить меня"

			setIsSuccess(true)
			setDialog({
				isOpen: false, // true - убрал
				type: "ok",
				content: "Вход выполнен успешно",
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

		if (isSuccess) navigate("/main")
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
