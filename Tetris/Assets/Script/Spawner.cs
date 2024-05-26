using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Block[] Blocks;

    //�����_���u���b�N
    Block GetRandomBlock()
    {
        int i = Random.Range(0, Blocks.Length);

        if (Blocks[i])
        {
            return Blocks[i];
        }
        else
        {
            return null;
        }
    }

    //�u���b�N����
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(), transform.position,
            Quaternion.identity);
        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
}
