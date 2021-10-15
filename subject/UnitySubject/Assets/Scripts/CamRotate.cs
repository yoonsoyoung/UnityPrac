using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 이때 class 명과 유니티 스크립트 파일명과 일치해야 함!
    // 외부 오픈 소스 긁어올 때 주의

    public float rotSpeed = 200f;
    // 회전 값 변수
    float mx = 0;
    float my = 0;

    // Start is called before the first frame update
    // 첫 프레임 업데이트 시 호출
    void Start()
    {
    }

    // Update is called once per frame
    // 매 프레임마다 실시간 호출
    void Update()
    {
        // 사용자의 마우스 입력을 받아서 물체를 회전
        float mouse_X = Input.GetAxis("Mouse X"); // 축 값(GetAxis)을 받아오는데(Input) Mouse X 라는 값(Input Manager에 있음)
        float mouse_Y = Input.GetAxis("Mouse Y");

        // 마우스 입력 값을 이용해 회전 방향을 결정
        // Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0); // X좌표에는 -mouse_Y, y좌표에는 mouse_X
        // why? x축을 회전시키면 y가 오른쪽은 양수, 왼쪽은 음수로 변화 / 위로 하면 x 양수, 아래로 하면 x 음수
        // x축은 Y값 기준 변화, y축은 X값 기준 변화

        // 회전 방향으로 물체 회전
        //transform.eulerAngles += dir * roSpeed * Time.deltaTime; // Time.deltaTime 시간 프레임별

        // x축 회전 값을 -90도~90도 사이로 제한
        //Vector3 rot = transform.eulerAngles;
        //rot.x = Mathf.Clamp(rot.x, -90f, 90f);
        //transform.eulerAngles = rot;

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90f, 90f);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
