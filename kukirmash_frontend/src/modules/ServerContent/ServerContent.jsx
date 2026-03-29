import React, { useState, useEffect, useRef } from "react"
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import styles from "./ServerContent.module.css"
import { MessageInput } from "../../components/MessageInput/MessageInput"
import { API_URL } from "../../services/ServerService"
// Импортируем наш новый сервис
import { TextMessageService } from "../../services/TextMessageService"

export const ServerContent = ({ server, channel }) => {
	const [messages, setMessages] = useState([])
	const [connection, setConnection] = useState(null)

	const messagesEndRef = useRef(null)

	useEffect(() => {
		messagesEndRef.current?.scrollIntoView({ behavior: "smooth" })
	}, [messages])

	useEffect(() => {
		if (!channel || !server) return

		let newConnection

		const setupChat = async () => {
			// 1. Загружаем историю через сервис
			try {
				const history = await TextMessageService.getChannelMessages(
					server.id,
					channel.id,
					50,
				)
				setMessages(history)
			} catch (e) {
				console.error("Ошибка загрузки истории сообщений:", e.message)
			}

			// 2. Настраиваем SignalR
			newConnection = new HubConnectionBuilder()
				.withUrl(`${API_URL}/hubs/text-channels`, {
					withCredentials: true,
				})
				.withAutomaticReconnect()
				.configureLogging(LogLevel.Information)
				.build()

			// 3. Подписываемся на получение сообщений
			newConnection.on("ReceiveMessage", (message) => {
				setMessages((prev) => [...prev, message])
			})

			try {
				await newConnection.start()
				await newConnection.invoke("JoinChannel", channel.id)
				setConnection(newConnection)
			} catch (e) {
				console.error("SignalR Connection Error: ", e)
			}
		}

		setupChat()

		return () => {
			if (newConnection) {
				newConnection
					.invoke("LeaveChannel", channel.id)
					.then(() => newConnection.stop())
			}
		}
	}, [channel?.id, server?.id])

	const handleSendMessage = async (text) => {
		if (connection && text.trim() !== "") {
			try {
				await connection.invoke(
					"SendMessage",
					server.id,
					channel.id,
					text,
				)
			} catch (e) {
				console.error("Ошибка отправки сообщения:", e)
			}
		}
	}

	if (!channel) {
		return (
			<div className={styles.container}>
				<div className={styles.header}>
					<h3>Выберите канал слева</h3>
				</div>
			</div>
		)
	}

	return (
		<div className={styles.container}>
			<div className={styles.header}>
				<h3># {channel.name}</h3>
			</div>

			<div className={styles.messagesArea}>
				{messages.length === 0 ? (
					<p
						style={{
							color: "gray",
							textAlign: "center",
							marginTop: "auto",
						}}
					>
						Здесь пока нет сообщений. Начните общение первым!
					</p>
				) : (
					messages.map((msg, idx) => (
						<div key={msg.id || idx} style={{ padding: "0.5rem" }}>
							<strong>{msg.creatorId || msg.CreatorId}: </strong>
							<span>{msg.text || msg.Text}</span>
						</div>
					))
				)}
				<div ref={messagesEndRef} />
			</div>

			<MessageInput onSend={handleSendMessage} />
		</div>
	)
}
