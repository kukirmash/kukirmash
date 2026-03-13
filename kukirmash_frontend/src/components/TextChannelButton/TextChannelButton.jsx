import React from "react"
import styles from "./TextChannelButton.module.css"

export const TextChannelButton = ({ name, onClick, isActive }) => {
	return (
		<button
			type="button"
			className={`${styles.channelButton} ${isActive ? styles.active : ""}`.trim()}
			onClick={onClick}
		>
			<span className={styles.hash}>#</span>
			<span className={styles.name}>{name}</span>
		</button>
	)
}
