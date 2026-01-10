import React from "react"
import styles from "./Switch.module.css"

export const Switch = ({ checked, onChange, label }) => {
	return (
		<>
			{label && <label className={styles.label}>{label}</label>}
			<label className={styles.wrapper}>
				<div className={styles.switch}>
					<input
						type="checkbox"
						className={styles.input}
						checked={checked}
						onChange={(e) => onChange(e.target.checked)}
					/>
					<span className={styles.slider}></span>
				</div>
			</label>
		</>
	)
}
