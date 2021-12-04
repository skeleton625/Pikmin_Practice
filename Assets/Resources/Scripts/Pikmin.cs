using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Pikmin : MonoBehaviour
{
    public enum State { Idle, Follow, Interact };

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public State PikminState { get => state; }
    [HideInInspector] public int PikminID = -1;
    private State state = default;
    private Transform target = null;
    private Coroutine updateTarget = null;
    private PikminManager pManager = null;

    private InteractiveObject objective = null;
    private WaitForSeconds wait = new WaitForSeconds(.25f);

    private void Awake()
    {
        target = GameObject.Find("Follow_Position").transform;
        agent = GetComponent<NavMeshAgent>();
        pManager = PikminManager.instance;
    }

    /* SetTarget 함수 대용 */
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ControlObject"))
        {
            if (state.Equals(State.Follow))
                return;
            else if (state.Equals(State.Interact))
            {
                objective.ReleasePikmin();
                transform.SetParent(null);
                objective = null;
            }
            state = State.Follow;
            pManager.GetPikmin(this);

            if (updateTarget != null)
                StopCoroutine(updateTarget);

            updateTarget = StartCoroutine(UpdateTarget());

            IEnumerator UpdateTarget()
            {
                agent.enabled = true;
                while (true)
                {
                    if (agent.enabled)
                        agent.SetDestination(target.position);
                    yield return wait;
                }
            }
        }
    }

    private void CheckInteraction()
    {
        /* 반지름이 1인 Raycast 구체로 충돌체 확인, 없을 시 종료 */
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        if (colliders.Length.Equals(0))
            return;

        bool flag = true;
        /* pikmin 주위의 한 IntractiveObject만 상호작용 */
        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("InteractiveObject"))
            {
                objective = collider.GetComponent<InteractiveObject>();
                objective.AssignPikmin();
                StartCoroutine(MovePikmin(objective.GetPosition()));
                flag = false;
                break;
            }
        }

        if (flag)
            state = State.Idle;

        IEnumerator MovePikmin(Vector3 position)
        {
            var navMeshPath = new NavMeshPath();
            agent.CalculatePath(position, navMeshPath);
            agent.SetPath(navMeshPath);

            while(true)
            {
                if(agent.IsDone())
                {
                    agent.enabled = false;
                    state = State.Interact;
                    transform.SetParent(objective.transform);
                    break;
                }
                yield return null;
            }
        }
    }


    public void Throw(Vector3 target, float time, float delay)
    {
        /* 비행 소리 필요 */
        if(updateTarget != null)
            StopCoroutine(updateTarget);
        agent.enabled = false;

        transform.DOJump(target, 2, 1, time).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
        {
            agent.enabled = true;
            CheckInteraction();

            /* 착지 소리 필요 */
        });

        Vector3 pos = target;
        pos.y = transform.position.y;
        transform.LookAt(pos);
        
        /* PikminVisualHandler */
    }
}
