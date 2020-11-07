using UnityEngine;

public class PikminController : MonoBehaviour
{
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private Transform Target = null;
    [SerializeField] private Transform Follower = null;
    [SerializeField] private Transform VisualCylinder = null;
    [SerializeField] private LineRenderer TargetLine = null;

    private int colliderLayer = 0;
    private Camera mainCamera = default;
    private readonly int linePointsCount = 5;

    private void Awake()
    {
        /* Layer Collider 제외 숫자 */
        colliderLayer = -257;
        mainCamera = FindObjectOfType<Camera>();
        TargetLine.positionCount = linePointsCount;
    }

    public void UpdateMousePosition()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, colliderLayer))
        {
            Target.position = hit.point + targetOffset;
            Target.up = Vector3.Lerp(Target.up, hit.normal, .3f);
            VisualCylinder.position = Target.position;

            for (int i = 0; i < linePointsCount; i++)
            {
                Vector3 linePos = Vector3.Lerp(Follower.position, Target.position, i * linePointsCount);
                TargetLine.SetPosition(i, linePos);
            }
        }
    }
}
