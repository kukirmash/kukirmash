import React, { useState, useEffect } from "react"

import { ServerButton } from "../../components/ServerButton/ServerButton"
import { AddServerFormDialog } from "../../components/AddServerFormDialog/AddServerFormDialog"
import { CircleButton } from "../../ui/CircleButton/CircleButton"

import kukirmash_logo from "../../assets/kukirmash_logo.svg"
import plus_secondary from "../../assets/plus_secondary.svg"
import search from "../../assets/search.svg"

import styles from "./ServerSideBar.module.css"
import { UserService, API_URL } from "../../services/UserService"

export const ServerSideBar = ({ onSearchClick, onServerClick }) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const [isAddServerFormOpen, setIsAddServerFormOpen] = useState(false)
	const [myServers, setMyServers] = useState([])

	//*----------------------------------------------------------------------------------------------------------------------------
	useEffect(() => {
		const getServers = async () => {
			try {
				const data = await UserService.getUserServers()
				setMyServers(data)
			} catch (error) {
				console.error("Ошибка загрузки серверов:", error)
			}
		}

		getServers()
	}, [])

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleServerCreated = async () => {
		try {
			const data = await UserService.getUserServers()
			setMyServers(data)
		} catch (error) {
			console.error(error)
		}
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.sidebar}>
			<img className={styles.logo} src={kukirmash_logo} alt="logo" />

			<div className={styles.separator}></div>

			{myServers.map((server) => (
				<ServerButton
					key={server.id}
					serverImg={
						server.iconPath ? `${API_URL}${server.iconPath}` : null
					}
					onClick={() => onServerClick(server)}
				/>
			))}

			{myServers.length !== 0 && <div className={styles.separator}></div>}

			{/* Кнопка "Добавить сервер" */}
			<CircleButton
				className={styles.actionButton}
				onClick={() => setIsAddServerFormOpen(true)}
			>
				<img src={plus_secondary} alt="добавить сервер" />
			</CircleButton>

			{isAddServerFormOpen && (
				<AddServerFormDialog
					onClose={() => {
						setIsAddServerFormOpen(false)
						handleServerCreated()
					}}
				/>
			)}

			{/* Кнопка "Поиск сервера" */}
			<CircleButton
				className={styles.actionButton}
				onClick={onSearchClick}
			>
				<img src={search} alt="поиск" />
			</CircleButton>
		</div>
	)
}
