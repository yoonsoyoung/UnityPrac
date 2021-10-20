using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    // Enemy 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    // Enemy 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 플레이어 트랜스폼
    Transform player;

    // 공격 가능 범위
    public float attackDistance = 2f;
    // 이동속도
    public float moveSpeed = 3f;
    // 캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    // 누적 시간
    float currentTime = 0; // 공격 딜레이 시간을 위해

    // 공격 딜레이 시간
    float attackDelay = 2f;

    // 초기 위치
    Vector3 originPos; // 좌표
    Quaternion originRot; // 회전

    // 이동 가능 범위
    public float moveDistance = 20;

    // 에너미의 체력
    public int hp = 15;

    // 에너미의 최대 체력
    int maxHp = 15;

    // 에너미 hp Slider 변수
    public Slider hpSlider;


    // 애니메이터 변수
    Animator anim;

    // 네비게이션 에이전트 변수
    NavMeshAgent smith;

    void Start()
    {
        m_State = EnemyState.Idle; // 최초의 Enemy 상태는 대기 상태로 변경
        player = GameObject.Find("Player").transform; // Player라는 태그값(Layer)의 트랜스폼을 가져와라

        cc = GetComponent<CharacterController>(); // 컴포넌트를 가져올 땐 GetComponent<>
        // 많이 가져오는 컴포넌트는 Transform, CharacterController, RigidBody 

        originPos = transform.position;
        originRot = transform.rotation;

        // 자식 오브젝트로부터 애니메이터 변수를 받아옴
        anim = transform.GetComponentInChildren<Animator>(); // Enemy의 자식으로 Zombie가 있기 때문에

        // 네비게이션 에이전트 컴포넌트 받아오기
        smith = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // 각 state에 따른 함수들을 실행
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
        // 현재 hp(%)를 hp 슬라이더의 value에 반영한다.
        hpSlider.value = (float)hp / (float)maxHp;
    }

    void Idle()
    {
        // 만약 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            // transform.position : 코드가 들어가 있는 오브젝트의 transform position
            // 플레이어의 position

            m_State = EnemyState.Move; ;
            print("상태 전환 : Idle -> Move");

            // 이동 애니메이션으로 전환
            anim.SetTrigger("IdleToMove"); // Animator Parameters에서 생성한 이름
        }
    }

    void Move()
    {
        // 만약 현재 위치가 초기 위치에서 이동 가능 버위를 벗어났다면
        if(Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태를 복귀(Return) 전환
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");
        }
        // 플레이어는 발견했으나 공격범위는 아닌
        if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized; // 차이값 정규화 => 방향

            // 캐릭터 컨트롤러를 이용해서 이동
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // 플레이어를 향해 방향 전환
            transform.forward = dir;

            // 네비게이션 에이전트 컴포넌트를 활성화한다.
            //smith.enabled = true;

            // 네비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
            smith.stoppingDistance = attackDistance;

            // 네비게이션 목적지를 플레이어의 위치로 설정한다.
            smith.destination = player.position;
        }

        // 공격 범위가 되면, 현재 상태를 공격으로 변환
        else
        {
            m_State = EnemyState.Attack;
            print("상태 변환 : Move > Attack");
            // 누적 시간을 공격 딜레이만큼 미리 실행
            currentTime = attackDelay;

            // 공격 대기 애니메이션 플레이
            anim.SetTrigger("MoveToAttackDelay");

            // 네비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
            smith.isStopped = true;
            smith.ResetPath();
        }
    }

    void Attack()
    {
        // 만약 플레이어가 공격 범위 이내에 있다면 플레이어를 공격
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 일정 시간마다 플레이어 공격
            currentTime += Time.deltaTime; // 현재 시간 누적
            if(currentTime > attackDelay)
            {
                if(currentTime > attackDelay)
                {
                    print("공격");
                    currentTime = 0;

                    // 공격 애니메이션 플레이
                    anim.SetTrigger("StartAttack");
                }
            }
        }
        // 그렇지 않다면 현재 상태를 Move로 전환 => 재추격
        else
        {

            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0;

            // 이동 애니메이션 플레이
            anim.SetTrigger("AttackToMove");
        }
    }

    void Return()
    {
        // 만약 초기위치에서의 거리가 0.1f 이상이면 초기 위치로 이동
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // 복귀 지점으로 방향 전환
            transform.forward = dir;

            // 네비게이션 목적지를 초기 저장된 위치로 설정한다.
            smith.destination = originPos;

            // 네비게이션으로 접근하는 최소 거리를 0으로 설정한다.
            smith.stoppingDistance = 0;
        }
        // 그렇지 않다면 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            m_State = EnemyState.Idle;
            print("상태 전환 : Return -> Idle");

            // hp를 다시 회복한다.
            hp = maxHp;

            // 대기 애니메이션으로 전환하는 트랜지션 호출
            anim.SetTrigger("MoveToIdle");
        }
    }
    // 데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
        // 만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 함수를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        // 플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        hp -= hitPower;

        // 네비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
        smith.isStopped = true;
        smith.ResetPath();

        // 에너미의 체력이 0보다 크면 피격 상태로 전환한다.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");

            // 피격 애니메이션을 플레이한다.
            anim.SetTrigger("Damaged");
            Damaged();
        }
        // 그렇지 않다면, 죽음 상태로 전환한다.
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");

            // 죽음 애니메이션을 플레이한다.
            anim.SetTrigger("Die");
            Die();
        }
    }

    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(1f);

        // 현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    // 죽음 상태 함수
    void Die()
    {
        // 진행중인 피격 코루틴을 중지한다.
        StopAllCoroutines();

        // 죽음 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 콘트롤러 컴포넌트를 비활성화한다.
        cc.enabled = false;

        // 2초 동안 기다린 뒤에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }
}
