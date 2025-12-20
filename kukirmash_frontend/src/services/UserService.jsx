const API_URL = "http://localhost:5058";

export const UserService = {

    //*----------------------------------------------------------------------------------------------------------------------------
    async register({ login, email, password }) {
        const response = await fetch(`${API_URL}/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ login, email, password }),
        });
        
        //console.log(response);
        return response;
    },

    //*----------------------------------------------------------------------------------------------------------------------------
    async login({ login, password }) {
        const response = await fetch(`${API_URL}/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ login, password }),
        });

        //console.log(response);
        return response;
    },

    //*----------------------------------------------------------------------------------------------------------------------------
};