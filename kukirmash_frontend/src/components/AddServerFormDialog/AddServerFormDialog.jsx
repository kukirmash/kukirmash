import React, { useState } from "react"
import { Dialog } from "../../ui/Dialog/Dialog"
import { Input } from "../../ui/Input/Input"
import { Button } from "../../ui/Button/Button"
import { ServerService } from "../../services/ServerService"
import styles from "./AddServerFormDialog.module.css"

export const AddServerFormDialog = ({ onClose }) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	// TODO: в один объект, добавить фото, большое текстовое поле для описания
	const [name, setName] = useState("")
	const [desc, setDesc] = useState("")
	const [errors, setErrors] = useState({})

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleSubmit = async (e) => {
		e.preventDefault()
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<Dialog
			title="Добавить сервер"
			onClose={onClose}
			width="30rem" // Указываем нужную ширину здесь!
		>
			<form className={styles.form} onSubmit={handleSubmit}>
				<Input
					label="Название"
					value={name}
					onChange={setName}
					placeholder="Введите название"
					errorText={errors.name}
				/>
				<Input
					label="Описание"
					value={desc}
					onChange={setDesc}
					placeholder="Введите описание"
				/>
				<Button type="submit" className={styles.submitBtn}>
					Создать
				</Button>
			</form>
		</Dialog>
	)

	//*----------------------------------------------------------------------------------------------------------------------------
}
