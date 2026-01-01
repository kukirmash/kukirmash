import { CancelButton } from "../../ui/CancelButton/CancelButton";
import styles from "./Dialog.module.css";
import error from "../../assets/error.svg"
import warning from "../../assets/warning.svg"
import ok from "../../assets/ok.svg"

function GetTitleByType(type)
{
  switch (type) {
    case "ok":
      return "Успешно";
  
    case "warning":
      return "Предупреждение";
  
    case "error":
      return "Oшибка";
  
    default:
      return ""
  }
}

function GetIconByType(type)
{
    switch (type) {
    case "ok":
      return ok;
  
    case "warning":
      return warning;
  
    case "error":
      return error;
  
    default:
      return ""
  }
}

export const Dialog = ({ type = "error", content = "Произошла неизвестная ошибка", onClose }) => {  
  return (
    <div className={styles.dialogBackground}>
      <div className={styles.dialog}>

        <div className={styles.dialogHeader}>
          <h2>{GetTitleByType(type)}</h2>
          <CancelButton onClick={onClose}></CancelButton>
        </div>

        <div className={styles.dialogContent}>
          <img className={styles.svg} alt= {type} src={GetIconByType(type)} />
          <p className = {styles.dialogText}>{content}</p>
        </div>

      </div>
    </div>
  );
};