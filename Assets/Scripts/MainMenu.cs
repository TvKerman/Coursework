using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
    [SerializeField] private AudioSource mainTheme;
    [SerializeField] private VideoPlayer videoPlayer;

    private long countFrame;
    private ISaveSystem _saveSystem;
    private bool _autosaveExist = false;

    private void Awake()
    {
        countFrame = (long)videoPlayer.frameCount - 1;
        _saveSystem = new JSONSaveSystem();
        _autosaveExist = _saveSystem.SavingExists();

        if (_autosaveExist)
            _continueBtn.gameObject.SetActive(true);
        else 
            _continueBtn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if ((videoPlayer.frame) >= countFrame) { 
            videoPlayer.Stop();
            ButtonCanvas.SetActive(true);
            videoPlayer.gameObject.SetActive(false);
            mainTheme.Play();

        }
    }

    public void NewGame() {
        // ���� ��������� ����� .json ���� ���� Autosave.json.
        // ��� ���� � Awake ����������� ����� ������������� ������ �� Autosave.json 
        //Debug.Log("����� ����");
        _saveSystem.AutoSave(_saveSystem.CreateStartSave());
        DeloadAnimation();
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
    }

    public void Continue() {
        // ���� ��� ����� Autosave.json, �� ������ ��� ������� � ������������.
        // ���� ����, �� ��������� ����� � � ����� ���������� ������ �� Autosave.json
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        DeloadAnimation();
        Debug.Log("����������");
    }

    //public void LoadGame() {
    //    // ���������� ���� ������ ����������.
    //
    //    // ����� ������ ���������� Autosave.json ���������������� � ������� ���������� ����������.
    //    // ����� ���������� �� ����, ��� � ��� ������ Continue. 
    //    Debug.Log("��������� ����������");
    //}

    public void Settings() {
        //����� ��� �����������
        Debug.Log("�����-������ ��� ��������, �� ������ ����� �� �������...\n� �� ������...\n� �� � ���� ����...\n� �� � ���� �����...");
    }

    public void Bonus() {
        // �������� ����� �������� �����-�� ���� � ���������� �����...
        Debug.Log("���������� ���� ��������");
        DeloadAnimation();
        AsyncOperation operation =  SceneManager.LoadSceneAsync(3);
    }

    public void Titles() {
        mainTheme.Stop();
        ButtonCanvas.SetActive(false);
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
    }

    private void DeloadAnimation() {
        LoadCanvas.GetComponent<Animator>().SetBool("Deload", true);
        //AnimationButtonCanvas.GetComponent<Animation>().Play();
        ButtonCanvas.SetActive(false);
    }

    public GameObject Buttons {
        get { return ButtonCanvas; }
    }

    public void Exit() {
        // �����
        Debug.Log("�����");
        Application.Quit();
    }
}
