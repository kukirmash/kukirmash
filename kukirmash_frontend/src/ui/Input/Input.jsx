import { React, useState } from "react"
import styles from "./Input.module.css"
import open_eye from "../../assets/open_eye.svg"
import close_eye from "../../assets/close_eye.svg"

export const Input = ({
	label = "",
	type = "text",
	value = "",
	placeholder = "",
	className = "",
	disabled = false,
	errorText = "",
	onChange = () => {},
	onBlur = () => {},
	...props
}) => {
	// Состояние для переключения видимости пароля
	const [isPasswordVisible, setIsPasswordVisible] = useState(false)
	// Вычисляем текущий тип инпута
	const currentType = type === "password" && isPasswordVisible ? "text" : type
	const displayIcon = isPasswordVisible ? open_eye : close_eye

	return (
		<div className={`${styles.container} ${className}`.trim()}>
			{label && <label className={styles.label}>{label}</label>}

			{/* Обертка для позиционирования кнопки */}
			<div className={styles.inputWrapper}>
				<input
					// Добавляем класс, если это пароль, чтобы сделать отступ для кнопки
					className={`${styles.input} ${type === "password" ? styles.inputWithIcon : ""}`}
					type={currentType}
					value={value}
					onChange={(e) => onChange(e.target.value)}
					onBlur={(e) => onBlur(e.target.value)}
					placeholder={placeholder}
					disabled={disabled}
					{...props}
				/>

				{/* Показываем кнопку только если изначально был передан type="password" */}
				{type === "password" && (
					<button
						type="button"
						className={styles.toggleButton}
						onClick={() => setIsPasswordVisible(!isPasswordVisible)}
						tabIndex="-1" // Чтобы по кнопке Tab не перескакивало на глазик при вводе
						title={
							isPasswordVisible
								? "Скрыть пароль"
								: "Показать пароль"
						}
					>
						<img src={displayIcon} alt="?" />
					</button>
				)}
			</div>

			{errorText.trim() && (
				<span className={styles.errorText}>{errorText}</span>
			)}
		</div>
	)
}
