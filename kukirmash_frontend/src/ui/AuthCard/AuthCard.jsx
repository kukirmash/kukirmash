import React from "react"
import styles from "./AuthCard.module.css"

export const AuthCard = ({ title, children, footer }) => {
	return (
		<div className={styles.card}>
			<h2 className={styles.title}>{title}</h2>

			<div className={styles.content}>{children}</div>

			{footer && <div className={styles.footer}>{footer}</div>}
		</div>
	)
}
