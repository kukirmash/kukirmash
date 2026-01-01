import React, { useState } from "react";
import styles from "./MainPage.module.css";
import { ServerSideBar } from "../../modules/ServerSideBar/ServerSideBar";

export const MainPage = () => {
  return (
    <div className={styles.page}>
      <div className={styles.container}>
          <ServerSideBar/>

      </div>
    </div>
  );
};
