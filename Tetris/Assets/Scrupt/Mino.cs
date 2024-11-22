using UnityEngine;
using UnityEngine.UI;

public class Mino : MonoBehaviour
{
    private ScoreManager scoreManager; // ScoreManager�̃C���X�^���X
    public float previousTime;
    public float fallTime = 1f;

    private static int width = 10;
    private static int height = 20;

    public Vector3 rotationPoint;

    // ���ʂ̃O���b�h
    public static Transform[,] grid = MoveOnlyMino.grid;

    private Vector3 originalScale;

    private bool HoldCount = false;

    public static bool hasOnlyTag = false;

    void Start()
    {
        originalScale = transform.localScale;
        scoreManager = FindObjectOfType<ScoreManager>(); // ScoreManager���擾
    }

    void Update()
    {
        MinoMovememt();
    }

    private void MinoMovememt()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (!ValidMovement())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!ValidMovement())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);

            if (!ValidMovement())
            {
                transform.position += new Vector3(0, +1, 0);

                AddToGrid();
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
            }

            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //���݂̈ʒu���牺�ɍ~����������
            while (ValidMovement())
            {
                transform.position += new Vector3(0, -1, 0);
            }

            //1���Ɉړ��������_�ŏՓ˂����̂Ō��̈ʒu�ɖ߂�
            transform.position += new Vector3(0, 1, 0); // �߂�

            AddToGrid();
            CheckLines();
            this.enabled = false;
            FindObjectOfType<SpawnMino>().NewMino();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }
        else if (Input.GetKeyDown(KeyCode.C) && !HoldCount) //�z�[���h�@�\���g���K�[
        {
            HoldMino();
        }
    }

    void HoldMino()
    {
        SpawnMino spawnMino = FindObjectOfType<SpawnMino>();
        if (spawnMino == null) return;

        if (spawnMino.holdMino == null)
        {
            //�z�[���h���̃~�m���Ȃ��ꍇ�A���݂̃~�m���z�[���h
            spawnMino.holdMino = this.gameObject;
            spawnMino.holdMino.SetActive(false); //�~�m���\����
            spawnMino.UpdateHoldMinoDisplay();   //�z�[���h�G���A�ɕ\��
            spawnMino.holdMino.SetActive(true);  //�~�m��\����
            FindObjectOfType<SpawnMino>().NewMino(); //�V�����~�m�𐶐�
        }
        else
        {
            //�z�[���h���̃~�m�ƌ��݂̃~�m�����ւ���
            GameObject temp = spawnMino.holdMino;
            spawnMino.holdMino = this.gameObject;
            spawnMino.holdMino.SetActive(false); //�z�[���h����~�m���\����
            spawnMino.UpdateHoldMinoDisplay();   //�z�[���h�G���A�ɕ\��
            spawnMino.holdMino.SetActive(true);  //�~�m��\����

            temp.SetActive(true); //���̃z�[���h�~�m��\��
            temp.transform.position = spawnMino.transform.position; //�ʒu�����Z�b�g
            temp.transform.localScale = originalScale;              //���̃X�P�[���ɖ߂�
            temp.GetComponent<Mino>().enabled = true;               //�~�m�̑����L����
        }

        //���݂̃~�m���z�[���h�G���A�Ɉړ�
        transform.localScale = originalScale;     //�X�P�[�������Z�b�g
        transform.rotation = Quaternion.identity; //��]�����Z�b�g
        this.enabled = false;     //���݂̃~�m�𖳌���
        HoldCount = true;
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            //������20�ȏ�Ȃ�Q�[���I�[�o�[���������s
            if (roundY >= height - 1)
            {
                GameOver();
                return;
            }

            grid[roundX, roundY] = children;
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");

        //�Q�[���I�[�o�[�t���O��ݒ�
        SpawnMino.isGameOver = true;

    }

    bool ValidMovement()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }

            if (grid[roundX, roundY] != null)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
                AddScore(50); // ���C������������50�|�C���g���Z
            }
        }
    }

    void AddScore(int points)
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(points); // ScoreManager�ɓ��_�����Z
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] != null)
            {
                // �^�O��"Only"�ł���΃t���O�𗧂Ă�
                if (grid[j, i].CompareTag("Only"))
                {
                    hasOnlyTag = true;
                }

                // �Q�[���I�u�W�F�N�g���폜
                Destroy(grid[j, i].gameObject);
                grid[j, i] = null;
            }
        }

        // "Only"�^�O�����I�u�W�F�N�g���������ꍇ�A�ua�v���o��
        if (hasOnlyTag)
        {
            SpawnMino.isGameOver = true;
        }
    }

    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }
}
