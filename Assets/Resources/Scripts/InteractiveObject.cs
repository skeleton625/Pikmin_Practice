using UnityEngine;
using DG.Tweening;

public class InteractiveObject : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int NeededPikminCount = 1;
    [SerializeField] private float InteractiveRadius = 1;
    private int currentPikminCount = 0;

    public void AssignPikmin()
    {
        ++currentPikminCount;

        /* 개수가 모두 채워질 경우, 초과할 경우에 대한 결과 구현 필요 */
        if (currentPikminCount.Equals(NeededPikminCount)) ;

        else if (currentPikminCount > NeededPikminCount) ;
    }

    public Vector3 GetPosition()
    {
        return Vector3.zero;
    }
}
