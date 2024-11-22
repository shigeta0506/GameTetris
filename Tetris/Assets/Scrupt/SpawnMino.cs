using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos; //ミノのプレハブ配列
    public GameObject holdMino; //ホールド中のミノ
    public static bool isGameOver = false; //ゲームオーバーフラグ
    public static bool isClear = false; //ゲームクリアフラグ
    private Vector3 holdPosition = new Vector3(-5, 4, 0); //ホールドエリアの表示位置
    private bool HoldCount = false;
    [SerializeField] Text gameOverText;
    [SerializeField] Text ClearText;
    [SerializeField] Button retryButton; // Retryボタンの参照
    [SerializeField] Button endButton; // Endボタンの参照

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (ClearText != null)
        {
            ClearText.gameObject.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false); // ゲーム開始時はボタンを非表示
            retryButton.onClick.AddListener(OnRetryButtonPressed); // ボタンが押されたときの処理
        }

        if (endButton != null)
        {
            endButton.gameObject.SetActive(false); // ゲーム開始時はボタンを非表示
            endButton.onClick.AddListener(OnEndButtonPressed); // ボタンが押されたときの処理
        }

        NewMino();
    }

    public void NewMino()
    {
        // ゲームオーバーの場合は新しいミノを生成しない
        if (isGameOver)
        {
            if (gameOverText != null)
            {
                gameOverText.gameObject.SetActive(true); // GameOverのメッセージを表示
            }
            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true); // Retryボタンを表示
            }
            if (endButton != null)
            {
                endButton.gameObject.SetActive(true); // Endボタンを表示
            }
            return;
        }
        else if (isClear)
        {
            if (ClearText != null)
            {
                ClearText.gameObject.SetActive(true); // クリアメッセージを表示
            }
            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true); // Retryボタンを表示
            }
            if (endButton != null)
            {
                endButton.gameObject.SetActive(true); // Endボタンを表示
            }
            return;
        }

        // 新しいミノを生成
        Instantiate(Minos[Random.Range(0, Minos.Length)], transform.position, Quaternion.identity);
    }

    public void UpdateHoldMinoDisplay()
    {
        if (holdMino != null)
        {
            // ホールド中のミノをホールドエリアに移動
            holdMino.transform.position = holdPosition;
        }
    }

    // Retryボタンが押されたときに呼ばれる処理
    void OnRetryButtonPressed()
    {
        gameOverText.gameObject.SetActive(false);
        ClearText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);

        isGameOver = false;
        isClear = false;
        Mino.hasOnlyTag = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Endボタンが押されたときに呼ばれる処理
    void OnEndButtonPressed()
    {
        // End スクリプトのメソッドを呼び出し
        FindObjectOfType<End>().OnEndButtonPressed();
    }
}
