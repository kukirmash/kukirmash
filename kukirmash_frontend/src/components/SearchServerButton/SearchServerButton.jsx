import React from "react"
import styles from "./SearchServerButton.module.css"
import { CircleButton } from "../../ui/CircleButton/CircleButton"
import search from "../../assets/search.svg"

export const SearchServerButton = ({ onClick }) => {
	return (
		<CircleButton className={styles.searchServerButton} onClick={onClick}>
			<img src={search} alt="поиск" />
		</CircleButton>
	)
}
