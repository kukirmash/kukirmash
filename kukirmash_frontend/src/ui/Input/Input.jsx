import React from "react"
import styles from "./Input.module.css"

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
	return (
		<div className={`${styles.container} ${className}`.trim()}>
			{label && <label className={styles.label}>{label}</label>}
			<input
				className={styles.input}
				type={type}
				value={value}
				onChange={(e) => onChange(e.target.value)}
				onBlur={(e) => onBlur(e.target.value)}
				placeholder={placeholder}
				disabled={disabled}
				{...props}
			/>

			{errorText.trim() && (
				<span className={styles.errorText}>{errorText}</span>
			)}
		</div>
	)
}
