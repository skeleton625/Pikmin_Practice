using UnityEngine;
using DG.Tweening;
using TMPro;

public class InteractiveObject : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject fractionPrefab = null;
    [SerializeField] private Vector3 uiOffset = Vector3.zero;
    [SerializeField] private Transform canvas = null;

    [Header("Variables")]
    [SerializeField] private int neededPikminCount = 1;
    [SerializeField] private float interactRadius = 1;

    private int currentPikminCount = 0;
    private GameObject fractionObject = null;
    private TextMeshProUGUI numerator = null;
    private TextMeshProUGUI denominator = null;

    private void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        fractionObject = Instantiate(fractionPrefab);
        numerator = fractionObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        denominator = fractionObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        fractionObject.SetActive(false);
    }

    /* 상호작용 오브젝트에 Pikmin 할당 함수 */
    public void AssignPikmin()
    {
        ++currentPikminCount;

        /* 상호작용 오브젝트에 Pikmin 추가 시, UI 변화 구현 */
        fractionObject.SetActive(true);
        fractionObject.transform.GetChild(0).DOComplete();
        fractionObject.transform.GetChild(0).DOPunchScale(Vector3.one, .3f);
        numerator.text = currentPikminCount.ToString();
        denominator.text = neededPikminCount.ToString();
        /* 필요한 Pikmin 수 만족 시 */
        if (currentPikminCount.Equals(neededPikminCount)) Interact();
        /* 필요한 Pikmin 수 초과 시 */
        else if (currentPikminCount > neededPikminCount) StopInteract();
    }

    /* 상호작용 오브젝트에 Pikmin 반환 함수 */
    public void ReleasePikmin()
    {
        if (currentPikminCount.Equals(0)) return;
        --currentPikminCount;

        if (currentPikminCount.Equals(0))
            fractionObject.SetActive(false);
        else
        {
            numerator.text = currentPikminCount.ToString();
            denominator.text = neededPikminCount.ToString();
        }

        if (currentPikminCount < neededPikminCount) StopInteract();
    }

    public Vector3 GetPosition()
    {
        float angle = currentPikminCount * Mathf.PI * 2f / neededPikminCount;
        return transform.position + Vector3.right * Mathf.Cos(angle) * interactRadius
                                  + Vector3.forward * Mathf.Sin(angle) * interactRadius;
    }

    public virtual void Interact() { }
    public virtual void StopInteract() { }
    public virtual void UpdateSpeed() { }
}
