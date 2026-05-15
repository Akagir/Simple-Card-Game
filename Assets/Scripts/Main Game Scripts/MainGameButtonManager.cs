using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameButtonManager : MonoBehaviour
{
    public void BackMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenuScene");
    }
}