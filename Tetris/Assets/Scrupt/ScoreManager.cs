using UnityEngine;
using UnityEngine.UI;  // Text���g�p���邽�߂ɕK�v

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;  // UI Text��Inspector�Őݒ肷��
    private int score = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    // ���_�����Z���郁�\�b�h
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    // UI���X�V���郁�\�b�h
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        if(score >= 50)
        {
            SpawnMino.isClear = true;
        }
    }
}
