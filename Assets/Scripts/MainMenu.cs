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
        // ���� ��������� ����� .json ���� ���� Autosave.json.
        // ��� ���� � Awake ����������� ����� ������������� ������ �� Autosave.json 
        //Debug.Log("����� ����");
        _saveSystem.AutoSave(_saveSystem.CreateStartSave());
        SceneManager.LoadSceneAsync(1);
    }

    public void Continue() {
        // ���� ��� ����� Autosave.json, �� ������ ��� ������� � ������������.
        // ���� ����, �� ��������� ����� � � ����� ���������� ������ �� Autosave.json
        SceneManager.LoadSceneAsync(1);
        Debug.Log("����������");
    }

    public void LoadGame() {
        // ���������� ���� ������ ����������.

        // ����� ������ ���������� Autosave.json ���������������� � ������� ���������� ����������.
        // ����� ���������� �� ����, ��� � ��� ������ Continue. 
        Debug.Log("��������� ����������");
    }

    public void Settings() {
        //����� ��� �����������
        Debug.Log("�����-������ ��� ��������, �� ������ ����� �� �������...\n� �� ������...\n� �� � ���� ����...\n� �� � ���� �����...");
    }

    public void Bonus() {
        // �������� ����� �������� �����-�� ���� � ���������� �����...
        Debug.Log("���������� ���� ��������");
        SceneManager.LoadScene(3);
    }

    public void Exit() {
        // �����
        Debug.Log("�����");
        Application.Quit();
    }
}
