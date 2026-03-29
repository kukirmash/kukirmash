import React, { useState, useEffect } from "react"
import styles from "./ServerChannelsBar.module.css"

import { Dialog } from "../../ui/Dialog/Dialog"
import { TextChannelButton } from "../../components/TextChannelButton/TextChannelButton"

import { ServerService } from "../../services/ServerService"

export const ServerChannelsBar = (
	{
		serverId,
		onChannelSelect,
		activeChannelId
	}) => {

	//----------------------------------------------------------------------------------------------------------------------------
	const [channels, setChannels] = useState([])		// текстовые каналы
	const [isLoading, setIsLoading] = useState(false)	// состояние - загрузились ли все каналы 

	const [dialog, setDialog] = useState({
		isOpen: false,
		type: "ok",
		content: "",
	})

	//----------------------------------------------------------------------------------------------------------------------------
	// Загрузка всех текстовых каналов
	useEffect(() => {
		if (!serverId)
			return;

		const fetchChannels = async () => {
			setIsLoading(true)
			try {
				const response = await ServerService.getServerTextChannels(serverId)

				switch (response.status) {
					case 200: // Успех
						const data = await response.json()
						setChannels(data)
						break
					case 401: // Unauthorized
						setDialog({
							isOpen: true,
							type: "error",
							content: "Ошибка авторизации. Пожалуйста, войдите заново.",
						})
						break
					case 500: // Server Error
						setDialog({
							isOpen: true,
							type: "error",
							content: "Внутренняя ошибка сервера при получении каналов.",
						})
						break
					default:
						setDialog({
							isOpen: true,
							type: "error",
							content: "Неизвестная ошибка при загрузке каналов.",
						})
						break
				}
			} catch (error) {
				setDialog({
					isOpen: true,
					type: "error",
					content: "Не удалось соединиться с сервером",
				})
			} finally {
				setIsLoading(false)
			}
		}

		fetchChannels();
	}, [serverId])

	//----------------------------------------------------------------------------------------------------------------------------
	return (
		<>
			{dialog.isOpen && (
				<Dialog
					type={dialog.type}
					content={dialog.content}
					onClose={setDialog({ ...dialog, isOpen: false })}
				/>
			)}

			<div className={styles.container}>
				<h3>Каналы</h3>

				<div className={styles.channelsList}>
					{isLoading ? (<p>Загрузка...</p>) :
						(
							channels.map((channel) => (
								<TextChannelButton
									key={channel.id}
									name={channel.name}
									isActive={activeChannelId === channel.id}
									onClick={() => onChannelSelect(channel)}
								/>
							))
						)}
				</div>
			</div>
		</>
	)

	//----------------------------------------------------------------------------------------------------------------------------
}
