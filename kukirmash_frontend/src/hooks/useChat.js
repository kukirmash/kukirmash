import { useState, useEffect } from "react"
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import { API_URL } from "../services/ServerService"
import { TextMessageService } from "../services/TextMessageService"

export const useChat = (serverId, channelId) => {

    //----------------------------------------------------------------------------------------------------------------------------
	const [messages, setMessages] = useState([])
	const [connection, setConnection] = useState(null)

    //----------------------------------------------------------------------------------------------------------------------------
	useEffect(() => {
		if (!channelId || !serverId) return

		let newConnection

		const setupChat = async () => {
			// 1. Загружаем историю из REST API
			try {
				const history = await TextMessageService.getChannelMessages(serverId, channelId, 50)
				setMessages(history)
			} catch (e) {
				console.error("Ошибка загрузки истории сообщений:", e.message)
			}

			// 2. Настраиваем SignalR
			newConnection = new HubConnectionBuilder()
				.withUrl(`${API_URL}/text-channels`, {
					withCredentials: true,
				})
				.withAutomaticReconnect()
				.configureLogging(LogLevel.Information)
				.build()

			// 3. Слушаем входящие сообщения
			newConnection.on("ReceiveMessage", (message) => {
				setMessages((prev) => [...prev, message])
			})

			try {
				await newConnection.start()
				await newConnection.invoke("JoinChannel", channelId)
				setConnection(newConnection)
			} catch (e) {
				console.error("SignalR Connection Error: ", e)
			}
		}

		setupChat()

		// Очистка при смене канала
		return () => {
			if (newConnection) {
				newConnection.invoke("LeaveChannel", channelId).then(() => newConnection.stop())
			}
		}
	}, [channelId, serverId])

    //----------------------------------------------------------------------------------------------------------------------------
	// Функция отправки
	const sendMessage = async (text) => {
		if (connection && text.trim() !== "") {
			try {
				await connection.invoke("SendMessage", serverId, channelId, text)
			} catch (e) {
				console.error("Ошибка отправки сообщения:", e)
			}
		}
	}

    //----------------------------------------------------------------------------------------------------------------------------
	return { messages, sendMessage }

    //----------------------------------------------------------------------------------------------------------------------------
}