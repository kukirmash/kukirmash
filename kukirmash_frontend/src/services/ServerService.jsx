// Получаем URL из ENV. Если переменной нет - используем локалхост как фолбек.
export const API_URL = process.env.REACT_APP_API_URL || "http://localhost:5000"

export const ServerService = {
	//*----------------------------------------------------------------------------------------------------------------------------
	async addServer({ name, desc, icon, isPrivate }) {
		// 1. Создаем объект FormData для отправки файлов
		const formData = new FormData()

		// 2. Добавляем поля. Ключи ("Name", "Description", "Icon")
		formData.append("Name", name)
		// Добавляем описание только если оно есть
		if (desc) formData.append("Description", desc)
		// Добавляем Icon только если это объект (файл), а не null
		if (icon) formData.append("Icon", icon)

		formData.append("IsPrivate", isPrivate)

		const request = {
			method: "POST",
			// ВАЖНО: Не указываем 'Content-Type': 'application/json' - Браузер сам поставит правильный тип для FormData
			body: formData,
			credentials: "include",
		}

		const response = await fetch(`${API_URL}/servers`, request)

		return response
	},

	//*----------------------------------------------------------------------------------------------------------------------------
	async getPublicServers() {
		const request = {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
			},
			credentials: "include",
		}

		const response = await fetch(`${API_URL}/servers/public`, request)

		if (!response.ok) {
			throw new Error("Не удалось загрузить список публичных серверов")
		}

		return await response.json()
	},

	//*----------------------------------------------------------------------------------------------------------------------------
	async getServerUsers(serverId) {
		const response = await fetch(`${API_URL}/servers/${serverId}/users`, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
			},
			credentials: "include",
		})

		if (!response.ok) {
			throw new Error("Не удалось загрузить список участников")
		}

		return await response.json()
	},
	//*----------------------------------------------------------------------------------------------------------------------------
	async joinServer(serverId) {
		const request = {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			credentials: "include",
		}

		const response = await fetch(
			`${API_URL}/servers/${serverId}/join`,
			request,
		)

		if (!response.ok) {
			throw new Error("Не удалось вступить в сервер")
		}

		return response
	},
	//*----------------------------------------------------------------------------------------------------------------------------
}
