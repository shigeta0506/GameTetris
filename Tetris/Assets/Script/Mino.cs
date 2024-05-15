using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime;
    //�����鎞��
    public float fallTime = 1f;

    //��]
    public Vector3 rotationPoint;

    void Update()
    {
        MinoMovememt();
    }

    private void MinoMovememt()
    {
        //���ɓ���
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        //�E�ɓ���
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        //�����ŉ��Ɉړ��E�����L�[�ł��ړ�����
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            //X�L�[�������č���]������
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            //Z�L�[�������ĉE��]������
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }
    }
}