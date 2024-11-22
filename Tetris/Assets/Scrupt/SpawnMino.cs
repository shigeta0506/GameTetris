using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos; //�~�m�̃v���n�u�z��
    public GameObject holdMino; //�z�[���h���̃~�m
    public static bool isGameOver = false; //�Q�[���I�[�o�[�t���O
    public static bool isClear = false; //�Q�[���N���A�t���O
    private Vector3 holdPosition = new Vector3(-5, 4, 0); //�z�[���h�G���A�̕\���ʒu
    private bool HoldCount = false;
    [SerializeField] Text gameOverText;
    [SerializeField] Text ClearText;
    [SerializeField] Button retryButton; // Retry�{�^���̎Q��
    [SerializeField] Button endButton; // End�{�^���̎Q��

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
            retryButton.gameObject.SetActive(false); // �Q�[���J�n���̓{�^�����\��
            retryButton.onClick.AddListener(OnRetryButtonPressed); // �{�^���������ꂽ�Ƃ��̏���
        }

        if (endButton != null)
        {
            endButton.gameObject.SetActive(false); // �Q�[���J�n���̓{�^�����\��
            endButton.onClick.AddListener(OnEndButtonPressed); // �{�^���������ꂽ�Ƃ��̏���
        }

        NewMino();
    }

    public void NewMino()
    {
        // �Q�[���I�[�o�[�̏ꍇ�͐V�����~�m�𐶐����Ȃ�
        if (isGameOver)
        {
            if (gameOverText != null)
            {
                gameOverText.gameObject.SetActive(true); // GameOver�̃��b�Z�[�W��\��
            }
            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true); // Retry�{�^����\��
            }
            if (endButton != null)
            {
                endButton.gameObject.SetActive(true); // End�{�^����\��
            }
            return;
        }
        else if (isClear)
        {
            if (ClearText != null)
            {
                ClearText.gameObject.SetActive(true); // �N���A���b�Z�[�W��\��
            }
            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true); // Retry�{�^����\��
            }
            if (endButton != null)
            {
                endButton.gameObject.SetActive(true); // End�{�^����\��
            }
            return;
        }

        // �V�����~�m�𐶐�
        Instantiate(Minos[Random.Range(0, Minos.Length)], transform.position, Quaternion.identity);
    }

    public void UpdateHoldMinoDisplay()
    {
        if (holdMino != null)
        {
            // �z�[���h���̃~�m���z�[���h�G���A�Ɉړ�
            holdMino.transform.position = holdPosition;
        }
    }

    // Retry�{�^���������ꂽ�Ƃ��ɌĂ΂�鏈��
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

    // End�{�^���������ꂽ�Ƃ��ɌĂ΂�鏈��
    void OnEndButtonPressed()
    {
        // End �X�N���v�g�̃��\�b�h���Ăяo��
        FindObjectOfType<End>().OnEndButtonPressed();
    }
}
