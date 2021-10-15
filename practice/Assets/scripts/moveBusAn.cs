using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBusAn : MonoBehaviour
{
    GameObject bus; // ������Ʈ
    Transform target; // ��ġ ����

    public float speed = 50;

    public float start_point = 176;
    public float end_point = -164;
    public float stop_point = -16;

    private float timer = 0.0f;
    private float waiting = 3.0f;


    void Start()
    {
        bus = GameObject.Find("SchoolBus"); // �ش� �̸��� ������Ʈ�� ã�� ����
        target = bus.GetComponent<Transform>();

        target.position = new Vector3(176, -83, 135); // bus ������Ʈ�� �ʱ� ��ġ
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ��ǥ �̵�
        InvokeRepeating("Update_Moving", 3f, 8f); // �ش� �Լ��� 1�� �� ���� ��Ų �� 5�� �������� �ݺ� ����
        
        
    }

    private void Update_Moving()
    {
        if (target.position.x > -16)
        {
            target.position = Vector3.MoveTowards(target.position, new Vector3(stop_point, -83, 135), Time.deltaTime * speed);
        } else if(target.position.x <= -16)
        {
            timer += Time.deltaTime;
            if (timer > waiting)
                 target.position = Vector3.MoveTowards(target.position, new Vector3(end_point, -83, 135), Time.deltaTime * speed);
        }

        if(target.position.x == end_point)
        {
            Debug.Log("�����");
            Destroy(this);
            InvokeRepeating("Instan_bus", 2f, 2f);
        }
    }

    private void Instan_bus()
    {
        Debug.Log("��Ÿ����");
        Instantiate(bus);
        // Update_Moving();
    }
}
