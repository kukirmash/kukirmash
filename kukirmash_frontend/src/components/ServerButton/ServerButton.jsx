import React from "react"
import styles from "./ServerButton.module.css"
import placeholder_server from "../../assets/placeholder_server.svg"
import { CircleButton } from "../../ui/CircleButton/CircleButton"

export const ServerButton = ({ serverImg, onClick }) => {
	return (
		<CircleButton className={styles.serverButton} onClick={onClick}>
			{serverImg ? (
				<img className={styles.serverImg} src={serverImg} alt="" />
			) : (
				<img
					className={styles.placeholder}
					src={placeholder_server}
					alt=""
				/>
			)}
		</CircleButton>
	)
}
