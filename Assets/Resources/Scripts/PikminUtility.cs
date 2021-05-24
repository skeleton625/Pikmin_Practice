using UnityEngine.AI;
using UnityEngine;

/* 이동이 완료되었는지 확인하기위한 NavMeshAgent 전역 함수용 클래스 */
public static class PikminUtility
{
    public static bool IsDone(this NavMeshAgent agent)
    {
        return (!agent.pathPending &&
                 agent.remainingDistance <= agent.stoppingDistance);
    }

    public static bool IsArrived(this NavMeshAgent agent, Vector3 start, Vector3 dest)
    {
        start.y = dest.y = 0f;
        return Vector3.Distance(start, dest) <= agent.stoppingDistance;
    }
}
