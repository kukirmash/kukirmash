import React from "react"
import styles from "./AddServerButton.module.css"
import plus_secondary from "../../assets/plus_secondary.svg"
import { CircleButton } from "../../ui/CircleButton/CircleButton"

export const AddServerButton = ({ onClick }) => {
	return (
		<CircleButton className={styles.addServerButton} onClick={onClick}>
			<img src={plus_secondary} alt="" />
		</CircleButton>
	)
}
