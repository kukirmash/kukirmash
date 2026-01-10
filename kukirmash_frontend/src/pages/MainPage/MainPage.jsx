import React, { useState } from "react"

import { ServerSideBar } from "../../modules/ServerSideBar/ServerSideBar"
import { ServerChannelsBar } from "../../modules/ServerChannelsBar/ServerChannelsBar"
import { MainContent } from "../../modules/MainContent/MainContent"
import { UsersBar } from "../../modules/UsersBar/UsersBar"

import styles from "./MainPage.module.css"

export const MainPage = () => {
	const [showChannels, setShowChannels] = useState(true)
	const [showUsers, setShowUsers] = useState(true)

	return (
		<div className={styles.page}>
			<div className={styles.container}>
				<ServerSideBar />
				{showChannels && <ServerChannelsBar />}
				<MainContent />
				{showUsers && <UsersBar />}
			</div>
		</div>
	)
}
