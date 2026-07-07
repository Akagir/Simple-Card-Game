using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject SettingsPanel;

    void Start()
    {
    }

    public void StartMainGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadSceneAsync("MainGameScene");        
    }

    public void OpenSettingsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SettingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SettingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        Application.Quit();
    }
}
