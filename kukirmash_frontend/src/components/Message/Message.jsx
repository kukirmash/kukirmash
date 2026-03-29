import React from "react"
import styles from "./Message.module.css"

export const Message = ({ author, date, text }) => {

    //----------------------------------------------------------------------------------------------------------------------------
	// Форматируем дату (например: 12.10.2023, 14:30)
	const formattedDate = new Date(date).toLocaleString("ru-RU", {
		day: "2-digit",
		month: "2-digit",
		year: "numeric",
		hour: "2-digit",
		minute: "2-digit"
	})

    //----------------------------------------------------------------------------------------------------------------------------
    // Берем первую букву для аватарки
	const avatarLetter = author ? author.charAt(0).toUpperCase() : "?"

    //----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.messageContainer}>
			<div className={styles.avatar}>
				{avatarLetter}
			</div>
			
			<div className={styles.content}>
				<div className={styles.header}>
					<span className={styles.author}>{author || "Пользователь"}</span>
					<span className={styles.date}>{formattedDate}</span>
				</div>
				<div className={styles.text}>{text}</div>
			</div>
		</div>
	)

    //----------------------------------------------------------------------------------------------------------------------------
}