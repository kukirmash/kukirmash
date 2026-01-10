// Получаем URL из ENV. Если переменной нет - используем локалхост как фолбек.
export const API_URL = process.env.REACT_APP_API_URL || "http://localhost:5000"

export const ServerService = {
	//*----------------------------------------------------------------------------------------------------------------------------
	async addServer({ name, desc, icon }) {
		// 1. Создаем объект FormData для отправки файлов
		const formData = new FormData()

		// 2. Добавляем поля. Ключи ("Name", "Description", "Icon")
		formData.append("Name", name)
		// Добавляем описание только если оно есть
		if (desc) {
			formData.append("Description", desc)
		}
		// Добавляем Icon только если это объект (файл), а не null
		if (icon) {
			formData.append("Icon", icon)
		}

		const request = {
			method: "POST",
			// ВАЖНО: Не указываем 'Content-Type': 'application/json' - Браузер сам поставит правильный тип для FormData
			body: formData,
			credentials: "include",
		}

		const response = await fetch(`${API_URL}/server`, request)

		return response
	},

	//*----------------------------------------------------------------------------------------------------------------------------
}
