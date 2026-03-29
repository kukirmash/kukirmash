import { useState, useEffect } from "react"

/**
 * Универсальный хук для управления состоянием любой формы и её валидацией.
 * Реализует паттерн "Инверсия управления": сам хук не знает правил валидации,
 * он лишь хранит состояние и вызывает переданную ему функцию проверки.
 *
 * Входные параметры:
 * @param {Object} initialValues - Начальные значения полей формы (например, { login: "", password: "" }).
 * @param {Function} validateFn - Функция валидации. Принимает текущие `values` и возвращает объект с ошибками (например, { login: "Обязательное поле" }). Если ошибок нет, должна вернуть пустой объект {}.
 *
 * Выходные данные (возвращаемый объект):
 * @returns {Object} formMethods.values - Текущие значения всех полей формы.
 * @returns {Object} formMethods.errors - Текущие ошибки валидации полей.
 * @returns {boolean} formMethods.isValid - Флаг валидности формы (true, если объект ошибок пустой). (для disabled кнопки submit)
 * @returns {Function} formMethods.handleChange - Функция для обновления конкретного поля: (name, value) => void.
 * @returns {Function} formMethods.validateField - Функция проверки одного поля (для onBlur).
 * @returns {Function} formMethods.validateAllFields - Функция проверки всех полей разом. Возвращает true (успех) или false (ошибка). (для onSubmit).
 * @returns {Function} formMethods.setValues - Прямой сеттер для ручной перезаписи всего объекта значений.
 */
export const useForm = (initialValues, validateFn) => {

	//----------------------------------------------------------------------------------------------------------------------------
	const [values, setValues] = useState(initialValues)
	const [errors, setErrors] = useState({})
	const [isValid, setIsValid] = useState(false)

	//----------------------------------------------------------------------------------------------------------------------------
	// Пересчитываем валидность формы при каждом изменении значений
	useEffect(() => {
		if (validateFn) {
			const currentErrors = validateFn(values)
			// Форма валидна, если объект ошибок пустой
			setIsValid(Object.keys(currentErrors).length === 0)
		} else {
			setIsValid(true) // Если правил нет, форма всегда валидна
		}
	}, [values, validateFn])

	//----------------------------------------------------------------------------------------------------------------------------
	// Обработчик ввода
	const handleChange = (name, value) => {
		setValues((prev) => ({ ...prev, [name]: value }))

		// Очищать ошибку поля, когда пользователь начинает в нем печатать
		if (errors[name]) {
			setErrors((prev) => ({ ...prev, [name]: undefined }))
		}
	}

	//----------------------------------------------------------------------------------------------------------------------------
	// Валидация конкретного поля (для onBlur)
	const validateField = (name) => {
		if (validateFn) {
			const currentErrors = validateFn(values)
			setErrors((prev) => ({ ...prev, [name]: currentErrors[name] }))
		}
	}
	//----------------------------------------------------------------------------------------------------------------------------
	// Валидация всей формы (при нажатии на Submit)
	const validateAllFields = () => {
		if (validateFn) {
			const formErrors = validateFn(values)
			setErrors(formErrors)
			return Object.keys(formErrors).length === 0
		}
		return true
	}

	//----------------------------------------------------------------------------------------------------------------------------
	return {
		values,
		errors,
		isValid,
		handleChange,
		validateField,
		validateAllFields,
		setValues,
	}

	//----------------------------------------------------------------------------------------------------------------------------
}