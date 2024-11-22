using UnityEngine;
using UnityEngine.UI;

public class Mino : MonoBehaviour
{
    private ScoreManager scoreManager; // ScoreManagerのインスタンス
    public float previousTime;
    public float fallTime = 1f;

    private static int width = 10;
    private static int height = 20;

    public Vector3 rotationPoint;

    // 共通のグリッド
    public static Transform[,] grid = MoveOnlyMino.grid;

    private Vector3 originalScale;

    private bool HoldCount = false;

    public static bool hasOnlyTag = false;

    void Start()
    {
        originalScale = transform.localScale;
        scoreManager = FindObjectOfType<ScoreManager>(); // ScoreManagerを取得
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
            //現在の位置から下に降下し続ける
            while (ValidMovement())
            {
                transform.position += new Vector3(0, -1, 0);
            }

            //1つ下に移動した時点で衝突したので元の位置に戻す
            transform.position += new Vector3(0, 1, 0); // 戻す

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
        else if (Input.GetKeyDown(KeyCode.C) && !HoldCount) //ホールド機能をトリガー
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
            //ホールド中のミノがない場合、現在のミノをホールド
            spawnMino.holdMino = this.gameObject;
            spawnMino.holdMino.SetActive(false); //ミノを非表示に
            spawnMino.UpdateHoldMinoDisplay();   //ホールドエリアに表示
            spawnMino.holdMino.SetActive(true);  //ミノを表示に
            FindObjectOfType<SpawnMino>().NewMino(); //新しいミノを生成
        }
        else
        {
            //ホールド中のミノと現在のミノを入れ替える
            GameObject temp = spawnMino.holdMino;
            spawnMino.holdMino = this.gameObject;
            spawnMino.holdMino.SetActive(false); //ホールドするミノを非表示に
            spawnMino.UpdateHoldMinoDisplay();   //ホールドエリアに表示
            spawnMino.holdMino.SetActive(true);  //ミノを表示に

            temp.SetActive(true); //元のホールドミノを表示
            temp.transform.position = spawnMino.transform.position; //位置をリセット
            temp.transform.localScale = originalScale;              //元のスケールに戻す
            temp.GetComponent<Mino>().enabled = true;               //ミノの操作を有効化
        }

        //現在のミノをホールドエリアに移動
        transform.localScale = originalScale;     //スケールをリセット
        transform.rotation = Quaternion.identity; //回転をリセット
        this.enabled = false;     //現在のミノを無効化
        HoldCount = true;
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            //高さが20以上ならゲームオーバー処理を実行
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

        //ゲームオーバーフラグを設定
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
                AddScore(50); // ラインが消えたら50ポイント加算
            }
        }
    }

    void AddScore(int points)
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(points); // ScoreManagerに得点を加算
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
                // タグが"Only"であればフラグを立てる
                if (grid[j, i].CompareTag("Only"))
                {
                    hasOnlyTag = true;
                }

                // ゲームオブジェクトを削除
                Destroy(grid[j, i].gameObject);
                grid[j, i] = null;
            }
        }

        // "Only"タグを持つオブジェクトがあった場合、「a」を出力
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
