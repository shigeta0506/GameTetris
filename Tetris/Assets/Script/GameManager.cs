using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    Spawner spawner;
    Block activeBlock;

    [SerializeField]
    private float dropInterval = 0.25f;
    float nextdropTimer;

    Board board;

    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;

    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;


    //スポナーオブジェクトをスポナー変数に格納
    private void Start()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();

        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);

        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;


        //スポナークラスからブロック生成関数を呼んで変数に格納
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

    }

    private void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        //右に移動
        if (Input.GetKey(KeyCode.RightArrow) && (Time.time > nextKeyLeftRightTimer) 
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            activeBlock.MoveRight();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }

        //左に移動
        else if (Input.GetKey(KeyCode.LeftArrow) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            activeBlock.MoveLeft();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }

        //下に移動
        else if (Input.GetKey(KeyCode.DownArrow) && (Time.time > nextKeyDownTimer)
            || Time.time > nextdropTimer)
        {
            activeBlock.MoveDown();

            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if (!board.CheckPosition(activeBlock))
            {
                BottomBorad();
            }
        }

        //右に回転
        else if (Input.GetKey(KeyCode.X) && (Time.time > nextKeyRotateTimer))
        {
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }

        //左に回転
        else if (Input.GetKey(KeyCode.Z) && (Time.time > nextKeyRotateTimer))
        {
            activeBlock.RotateLeft();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateRight();
            }
        }

    }

    void BottomBorad()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;
    }
}
