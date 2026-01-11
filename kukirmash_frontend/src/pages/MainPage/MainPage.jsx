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

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.page}>
			<div className={styles.container}>
				<ServerSideBar
					onSearchClick={() => setActiveView("search")}
					onServerClick={() => setActiveView("server")}
				/>

				{activeView == "search" && <SearchServers />}

				{activeView === "server" && (
					<>
						<ServerChannelsBar />
						<ServerContent />
						<UsersBar />
					</>
				)}
			</div>
		</div>
	)
	//*----------------------------------------------------------------------------------------------------------------------------
}
