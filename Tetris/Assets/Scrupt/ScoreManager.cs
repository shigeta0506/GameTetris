using UnityEngine;
using UnityEngine.UI;  // Textを使用するために必要

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;  // UI TextをInspectorで設定する
    private int score = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    // 得点を加算するメソッド
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    // UIを更新するメソッド
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        if(score >= 50)
        {
            SpawnMino.isClear = true;
        }
    }
}
