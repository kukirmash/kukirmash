import React, { useState } from "react"
import styles from "./MessageInput.module.css"
import { Button } from "../../ui/Button/Button"

export const MessageInput = () => {
	const [text, setText] = useState("")

	return (
		<div className={styles.inputContainer}>
			<input
				type="text"
				className={styles.input}
				placeholder="Написать сообщение..."
				value={text}
				onChange={(e) => setText(e.target.value)}
			/>
			<Button className={styles.sendButton}>></Button>
		</div>
	)
}
