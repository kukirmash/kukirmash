import { useState, useEffect } from "react"

export const useAuthValidation = (initialValues) => {
	//*----------------------------------------------------------------------------------------------------------------------------
	const [values, setValues] = useState(initialValues)
	const [errors, setErrors] = useState({})
	const [isValid, setIsValid] = useState(false)

	//*----------------------------------------------------------------------------------------------------------------------------
	// Функция валидации всех полей
	// Валидируем только те поля, которые есть в форме
	const validate = (dataToValidate) => {
		const newErrors = {}
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

		if ("login" in dataToValidate && !dataToValidate.login.trim()) {
			newErrors.login = "Логин обязателен"
		}

		if (
			"email" in dataToValidate &&
			!emailRegex.test(dataToValidate.email)
		) {
			newErrors.email = "Некорректная почта"
		}

		if (
			"password" in dataToValidate &&
			dataToValidate.password.length < 8
		) {
			newErrors.password = "Минимум 8 символов"
		}

		// ЛОГИКА СРАВНЕНИЯ ПАРОЛЕЙ
		// Проверяем только если в форме есть оба поля
		if (
			"confirmPassword" in dataToValidate &&
			"password" in dataToValidate
		) {
			if (
				dataToValidate.confirmPassword &&
				dataToValidate.password !== dataToValidate.confirmPassword
			) {
				newErrors.confirmPassword = "Пароли не совпадают"
			}
		}

		if (
			"serverName" in dataToValidate &&
			!dataToValidate.serverName.trim()
		) {
			newErrors.serverName = "Название сервера обязательно"
		}

		return newErrors
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	// Обработчик изменений поля
	const handleChange = (name, value) => {
		// Используем setValues с callback, чтобы гарантированно иметь предыдущее состояние
		setValues((prevValues) => {
			const updatedValues = { ...prevValues, [name]: value }

			// Внутри обновления значений обновляем и ошибки
			setErrors((prevErrors) => {
				const newErrors = { ...prevErrors }

				// 1. Убираем ошибку самого поля, которое редактируем (например, "обязательное поле")
				delete newErrors[name]

				// 2. Специфичная логика для ПАРОЛЕЙ (работает в обе стороны)
				// Если меняем 'password' ИЛИ 'confirmPassword'
				if (name === "password" || name === "confirmPassword") {
					// Берем актуальные значения из updatedValues
					// (если меняем password, он уже в updatedValues, если confirm - тоже)
					const password = updatedValues.password
					const confirm = updatedValues.confirmPassword

					// Проверку делаем только если поле confirmPassword вообще существует и не пустое
					// (чтобы не ругаться на несовпадение, пока юзер еще не дошел до второго поля)
					if (confirm) {
						if (password !== confirm) {
							// Если отличаются - СТАВИМ ошибку
							newErrors.confirmPassword = "Пароли не совпадают"
						} else {
							// Если совпадают - УБИРАЕМ ошибку
							delete newErrors.confirmPassword
						}
					}
				}

				return newErrors
			})

			return updatedValues
		})
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	// Валидация одного поля
	const validateField = (name) => {
		const currentErrors = validate(values)

		if (currentErrors[name]) {
			setErrors((prev) => ({ ...prev, [name]: currentErrors[name] }))
		}
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	// Проверка валидности всех полей
	// Поля валидны, если нет ошибок И нет пустых полей
	useEffect(() => {
		const currentErrors = validate(values)

		// Список необязательных полей
		const optionalFields = ["serverDesc", "remember"]

		const hasErrors = Object.keys(currentErrors).length > 0

		// Проверяем на пустоту только ОБЯЗАТЕЛЬНЫЕ поля
		const hasEmptyRequiredFields = Object.entries(values).some(
			([key, val]) => {
				if (optionalFields.includes(key)) return false // Пропускаем необязательные
				return val === ""
			},
		)

		setIsValid(!hasErrors && !hasEmptyRequiredFields)
	}, [values])

	//*----------------------------------------------------------------------------------------------------------------------------
	// Функция для проверки всей формы перед отправкой
	const validateAllFields = () => {
		const formErrors = validate(values)
		setErrors(formErrors)

		return Object.keys(formErrors).length === 0
	}

	//*----------------------------------------------------------------------------------------------------------------------------
	return {
		values, // значения всех полей
		errors, // ошибки полей
		isValid, // данные поля валидны - т.е. нет ошибок И нет пустых полей
		handleChange, // обработчик изменений полей
		validateField, // проверка конкретного поля
		validateAllFields, // проверка всех полей
		setValues, //
	}
	//*----------------------------------------------------------------------------------------------------------------------------
}
