import React from "react"

import styles from "./User.module.css"

export const User = ({ login, userImg }) => {
	return (
		<div className={styles.user}>
			<div className={styles.avatarCont}>
				{userImg ? (
					<img src={userImg} alt="?"></img>
				) : (
					<div className={styles.placeholder}>
						{login ? login[0] : "?"}
					</div>
				)}
			</div>

			<span className={styles.login}>{login}</span>
		</div>
	)
}
