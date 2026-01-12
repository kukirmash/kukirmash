import React, { useState, useEffect } from "react"

import { User } from "../../components/User/User"

import { ServerService } from "../../services/ServerService"
import styles from "./UsersBar.module.css"

export const UsersBar = ({ serverId }) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const [users, setUsers] = useState([])
	const [loading, setLoading] = useState(false)

	//*----------------------------------------------------------------------------------------------------------------------------
	// Загружаем пользователей каждый раз, когда меняется serverId
	useEffect(() => {
		if (!serverId) return

		const fetchUsers = async () => {
			setLoading(true)
			try {
				const data = await ServerService.getServerUsers(serverId)
				setUsers(data)
			} catch (error) {
				console.error(error)
			} finally {
				setLoading(false)
			}
		}

		fetchUsers()
	}, [serverId])

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<div className={styles.container}>
			<h3>Участники — {users.length}</h3>

			<div className={styles.usersList}>
				{loading ? (
					<p>Загрузка...</p>
				) : (
					users.map((user) => (
						<User key={user.id} login={user.login} />
					))
				)}
			</div>
		</div>
	)

	//*----------------------------------------------------------------------------------------------------------------------------
}
