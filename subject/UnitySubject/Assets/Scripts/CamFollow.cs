using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라의 위치를 목표 트랜스폼의 위치에 일치 시킴
        transform.position = target.position;
    }
}
