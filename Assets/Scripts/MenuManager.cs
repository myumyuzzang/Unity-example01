using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnPressGameStart()
    {
        SceneManager.LoadScene("GameScene");
        // 씬 넘어가기. 씬매니저라는 클래스 호출 후 .로드씬("씬이름");
    }

    public void OnPressExit()
    {
        Application.Quit(); // 게임 끝내는 명령어
    }
}
