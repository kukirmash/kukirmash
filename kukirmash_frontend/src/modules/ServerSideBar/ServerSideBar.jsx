import React, { useState, useEffect } from "react"

import { ServerButton } from "../../components/ServerButton/ServerButton"
import { AddServerButton } from "../../components/AddServerButton/AddServerButton"
import { SearchServerButton } from "../../components/SearchServerButton/SearchServerButton"
import { AddServerFormDialog } from "../../components/AddServerFormDialog/AddServerFormDialog"

import kukirmash_logo from "../../assets/kukirmash_logo.svg"
import styles from "./ServerSideBar.module.css"
import { UserService, API_URL } from "../../services/UserService"

export const ServerSideBar = ({ onSearchClick, onServerClick }) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const [isAddServerFormOpen, setIsAddServerFormOpen] = useState(false)

	const [myServers, setMyServers] = useState([])

	//*----------------------------------------------------------------------------------------------------------------------------
	// Загружаем сервера при монтировании компонента (открытии окна)
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
	}, []) // Пустой массив зависимостей = выполнить один раз при старте

	//*----------------------------------------------------------------------------------------------------------------------------
	// Функция обновления списка - сразу после создания сервера
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
					// Собираем полный путь к картинке
					serverImg={
						server.iconPath ? `${API_URL}${server.iconPath}` : null
					}
					onClick={onServerClick}
				/>
			))}

			{myServers.length != 0 && <div className={styles.separator}></div>}

			<AddServerButton onClick={() => setIsAddServerFormOpen(true)} />
			{isAddServerFormOpen && (
				<AddServerFormDialog
					onClose={() => {
						setIsAddServerFormOpen(false)
						handleServerCreated()
					}}
				/>
			)}

			<SearchServerButton onClick={onSearchClick}></SearchServerButton>
		</div>
	)

	//*----------------------------------------------------------------------------------------------------------------------------
}
