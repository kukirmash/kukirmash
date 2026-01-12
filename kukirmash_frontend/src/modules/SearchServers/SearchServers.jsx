import React, { useState, useEffect } from "react"

import { ServerCard } from "../../components/ServerCard/ServerCard"

import styles from "./SearchServers.module.css"
import { ServerService, API_URL } from "../../services/ServerService"

export const SearchServers = () => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const [publicServers, setPublicServers] = useState([])

	//*----------------------------------------------------------------------------------------------------------------------------
	// Загружаем сервера при монтировании компонента (открытии окна)
	useEffect(() => {
		const getServers = async () => {
			try {
				const data = await ServerService.getPublicServers()
				setPublicServers(data)
			} catch (error) {
				console.error("Ошибка загрузки серверов:", error)
			}
		}

		getServers()
	}, []) // Пустой массив зависимостей = выполнить один раз при старте

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleJoin = async (serverId) => {
		try {
			await ServerService.joinServer(serverId)
			alert("Вы успешно вступили в сервер!")
			// Тут можно обновить список серверов или перекинуть пользователя
		} catch (error) {
			console.error(error)
			alert("Ошибка при вступлении")
		}
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.container}>
			<h2>Поиск серверов</h2>

			<div className={styles.serversContainer}>
				{publicServers.map((server) => (
					<ServerCard
						key={server.id}
						serverImg={
							server.iconPath
								? `${API_URL}${server.iconPath}`
								: null
						}
						name={server.name}
						desc={server.description}
						onJoin={() => handleJoin(server.id)}
					/>
				))}
			</div>
		</div>
	)

	//*----------------------------------------------------------------------------------------------------------------------------
}
