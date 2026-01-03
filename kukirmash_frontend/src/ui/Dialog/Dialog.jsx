import React from "react"

import { CancelButton } from "../CancelButton/CancelButton"

import errorIcon from "../../assets/error.svg"
import warningIcon from "../../assets/warning.svg"
import okIcon from "../../assets/ok.svg"
import styles from "./Dialog.module.css"

//*----------------------------------------------------------------------------------------------------------------------------
const icons = {
	ok: okIcon,
	warning: warningIcon,
	error: errorIcon,
}

//*----------------------------------------------------------------------------------------------------------------------------
const titles = {
	ok: "Успешно",
	warning: "Предупреждение",
	error: "Ошибка",
}

//*----------------------------------------------------------------------------------------------------------------------------
export const Dialog = ({
	type,
	title = "",
	content,
	children,
	onClose = {},
	width = "24rem",
}) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	// Если передан type, берем стандартную иконку и заголовок,
	// но приоритет у пропса title
	const displayTitle = title || (type ? titles[type] : "")
	const displayIcon = type ? icons[type] : null

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.dialogBackground} onClick={onClose}>
			<div
				className={styles.dialog}
				style={{ width: width }}
				onClick={(e) => e.stopPropagation()}
			>
				<div className={styles.dialogHeader}>
					<h2>{displayTitle}</h2>
					<CancelButton onClick={onClose} />
				</div>

				<div className={styles.dialogContent}>
					{displayIcon && (
						<img
							className={styles.svg}
							alt={type}
							src={displayIcon}
						/>
					)}
					{content && <p className={styles.dialogText}>{content}</p>}
					{children}
				</div>
			</div>
		</div>
	)
}
