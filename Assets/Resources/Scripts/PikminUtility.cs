using UnityEngine.AI;

/* 이동이 완료되었는지 확인하기위한 NavMeshAgent 전역 함수용 클래스 */
public static class PikminUtility
{
    public static bool IsDone(this NavMeshAgent agent)
    {
        return (!agent.pathPending &&
                 agent.remainingDistance <= agent.stoppingDistance &&
                (agent.hasPath || agent.velocity.sqrMagnitude.Equals(0))
               );
    }
}
