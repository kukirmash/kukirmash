import React, { useState } from "react";
import styles from "./AddServerFormDialog.module.css";
import dialogStyles from "../Dialog/Dialog.module.css"
import { CancelButton } from "../../ui/CancelButton/CancelButton"
import { Input } from "../../ui/Input/Input";
import { Button } from "../../ui/Button/Button";

export const AddServerFormDialog = () => {

    const [name, setName] = useState("");
    const [desc, setDesc] = useState("");

    return (
        <>
            <div className={dialogStyles.dialogBackground}>
                <div className={dialogStyles.dialog}>

                    <div className={dialogStyles.dialogHeader}>
                        <h2>Добавить сервер</h2>
                        <CancelButton onClick={() => { }}></CancelButton>
                    </div>

                    <div className={dialogStyles.dialogContent}>
                        <form className={styles.form}>
                            <Input
                                label="Название"
                                type="text"
                                value={name}
                                onChange={setName}
                                placeholder="Введите название"
                                onBlur={() => { }}
                            />
                            <Input
                                label="Описание"
                                type="text"
                                value={desc}
                                onChange={setDesc}
                                placeholder="Введите описание"
                                onBlur={() => { }}
                            />

                            <Button type="submit">Создать</Button>

                        </form>
                    </div>
                </div>
            </div>
        </>
    );
};