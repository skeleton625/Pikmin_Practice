using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InteractiveObject : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected GameObject fractionPrefab = null;
    [SerializeField] protected Vector3 uiOffset = Vector3.zero;

    [Header("Variables")]
    [SerializeField] protected int neededPikminCount = 1;
    [SerializeField] protected float interactRadius = 1;

    protected GameObject fractionObject = null;
    private TextMeshProUGUI denominator = null;
    private TextMeshProUGUI numerator = null;
    private Pikmin[] pikminArray = null;
    private int pikminCount = 0;

    private void Awake()
    {
        Initialize();

    }

    public virtual void Initialize()
    {
        /* InteractiveObject의 UI prefab 추가 */
        GameObject canvas = GameObject.Find("Canvas");
        if(canvas != null)
        {
            fractionObject = Instantiate(fractionPrefab, canvas.transform);
            numerator = fractionObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            denominator = fractionObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            fractionObject.SetActive(false);
        }

        pikminArray = new Pikmin[100];
    }

    /* 상호작용 오브젝트에 Pikmin 할당 함수 */
    public void AssignPikmin()
    {
        ++pikminCount;
        /* 상호작용 오브젝트에 Pikmin 추가 시, UI 변화 구현 */
        fractionObject.SetActive(true);
        fractionObject.transform.GetChild(0).DOComplete();
        fractionObject.transform.GetChild(0).DOPunchScale(Vector3.one, .3f);
        numerator.text = pikminCount.ToString();
        denominator.text = neededPikminCount.ToString();
        /* 필요한 Pikmin 수 만족 시 */
        if (pikminCount.Equals(neededPikminCount)) Interact();
        /* 필요한 Pikmin 수 초과 시 */
        else if (pikminCount > neededPikminCount) StopInteract();
    }

    /* 상호작용 오브젝트에 Pikmin 반환 함수 */
    public void ReleasePikmin()
    {
        /* 할당된 Pikmin이 없을 경우 */
        pikminCount = Mathf.Clamp(pikminCount - 1, 0, 100);

        /* Num 번 Pikmin 제거, 그에 따른 InteractiveObject UI 수정 */
        if (pikminCount.Equals(0))
            fractionObject.SetActive(false);
        else
        {
            fractionObject.transform.GetChild(0).DOComplete();
            fractionObject.transform.GetChild(0).DOPunchScale(Vector3.one, .3f);
            numerator.text = pikminCount.ToString();
            denominator.text = neededPikminCount.ToString();
        }

        /* 할당된 Pikmin 개수가 필요한 개수보다 적을 경우, 상호작용 종료 */
        if (pikminCount < neededPikminCount) StopInteract();
    }

    public Vector3 GetPosition()
    {
        float angle = pikminCount * Mathf.PI * 2f / neededPikminCount;
        return transform.position + Vector3.right * Mathf.Cos(angle) * interactRadius
                                  + Vector3.forward * Mathf.Sin(angle) * interactRadius;
    }

    public virtual void Interact() { }
    public virtual void StopInteract() { }
    public virtual void UpdateSpeed() { }
}
