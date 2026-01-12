import React from "react"
import styles from "./ServerContent.module.css"

export const ServerContent = ({ name }) => {
	return (
		<div className={styles.container}>
			<h3>{name}</h3>
		</div>
	)
}
