import React, { useState } from "react";
import { Link } from "react-router-dom";
import styles from "./Header.module.css";
import { Logo } from "../../components/Logo/Logo";
import { Button } from "../../ui/Button/Button";


export const Header = () => {
    return (
        <nav className={styles.navbar}>
            <Logo imgSize={48} fontSize={32} />
            <div className={styles.navButtons}>
                <Link to="/register">
                    <Button variant="secondary">Регистрация</Button>
                </Link>

                <Link to="/login">
                    <Button variant="primary">Войти</Button>
                </Link>
            </div>

        </nav>
    );
};