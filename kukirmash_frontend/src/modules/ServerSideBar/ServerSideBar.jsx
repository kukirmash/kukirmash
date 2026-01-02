import React, { useState } from "react"
import styles from "./ServerSideBar.module.css"
import { ServerButton } from "../../components/ServerButton/ServerButton"
import { AddServerButton } from "../../components/AddServerButton/AddServerButton"
import { SearchServerButton } from "../../components/SearchServerButton/SearchServerButton"
import kukirmash_logo from "../../assets/kukirmash_logo.svg"
import { AddServerFormDialog } from "../../components/AddServerFormDialog/AddServerFormDialog"

export const ServerSideBar = () => {
	const [isAddServerOpen, setIsAddServerOpen] = useState(false)

	return (
		<div className={styles.sidebar}>
			<img className={styles.logo} src={kukirmash_logo} alt="logo" />

			<div className={styles.separator}></div>

			<ServerButton onClick={{}} />
			<ServerButton onClick={{}} />
			<ServerButton onClick={{}} />

			<div className={styles.separator}></div>

			<AddServerButton onClick={() => setIsAddServerOpen(true)} />
			{isAddServerOpen && (
				<AddServerFormDialog
					onClose={() => setIsAddServerOpen(false)}
				/>
			)}

			<SearchServerButton></SearchServerButton>
		</div>
	)
}
