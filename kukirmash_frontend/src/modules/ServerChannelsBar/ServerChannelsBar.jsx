import React, { useState, useEffect } from "react"
import styles from "./ServerChannelsBar.module.css"
import { TextChannelButton } from "../../components/TextChannelButton/TextChannelButton"
import { ServerService } from "../../services/ServerService" // Убедитесь, что путь верный

export const ServerChannelsBar = ({
	serverId,
	onChannelSelect,
	activeChannelId,
}) => {
	const [channels, setChannels] = useState([])
	const [isLoading, setIsLoading] = useState(false)

	useEffect(() => {
		if (!serverId) return

		const fetchChannels = async () => {
			setIsLoading(true)
			try {
				const data = await ServerService.getServerTextChannels(serverId)
				setChannels(data)
			} catch (error) {
				console.error("Ошибка при получении каналов:", error)
			} finally {
				setIsLoading(false)
			}
		}

		fetchChannels()
	}, [serverId])

	return (
		<div className={styles.container}>
			<h3>Каналы</h3>

			<div className={styles.channelsList}>
				{isLoading ? (
					<p>Загрузка...</p>
				) : (
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
	)
}
