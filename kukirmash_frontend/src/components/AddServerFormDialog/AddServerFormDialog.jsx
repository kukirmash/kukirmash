import React, { useState } from "react"

import { Dialog } from "../../ui/Dialog/Dialog"
import { Input } from "../../ui/Input/Input"
import { Button } from "../../ui/Button/Button"
import { ImageUpload } from "../../ui/ImageUpload/ImageUpload"
import { Switch } from "../../ui/Switch/Switch"

import { ServerService } from "../../services/ServerService"
import { useAuthValidation } from "../../hooks/useAuthValidation"
import styles from "./AddServerFormDialog.module.css"

export const AddServerFormDialog = ({ onClose }) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	// хук валидации
	const {
		values: serverData,
		errors,
		isValid,
		handleChange,
		validateField,
		validateAllFields,
	} = useAuthValidation({
		serverName: "",
		serverDesc: "",
	})

	// данные для сервера без валидации
	const [serverIconFile, setServerIconFile] = useState(null)
	const [isPrivate, setIsPrivate] = useState(false)

	// Состояние для информационного диалога (успех/ошибка)
	const [infoDialog, setInfoDialog] = useState({
		isOpen: false,
		type: "ok",
		content: "",
	})

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleSubmit = async (e) => {
		e.preventDefault()

		// Проверяем все поля перед отправкой
		if (!validateAllFields()) return

		try {
			const response = await ServerService.addServer({
				name: serverData.serverName,
				desc: serverData.serverDesc,
				icon: serverIconFile,
				isPrivate: isPrivate,
			})

			if (response.ok) {
				setInfoDialog({
					isOpen: true,
					type: "ok",
					content: "Сервер успешно создан!",
				})
			} else {
				const errorData = await response.json()
				throw new Error(
					errorData.detail || "Ошибка при создании сервера",
				)
			}
		} catch (err) {
			setInfoDialog({
				isOpen: true,
				type: "error",
				content: "Неизвестная ошибка",
			})
		}
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleInfoClose = () => {
		// Если успех - закрываем сразу всё родительское окно.
		// Нет смысла закрывать infoDialog, если мы уничтожаем весь компонент.
		if (infoDialog.type === "ok") {
			onClose()
		} else {
			// Если была ошибка, закрываем только инфо-диалог, оставляем форму
			setInfoDialog({ ...infoDialog, isOpen: false })
		}
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	return (
		<>
			<Dialog title="Добавить сервер" onClose={onClose} width="26rem">
				<form className={styles.form} onSubmit={handleSubmit}>
					<ImageUpload
						label="Аватар"
						onFileSelect={setServerIconFile}
					/>

					<Input
						label="Название*"
						value={serverData.serverName}
						onChange={(val) => handleChange("serverName", val)}
						onBlur={() => validateField("serverName")}
						placeholder="Введите название сервера"
						errorText={errors.serverName}
					/>

					<Input
						label="Описание"
						value={serverData.serverDesc}
						onChange={(val) => handleChange("serverDesc", val)}
						placeholder="Краткое описание"
						errorText={errors.serverDesc}
					/>

					<Switch
						checked={isPrivate}
						onChange={setIsPrivate}
						label="Приватный сервер*"
					></Switch>

					<Button
						type="submit"
						disabled={!isValid}
						className={styles.submitButton}
					>
						Создать сервер
					</Button>
				</form>
			</Dialog>

			{infoDialog.isOpen && (
				<Dialog
					type={infoDialog.type}
					content={infoDialog.content}
					onClose={handleInfoClose}
				/>
			)}
		</>
	)
}
