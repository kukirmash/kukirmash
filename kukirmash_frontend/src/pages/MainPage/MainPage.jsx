import React, { useState } from "react";
import styles from "./MainPage.module.css";
import { Logo } from "../../components/Logo/Logo";
import { NavBar } from "../../modules/NavBar/NavBar";

export const MainPage = () => {

  return (
    <div className={styles.page}>
      <div className={styles.container}>
        <NavBar />
      </div>
    </div>
  );
};
