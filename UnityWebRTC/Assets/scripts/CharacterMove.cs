using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 7f;
    CharacterController cc; // ĳ������ ���� �Ӽ�

    // �߷� ����
    float gravity = -30f;

    // ���� �ӷ�
    float yVelocity = 0;

    // ������ ����
    public float jumpPower = 10f;

    // ���� ���� ���� : bool ->> true / false
    public bool isJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // Input Manager�� Horizontal ���� ������ 
        float v = Input.GetAxis("Vertical");

        // �̵� ���� ����
        Vector3 dir = new Vector3(h, 0, v); // x��ǥ�� ������, Vertical�� z��
        dir = dir.normalized; // normalized : ���� ������ ����ȭ(��� ������ ���� ���̸� 1�� �Ͽ� ���⿡ ���� �̵� �ӵ��� ���� ��)

        // ���� ī�޶� �������� ������ ��ȯ
        dir = Camera.main.transform.TransformDirection(dir);

        transform.position += dir * moveSpeed * Time.deltaTime; // deltaTime�� ���� �ð��� ������ Ÿ������ ����ȭ ����
        transform.position = transform.position + dir * moveSpeed * Time.deltaTime;
        // A = A + B;
        // A += B;

        // ���� ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }

        // ���� Ű���� �����̽� �ٸ� �Է��߰�, ������ ���� ���� ���¶��
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // ĳ���� ���� �ӵ��� �������� �����ϰ� ���� ���·� ����
            yVelocity = jumpPower;
            isJumping = true;
        }

        // ĳ���� ���� �ӵ��� �߷� �� ����
        yVelocity += gravity * Time.deltaTime;

        // yVelocity = yVelocity + gravity * Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

    }
}
