// Импортируем API_URL из файла, где он у вас определен глобально (или из process.env)
import { API_URL } from "./ServerService"

export const TextMessageService = {
	//----------------------------------------------------------------------------------------------------------------------------
	// Получение истории сообщений
	async getChannelMessages(serverId, channelId, count = 50) {
		const response = await fetch(
			`${API_URL}/servers/${serverId}/text-channels/${channelId}/messages/${count}`,
			{
				method: "GET",
				headers: {
					"Content-Type": "application/json",
				},
				credentials: "include", // Важно для куки/авторизации
			},
		)

		if (!response.ok) {
			throw new Error("Не удалось загрузить историю сообщений")
		}

		return await response.json()
	},

	//----------------------------------------------------------------------------------------------------------------------------
}
