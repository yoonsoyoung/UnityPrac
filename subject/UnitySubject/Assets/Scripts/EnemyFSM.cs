using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // Enemy ���� ���
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return
    }

    // Enemy ���� ����
    EnemyState m_State;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    // �÷��̾� Ʈ������
    Transform player;

    // ���� ���� ����
    public float attackDistance = 2f;
    // �̵��ӵ�
    public float moveSpeed = 3f;
    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // ���� �ð�
    float currentTime = 0; // ���� ������ �ð��� ����

    // ���� ������ �ð�
    float attackDelay = 2f;

    // �ʱ� ��ġ
    Vector3 originPos; // ��ǥ
    Quaternion originRot; // ȸ��

    // �̵� ���� ����
    public float moveDistance = 20;

    // �ִϸ����� ����
    Animator anim;

    void Start()
    {
        m_State = EnemyState.Idle; // ������ Enemy ���´� ��� ���·� ����
        player = GameObject.Find("Player").transform; // Player��� �±װ�(Layer)�� Ʈ�������� �����Ͷ�

        cc = GetComponent<CharacterController>(); // ������Ʈ�� ������ �� GetComponent<>
        // ���� �������� ������Ʈ�� Transform, CharacterController, RigidBody 

        originPos = transform.position;
        originRot = transform.rotation;

        // �ڽ� ������Ʈ�κ��� �ִϸ����� ������ �޾ƿ�
        anim = transform.GetComponentInChildren<Animator>(); // Enemy�� �ڽ����� Zombie�� �ֱ� ������
    }

    // Update is called once per frame
    void Update()
    {
        // �� state�� ���� �Լ����� ����
        switch(m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
        }
    }

    void Idle()
    {
        // ���� �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move ���·� ��ȯ
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            // transform.position : �ڵ尡 �� �ִ� ������Ʈ�� transform position
            // �÷��̾��� position

            m_State = EnemyState.Move; ;
            print("���� ��ȯ : Idle -> Move");

            // �̵� �ִϸ��̼����� ��ȯ
            anim.SetTrigger("IdleToMove"); // Animator Parameters���� ������ �̸�
        }
    }

    void Move()
    {
        // ���� ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ ����ٸ�
        if(Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // ���� ���¸� ����(Return) ��ȯ
            m_State = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");
        }
        // �÷��̾�� �߰������� ���ݹ����� �ƴ�
        if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // �̵� ���� ����
            Vector3 dir = (player.position - transform.position).normalized; // ���̰� ����ȭ => ����

            // ĳ���� ��Ʈ�ѷ��� �̿��ؼ� �̵�
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // �÷��̾ ���� ���� ��ȯ
            transform.forward = dir;
        }

        // ���� ������ �Ǹ�, ���� ���¸� �������� ��ȯ
        else
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ : Move > Attack");
            // ���� �ð��� ���� �����̸�ŭ �̸� ����
            currentTime = attackDelay;
        }
    }

    void Attack()
    {
        // ���� �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ ����
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // ���� �ð����� �÷��̾� ����
            currentTime += Time.deltaTime; // ���� �ð� ����
            if(currentTime > attackDelay)
            {
                if(currentTime > attackDelay)
                {
                    print("����");
                    currentTime = 0;
                }
            }
        }
        // �׷��� �ʴٸ� ���� ���¸� Move�� ��ȯ => ���߰�
        else
        {

            m_State = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            currentTime = 0;
        }
    }

    void Return()
    {
        // ���� �ʱ���ġ������ �Ÿ��� 0.1f �̻��̸� �ʱ� ��ġ�� �̵�
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // ���� �������� ���� ��ȯ
            transform.forward = dir;
        }
        // �׷��� �ʴٸ� �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ���� ��ȯ
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            m_State = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");

            // ��� �ִϸ��̼����� ��ȯ�ϴ� Ʈ������ ȣ��
            anim.SetTrigger("MoveToIdle");
        }
    }
}
