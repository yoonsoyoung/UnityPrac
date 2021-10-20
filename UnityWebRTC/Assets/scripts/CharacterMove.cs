using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 7f;
    CharacterController cc; // 캐릭터의 물리 속성

    // 중력 변수
    float gravity = -30f;

    // 수직 속력
    float yVelocity = 0;

    // 점프력 변수
    public float jumpPower = 10f;

    // 점프 상태 변수 : bool ->> true / false
    public bool isJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // Input Manager에 Horizontal 값을 가져옴 
        float v = Input.GetAxis("Vertical");

        // 이동 방향 설정
        Vector3 dir = new Vector3(h, 0, v); // x좌표는 가로축, Vertical은 z축
        dir = dir.normalized; // normalized : 방향 벡터의 정규화(모든 방향의 벡터 길이를 1로 하여 방향에 따른 이동 속도를 같게 함)

        // 메인 카메라를 기준으로 방향을 변환
        dir = Camera.main.transform.TransformDirection(dir);

        transform.position += dir * moveSpeed * Time.deltaTime; // deltaTime은 현재 시간을 프레임 타임으로 정규화 해줌
        transform.position = transform.position + dir * moveSpeed * Time.deltaTime;
        // A = A + B;
        // A += B;

        // 만일 점프 중이었고, 다시 바닥에 착지했다면
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }

        // 만일 키보드 스페이스 바를 입력했고, 점프를 하지 않은 상태라면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력을 적용하고 점프 상태로 변경
            yVelocity = jumpPower;
            isJumping = true;
        }

        // 캐릭터 수직 속도에 중력 값 적용
        yVelocity += gravity * Time.deltaTime;

        // yVelocity = yVelocity + gravity * Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

    }
}
