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
        Debug.Log("����� ����");
    }

    public void Continue() {
        Debug.Log("����������");
    }

    public void LoadGame() {
        Debug.Log("��������� ����������");
    }

    public void Settings() {
        Debug.Log("�����-������ ��� ��������, �� ������ ����� �� �������...\n� �� ������...\n� �� � ���� ����...\n� �� � ���� �����...");
    }

    public void Bonus() {
        Debug.Log("���������� ���� ��������");
    }

    public void Exit() {
        Debug.Log("�����");
        Application.Quit();
    }
}
