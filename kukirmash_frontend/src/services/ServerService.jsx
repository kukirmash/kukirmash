const API_URL = "http://localhost:5058";

export const ServerService = {

    //*----------------------------------------------------------------------------------------------------------------------------
    async add({ name, desc }) {
        const response = await fetch(`${API_URL}/server`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ name, desc }),
            credentials: "include"
        });
        
        //console.log(response);
        return response;
    },

    //*----------------------------------------------------------------------------------------------------------------------------
};