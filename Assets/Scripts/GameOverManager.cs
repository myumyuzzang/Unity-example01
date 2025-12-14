using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void OnPressPlayAgain()
    {
        Debug.Log("버튼 눌림!");
        SceneManager.LoadScene("GameScene");
    }

    public void OnPressMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
