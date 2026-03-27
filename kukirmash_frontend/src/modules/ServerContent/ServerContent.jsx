import React from "react"
import styles from "./ServerContent.module.css"
import { MessageInput } from "../../components/MessageInput/MessageInput"

export const ServerContent = ({ name }) => {
	return (
		<div className={styles.container}>
			{/* Шапка чата */}
			<div className={styles.header}>
				<h3>{name}</h3>
			</div>

			{/* Область для сообщений (именно она будет скроллиться) */}
			<div className={styles.messagesArea}>
				{/* Здесь в будущем будет map() по массиву сообщений */}
			</div>

			{/* Поле ввода прибито к низу */}
			<MessageInput />
		</div>
	)
}
