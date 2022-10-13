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

    [SerializeField] private GameObject LoadCanvas;
    [SerializeField] private GameObject ButtonCanvas;

    private ISaveSystem _saveSystem;
    private bool _autosaveExist = false;

    private void Awake()
    {
        _saveSystem = new JSONSaveSystem();
        _autosaveExist = _saveSystem.SavingExists();
        if (_autosaveExist)
            _continueBtn.gameObject.SetActive(true);
        else 
            _continueBtn.gameObject.SetActive(false);
    }

    public void NewGame() {
        // Надо создавать новый .json файл типа Autosave.json.
        // При этом в Awake загружаемой сцены устанавливать данные из Autosave.json 
        //Debug.Log("Новая игра");
        _saveSystem.AutoSave(_saveSystem.CreateStartSave());
        DeloadAnimation();
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
    }

    public void Continue() {
        // Если нет файла Autosave.json, то кнопка вне времени и пространства.
        // Если есть, то загрузить сцену и в сцене установить данные из Autosave.json
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        DeloadAnimation();
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
        DeloadAnimation();
        AsyncOperation operation =  SceneManager.LoadSceneAsync(3);
    }

    private void DeloadAnimation() {
        LoadCanvas.GetComponent<Animator>().SetBool("Deload", true);
        ButtonCanvas.GetComponent<Animator>().SetBool("Deload", true);
    }

    public void Exit() {
        // Выход
        Debug.Log("Выход");
        Application.Quit();
    }
}
