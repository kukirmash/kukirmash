import React, { useEffect, useRef } from "react"
import styles from "./ServerContent.module.css"
import { MessageInput } from "../../components/MessageInput/MessageInput"
import { Message } from "../../components/Message/Message"
import { useChat } from "../../hooks/useChat"

export const ServerContent = ({ server, channel }) => {
	//----------------------------------------------------------------------------------------------------------------------------
	const { messages, sendMessage } = useChat(server?.id, channel?.id)
	const messagesEndRef = useRef(null)

	//----------------------------------------------------------------------------------------------------------------------------
	// Автоскролл вниз при новых сообщениях
	useEffect(() => {
		messagesEndRef.current?.scrollIntoView({ behavior: "smooth" })
	}, [messages])

	//----------------------------------------------------------------------------------------------------------------------------
	if (!channel) {
		return (
			<div className={styles.container}>
				<div className={styles.header}>
					<h3>Выберите канал слева</h3>
				</div>
			</div>
		)
	}

	//----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.container}>
			<div className={styles.header}>
				<h3># {channel.name}</h3>
			</div>

			<div className={styles.messagesArea}>
				<div className={styles.messagesWrapper}>
					{messages.length === 0 ? (
						<p style={{ color: "gray", textAlign: "center", marginTop: "auto" }}>
							Здесь пока нет сообщений. Начните общение первым!
						</p>
					) : (
						messages.map((msg, idx) => (
							<Message
								key={msg.id || idx}
								author={msg.authorName || msg.AuthorName || "Загрузка..."}
								date={msg.createdDateTimeUtc || msg.CreatedDateTimeUtc || new Date()}
								text={msg.text || msg.Text}
							/>
						))
					)}
					<div ref={messagesEndRef} />
				</div>
			</div>

			<MessageInput onSend={sendMessage} />
		</div>
	)

	//----------------------------------------------------------------------------------------------------------------------------
}