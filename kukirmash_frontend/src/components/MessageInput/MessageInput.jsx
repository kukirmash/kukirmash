import React, { useState } from "react"
import styles from "./MessageInput.module.css"
import { Button } from "../../ui/Button/Button"

export const MessageInput = ({ onSend }) => {
	const [text, setText] = useState("")

	const handleSend = () => {
		if (text.trim() === "") return
		onSend(text)
		setText("") // Очищаем поле после отправки
	}

	const handleKeyDown = (e) => {
		if (e.key === "Enter") {
			handleSend()
		}
	}

	return (
		<div className={styles.inputContainer}>
			<input
				type="text"
				className={styles.input}
				placeholder="Написать сообщение..."
				value={text}
				onChange={(e) => setText(e.target.value)}
				onKeyDown={handleKeyDown}
			/>
			<Button className={styles.sendButton} onClick={handleSend}>
				{">"}
			</Button>
		</div>
	)
}
