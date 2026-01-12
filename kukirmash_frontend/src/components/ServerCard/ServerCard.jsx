import React from "react"

import { ServerButton } from "../ServerButton/ServerButton"
import { Button } from "../../ui/Button/Button"

import styles from "./ServerCard.module.css"

export const ServerCard = ({ name, desc, serverImg, onJoin }) => {
	return (
		<div className={styles.container}>
			<div className={styles.imgName}>
				<ServerButton serverImg={serverImg} />
				<h2 className={styles.serverName}>{name}</h2>
			</div>

			{desc ? (
				<p className={styles.description}>{desc}</p>
			) : (
				<p className={styles.noDescription}>Нет описания</p>
			)}

			<Button onClick={onJoin}>Вступить</Button>
		</div>
	)
}
