import React, { useState } from "react"

import { SearchServers } from "../../modules/SearchServers/SearchServers"
import { ServerSideBar } from "../../modules/ServerSideBar/ServerSideBar"
import { ServerChannelsBar } from "../../modules/ServerChannelsBar/ServerChannelsBar"
import { ServerContent } from "../../modules/ServerContent/ServerContent"
import { UsersBar } from "../../modules/UsersBar/UsersBar"

import styles from "./MainPage.module.css"

export const MainPage = () => {
	//----------------------------------------------------------------------------------------------------------------------------
	const [activeView, setActiveView] = useState("search") 			// режим: сервер, поиск серверов
	const [selectedServerId, setSelectedServerId] = useState(null)		// id выбранного сервера
	const [selectedChannelId, setSelectedChannelId] = useState(null)	// id выбранного текстового канала

	//----------------------------------------------------------------------------------------------------------------------------
	const handleServerClick = (server) => {
		setSelectedServerId(server)
		setSelectedChannelId(null) // Сбрасываем выбранный канал при переключении сервера
		setActiveView("server")
	}

	const handleChannelSelect = (channel) => {
		setSelectedChannelId(channel)
	}

	//----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.page}>
			<div className={styles.container}>
				<ServerSideBar
					onSearchClick={() => setActiveView("search")}
					onServerClick={handleServerClick}
				/>

				{activeView === "search" && <SearchServers />}

				{activeView === "server" && selectedServerId && (
					<>
						<ServerChannelsBar
							serverId={selectedServerId.id}
							onChannelSelect={handleChannelSelect}
							activeChannelId={selectedChannelId?.id}
						/>

						<ServerContent
							server={selectedServerId}
							channel={selectedChannelId}
						/>
						<UsersBar serverId={selectedServerId.id} />
					</>
				)}
			</div>
		</div>
	)

	//----------------------------------------------------------------------------------------------------------------------------
}
