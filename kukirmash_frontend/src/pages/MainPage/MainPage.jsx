import React, { useState } from "react"

import { SearchServers } from "../../modules/SearchServers/SearchServers"
import { ServerSideBar } from "../../modules/ServerSideBar/ServerSideBar"
import { ServerChannelsBar } from "../../modules/ServerChannelsBar/ServerChannelsBar"
import { ServerContent } from "../../modules/ServerContent/ServerContent"
import { UsersBar } from "../../modules/UsersBar/UsersBar"

import styles from "./MainPage.module.css"

export const MainPage = () => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const [activeView, setActiveView] = useState("search")
	const [selectedServer, setSelectedServer] = useState(null)
	const [selectedChannel, setSelectedChannel] = useState(null) // <-- Новый стейт для текущего канала

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleServerClick = (server) => {
		setSelectedServer(server)
		setSelectedChannel(null) // Сбрасываем выбранный канал при переключении сервера
		setActiveView("server")
	}

	const handleChannelSelect = (channel) => {
		setSelectedChannel(channel)
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.page}>
			<div className={styles.container}>
				<ServerSideBar
					onSearchClick={() => setActiveView("search")}
					onServerClick={handleServerClick}
				/>

				{activeView === "search" && <SearchServers />}

				{activeView === "server" && selectedServer && (
					<>
						<ServerChannelsBar
							serverId={selectedServer.id}
							onChannelSelect={handleChannelSelect}
							activeChannelId={selectedChannel?.id}
						/>
						{/* Передаем имя канала, если он выбран, иначе имя сервера */}
						<ServerContent
							name={
								selectedChannel
									? selectedChannel.name
									: selectedServer.name
							}
						/>
						<UsersBar serverId={selectedServer.id} />
					</>
				)}
			</div>
		</div>
	)

	//*----------------------------------------------------------------------------------------------------------------------------
}
