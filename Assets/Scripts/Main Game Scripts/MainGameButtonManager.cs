using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameButtonManager : MonoBehaviour
{
    public void BackMainMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadSceneAsync("MainMenuScene");
    }
}