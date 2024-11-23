using UnityEngine;
using UnityEngine.UI;  // Textを使用するために必要

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;  // UI TextをInspectorで設定する
    private int score = 500;

    void Start()
    {
        UpdateScoreUI();
    }

    // 得点を加算するメソッド
    public void AddScore(int points)
    {
        score -= points;
        UpdateScoreUI();
    }

    // UIを更新するメソッド
    private void UpdateScoreUI()
    {
        if (score >= 0)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        if(score == 0)
        {
            SpawnMino.isClear = true;
        }
    }
}
