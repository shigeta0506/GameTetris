using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    private Transform[,] grid;

    //[SerializeField]
    //private Transform emptySprite;

    [SerializeField]
    private int height = 20, width = 10/*, header = 8*/;

    //private void Start()
    //{
    //    CreateBorad();
    //}

    //�{�[�h�����֐�
    //void CreateBorad()
    //{
    //    if(emptySprite)
    //    {
    //        for (int y = 0; y < height - header; y++)
    //        {
    //            for (int x = 0; x < width; x++)
    //            {
    //                Transform clone = Instantiate(emptySprite, 
    //                    new Vector3(x, y, 0),Quaternion.identity);

    //                clone.transform.parent = transform;
    //            }
    //        }
    //    }
    //}

    //�g���Ƀu���b�N�����邩

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    public bool CheckPosition(Block block)
    {
        foreach  (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!BoardOutCheck((int)pos.x,(int)pos.y)) 
            {
                return false;
            }

            if (BlockCheck((int)pos.x,(int)pos.y,block))
            {
                return false;
            }
        }

        return true;
    }

    //�g���ɂ��邩����
    public bool BoardOutCheck(int x,int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    //�񎟌��z�񂪋�ł��邩&�e���Ⴄ��
    public bool BlockCheck(int x, int y,Block block)
    {
        return(grid[x,y] != null && grid[x,y].parent != block.transform);
    }

    public void SaveBlockInGrid(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }
}
