// src/ui/ImageUpload/ImageUpload.jsx
import React, { useState, useRef } from "react"
import styles from "./ImageUpload.module.css"

export const ImageUpload = ({ label, onFileSelect }) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const [previewImageUrl, setPreviewImageUrl] = useState(null)
	const fileInputRef = useRef(null)

	//*----------------------------------------------------------------------------------------------------------------------------
	const handleFileChange = (event) => {
		const file = event.target.files[0]

		if (file) {
			// Простая валидация на тип изображения
			if (!file.type.startsWith("image/")) {
				alert("Пожалуйста, выберите изображение.")
				return
			}

			// Создаем URL для превью
			const url = URL.createObjectURL(file)
			setPreviewImageUrl(url)

			// Передаем файл родителю
			onFileSelect(file)
		}
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	// TODO: кнопка сброса фото
	return (
		<>
			{label && <label className={styles.label}>{label}</label>}

			<div
				className={styles.container}
				onClick={() => {
					fileInputRef.current.click()
				}}
			>
				{previewImageUrl ? (
					<img
						src={previewImageUrl}
						alt="Server preview"
						className={styles.previewImage}
					/>
				) : (
					<div className={styles.placeholder}>Выберите файл</div>
				)}

				{/* Скрытый input - нажатие только программно (эмулируется через ref) */}
				<input
					type="file"
					ref={fileInputRef}
					onChange={handleFileChange}
					className={styles.hiddenInput}
					accept="image/png, image/jpeg, image/gif, image/webp" // Ограничиваем типы файлов
				/>
			</div>
		</>
	)

	//*----------------------------------------------------------------------------------------------------------------------------
}
