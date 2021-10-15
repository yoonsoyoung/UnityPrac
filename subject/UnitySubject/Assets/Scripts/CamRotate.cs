using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // �̶� class ��� ����Ƽ ��ũ��Ʈ ���ϸ�� ��ġ�ؾ� ��!
    // �ܺ� ���� �ҽ� �ܾ�� �� ����

    public float rotSpeed = 200f;
    // ȸ�� �� ����
    float mx = 0;
    float my = 0;

    // Start is called before the first frame update
    // ù ������ ������Ʈ �� ȣ��
    void Start()
    {
    }

    // Update is called once per frame
    // �� �����Ӹ��� �ǽð� ȣ��
    void Update()
    {
        // ������� ���콺 �Է��� �޾Ƽ� ��ü�� ȸ��
        float mouse_X = Input.GetAxis("Mouse X"); // �� ��(GetAxis)�� �޾ƿ��µ�(Input) Mouse X ��� ��(Input Manager�� ����)
        float mouse_Y = Input.GetAxis("Mouse Y");

        // ���콺 �Է� ���� �̿��� ȸ�� ������ ����
        // Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0); // X��ǥ���� -mouse_Y, y��ǥ���� mouse_X
        // why? x���� ȸ����Ű�� y�� �������� ���, ������ ������ ��ȭ / ���� �ϸ� x ���, �Ʒ��� �ϸ� x ����
        // x���� Y�� ���� ��ȭ, y���� X�� ���� ��ȭ

        // ȸ�� �������� ��ü ȸ��
        //transform.eulerAngles += dir * roSpeed * Time.deltaTime; // Time.deltaTime �ð� �����Ӻ�

        // x�� ȸ�� ���� -90��~90�� ���̷� ����
        //Vector3 rot = transform.eulerAngles;
        //rot.x = Mathf.Clamp(rot.x, -90f, 90f);
        //transform.eulerAngles = rot;

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90f, 90f);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
