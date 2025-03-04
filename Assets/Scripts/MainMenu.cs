using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("DinoGame"); // Change "GameScene" to your actual game scene name.
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!"); // Only works in a built application.
    }
}
