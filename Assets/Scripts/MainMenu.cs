using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button _newGameBtn;
    [SerializeField] private Button _loadGameBtn;
    [SerializeField] private Button _continueBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _quitBtn;
    [SerializeField] private Button _bonusBtn;

    private ISaveSystem _saveSystem;
    private string _savePath;

    private void Awake()
    {
        _saveSystem = new JSONSaveSystem();
        if (_savePath == null)
            _continueBtn.gameObject.SetActive(false);
        else 
            _continueBtn.gameObject.SetActive(true);
    }

    public void NewGame() {
        // Надо создавать новый .json файл типа Autosave.json.
        // При этом в Awake загружаемой сцены устанавливать данные из Autosave.json 
        Debug.Log("Новая игра");
        SceneManager.LoadScene(1);
    }

    public void Continue() {
        // Если нет файла Autosave.json, то кнопка вне времени и пространства.
        // Если есть, то загрузить сцену и в сцене установить данные из Autosave.json
        Debug.Log("Продолжить");
    }

    public void LoadGame() {
        // Необходимо меню выбора сохранений.

        // После выбора сохранения Autosave.json перезаписывается с данными выбранного сохранения.
        // Далее происходит всё тоже, что и при кнопке Continue. 
        Debug.Log("Загрузить сохранение");
    }

    public void Settings() {
        //Дебаг тут красноречив
        Debug.Log("Когда-нибудь они появятся, но скорее всего не сегодня...\nИ не завтра...\nИ не в этом году...\nИ не в этой жизни...");
    }

    public void Bonus() {
        // Возможно стоит добавить какое-то меню с настройкой сцены...
        Debug.Log("Адекватным вход запрещён");
        SceneManager.LoadScene(3);
    }

    public void Exit() {
        // Выход
        Debug.Log("Выход");
        Application.Quit();
    }
}
