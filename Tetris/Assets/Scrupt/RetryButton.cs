using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    // ���̊֐��̓{�^���������ꂽ�Ƃ��ɌĂяo�����
    public void OnRetryButtonPressed()
    {
        // ���݂̃V�[���������[�h����
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
