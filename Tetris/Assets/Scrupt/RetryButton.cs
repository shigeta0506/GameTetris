using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    // この関数はボタンが押されたときに呼び出される
    public void OnRetryButtonPressed()
    {
        // 現在のシーンをリロードする
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
