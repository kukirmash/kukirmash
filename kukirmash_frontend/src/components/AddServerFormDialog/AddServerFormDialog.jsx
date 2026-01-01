import React, { useState } from "react";
import styles from "./AddServerFormDialog.module.css";
import dialogStyles from "../Dialog/Dialog.module.css"
import { CancelButton } from "../../ui/CancelButton/CancelButton"
import { Input } from "../../ui/Input/Input";
import { Button } from "../../ui/Button/Button";
import { Dialog } from "../Dialog/Dialog";
import { ServerService } from "../../services/ServerService";

export const AddServerFormDialog = ({ onClose }) => {

    //*----------------------------------------------------------------------------------------------------------------------------
    const [name, setName] = useState("");
    const [desc, setDesc] = useState("");

    const [infoDialog, setInfoDialog] = useState({
        isOpen: false,
        type: "error",
        content: ""
    });

    const [errors, setErrors] = useState({});
    const [isAdded, setIsAdded] = useState(false);

    //*----------------------------------------------------------------------------------------------------------------------------
    // Проверка перед отправкой запроса (onSubmit у формы)
    const validateAllFields = () => {
        const newErrors = {};

        if (!name.trim())
            newErrors.name = "Название обязателено";

        setErrors(newErrors);

        return Object.keys(newErrors).length === 0;
    };

    //*----------------------------------------------------------------------------------------------------------------------------
    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateAllFields())
            return;

        try {
            // Отправка запроса
            const response = await ServerService.add({ name, desc });

            if (response.status === 401) {
                throw new Error("Вы не авторизованы. Пожалуйста, войдите снова.");
            }

            if (!response.ok) {
                // Пытаемся прочитать JSON ошибки от сервера
                const errorData = await response.json();
                throw new Error(errorData.detail || "Ошибка при создании сервера");
            }

            // УСПЕХ
            setInfoDialog({
                isOpen: true,
                type: "ok",
                content: "Сервер успешно создан!"
            });
            // Не закрываем форму сразу, ждем пока пользователь нажмет ОК на диалоге успеха
        }
        catch (err) {
            // ОШИБКА
            console.error(err);
            setInfoDialog({
                isOpen: true,
                type: "error",
                content: err.message || "Не удалось соединиться с сервером"
            });
        }
    };

    //*----------------------------------------------------------------------------------------------------------------------------
    const validateField = (name, value) => {
        const fieldErrors = {
            name: "",
        };

        if (name === "name" && !value.trim()) {
            fieldErrors.name = "Название обязателено";
        }

        setErrors(prev => ({ ...prev, [name]: fieldErrors[name] }));
    };

    //*----------------------------------------------------------------------------------------------------------------------------
    // Обработчик закрытия информационного диалога
    const handleInfoDialogClose = () => {
        setInfoDialog({ ...infoDialog, isOpen: false });

        // Если это был успех, закрываем и саму форму создания сервера
        if (infoDialog.type === "ok") {
            onClose();
            // Тут можно добавить логику обновления списка серверов (callback)
        }
    };

    //*----------------------------------------------------------------------------------------------------------------------------
    return (
        <>
            {infoDialog.isOpen && (
                <div style={{ zIndex: 1000, position: 'absolute' }}> {/* z-index чтобы был выше формы */}
                    <Dialog
                        type={infoDialog.type}
                        content={infoDialog.content}
                        onClose={handleInfoDialogClose}
                    />
                </div>
            )}

            <div className={dialogStyles.dialogBackground}>
                <div className={dialogStyles.dialog}>

                    <div className={dialogStyles.dialogHeader}>
                        <h2>Добавить сервер</h2>
                        <CancelButton onClick={onClose}></CancelButton>
                    </div>

                    <div className={dialogStyles.dialogContent}>
                        <form className={styles.form} onSubmit={handleSubmit}>
                            <Input
                                label="Название"
                                type="text"
                                value={name}
                                onChange={setName}
                                placeholder="Введите название"
                                onBlur={() => { validateField("name", name) }}
                            />

                            {errors.name && <p className={styles.error}>{errors.name}</p>}

                            <Input
                                label="Описание"
                                type="text"
                                value={desc}
                                onChange={setDesc}
                                placeholder="Введите описание"
                            />

                            <Button type="submit">Создать</Button>

                        </form>
                    </div>
                </div>
            </div>
        </>
    );
};