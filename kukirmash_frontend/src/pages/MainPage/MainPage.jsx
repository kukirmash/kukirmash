import React, { useState } from "react";
import styles from "./MainPage.module.css";
import { Sidebar } from "../../modules/SideBar/SideBar";

export const MainPage = () => {
  return (
    <div className={styles.page}>
      <div className={styles.container}>
          <Sidebar>

          </Sidebar>
      </div>
    </div>
  );
};
