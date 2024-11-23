using UnityEngine;
using UnityEngine.UI;  // Text���g�p���邽�߂ɕK�v

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;  // UI Text��Inspector�Őݒ肷��
    private int score = 500;

    void Start()
    {
        UpdateScoreUI();
    }

    // ���_�����Z���郁�\�b�h
    public void AddScore(int points)
    {
        score -= points;
        UpdateScoreUI();
    }

    // UI���X�V���郁�\�b�h
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
