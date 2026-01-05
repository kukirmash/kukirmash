const API_URL = "http://localhost:5000"

export const ServerService = {
	//*----------------------------------------------------------------------------------------------------------------------------
	async addServer({ name, desc }) {
		const request = {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({ name, desc }),
			credentials: "include",
		}

		const response = await fetch(`${API_URL}/server`, request)

		//console.log(response);
		return response
	},

	//*----------------------------------------------------------------------------------------------------------------------------
}
