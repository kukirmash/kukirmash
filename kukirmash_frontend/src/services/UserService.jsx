export const API_URL = process.env.REACT_APP_API_URL || "http://localhost:5000"

export const UserService = {
	//*----------------------------------------------------------------------------------------------------------------------------
	async register({ login, email, password }) {
		const request = {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({ login, email, password }),
			credentials: "include",
		}

		const response = await fetch(`${API_URL}/register`, request)

		//console.log(response);
		return response
	},

	//*----------------------------------------------------------------------------------------------------------------------------
	async login({ login, password }) {
		const request = {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({ login, password }),
			credentials: "include",
		}

		const response = await fetch(`${API_URL}/login`, request)

		//console.log(response);
		return response
	},

	//*----------------------------------------------------------------------------------------------------------------------------
	async getUserServers() {
		const response = await fetch(`${API_URL}/users/me/servers`, {
			method: "GET",
			headers: {
				"Content-Type": "application/json",
			},
			credentials: "include", // Важно для передачи кук с токеном
		})

		if (!response.ok) {
			throw new Error("Не удалось загрузить список серверов")
		}

		return await response.json()
	},

	//*----------------------------------------------------------------------------------------------------------------------------
}
