// Получаем URL из ENV. Если переменной нет - используем локалхост как фолбек.
const API_URL = process.env.REACT_APP_API_URL || "http://localhost:5000"

export const ServerService = {
	//*----------------------------------------------------------------------------------------------------------------------------
	async addServer({ name, desc, icon }) {
		// 1. Создаем объект FormData для отправки файлов
		const formData = new FormData()

		// 2. Добавляем поля. Ключи ("Name", "Description", "Icon")
		formData.append("Name", name)
		formData.append("Description", desc)

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
