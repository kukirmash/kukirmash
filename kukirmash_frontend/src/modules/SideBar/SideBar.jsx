import React from "react";
import styles from "./SideBar.module.css";
import { ServerButton } from "../../components/ServerButton/ServerButton";


export const Sidebar = () => {
    return (
        <div className={styles.sidebar}>
            
            <div className={styles.serverList}>
                <ServerButton onClick={() => console.log("Server 1 clicked")} />
                <ServerButton onClick={() => console.log("Server 2 clicked")} />
            </div>


            <div className={styles.separator}></div>

        </div>
    );
};