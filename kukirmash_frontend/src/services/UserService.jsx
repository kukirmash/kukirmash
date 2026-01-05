const API_URL = "http://localhost:5000"

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
}
