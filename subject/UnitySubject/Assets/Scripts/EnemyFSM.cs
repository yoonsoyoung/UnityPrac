using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    // Enemy ���� ���
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
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

    // ���ʹ��� ü��
    public int hp = 15;

    // ���ʹ��� �ִ� ü��
    int maxHp = 15;

    // ���ʹ� hp Slider ����
    public Slider hpSlider;


    // �ִϸ����� ����
    Animator anim;

    // �׺���̼� ������Ʈ ����
    NavMeshAgent smith;

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

        // �׺���̼� ������Ʈ ������Ʈ �޾ƿ���
        smith = GetComponent<NavMeshAgent>();
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
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }
        // ���� hp(%)�� hp �����̴��� value�� �ݿ��Ѵ�.
        hpSlider.value = (float)hp / (float)maxHp;
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

            // �׺���̼� ������Ʈ ������Ʈ�� Ȱ��ȭ�Ѵ�.
            //smith.enabled = true;

            // �׺���̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
            smith.stoppingDistance = attackDistance;

            // �׺���̼� �������� �÷��̾��� ��ġ�� �����Ѵ�.
            smith.destination = player.position;
        }

        // ���� ������ �Ǹ�, ���� ���¸� �������� ��ȯ
        else
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ : Move > Attack");
            // ���� �ð��� ���� �����̸�ŭ �̸� ����
            currentTime = attackDelay;

            // ���� ��� �ִϸ��̼� �÷���
            anim.SetTrigger("MoveToAttackDelay");

            // �׺���̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
            smith.isStopped = true;
            smith.ResetPath();
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

                    // ���� �ִϸ��̼� �÷���
                    anim.SetTrigger("StartAttack");
                }
            }
        }
        // �׷��� �ʴٸ� ���� ���¸� Move�� ��ȯ => ���߰�
        else
        {

            m_State = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            currentTime = 0;

            // �̵� �ִϸ��̼� �÷���
            anim.SetTrigger("AttackToMove");
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

            // �׺���̼� �������� �ʱ� ����� ��ġ�� �����Ѵ�.
            smith.destination = originPos;

            // �׺���̼����� �����ϴ� �ּ� �Ÿ��� 0���� �����Ѵ�.
            smith.stoppingDistance = 0;
        }
        // �׷��� �ʴٸ� �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ���� ��ȯ
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            m_State = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");

            // hp�� �ٽ� ȸ���Ѵ�.
            hp = maxHp;

            // ��� �ִϸ��̼����� ��ȯ�ϴ� Ʈ������ ȣ��
            anim.SetTrigger("MoveToIdle");
        }
    }
    // ������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
        // ����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶�� �ƹ��� ó���� ���� �ʰ� �Լ��� �����Ѵ�.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        hp -= hitPower;

        // �׺���̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
        smith.isStopped = true;
        smith.ResetPath();

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ: Any state -> Damaged");

            // �ǰ� �ִϸ��̼��� �÷����Ѵ�.
            anim.SetTrigger("Damaged");
            Damaged();
        }
        // �׷��� �ʴٸ�, ���� ���·� ��ȯ�Ѵ�.
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any state -> Die");

            // ���� �ִϸ��̼��� �÷����Ѵ�.
            anim.SetTrigger("Die");
            Die();
        }
    }

    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DamageProcess());
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(1f);

        // ���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
    }

    // ���� ���� �Լ�
    void Die()
    {
        // �������� �ǰ� �ڷ�ƾ�� �����Ѵ�.
        StopAllCoroutines();

        // ���� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        cc.enabled = false;

        // 2�� ���� ��ٸ� �ڿ� �ڱ� �ڽ��� �����Ѵ�.
        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }
}
