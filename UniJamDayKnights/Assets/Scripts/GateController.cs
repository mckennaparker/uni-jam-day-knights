using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("Condition")]
    [SerializeField] private GateCondition openCondition;

    [Header("Gate Parts")]
    [SerializeField] private GameObject gateVisual;
    [SerializeField] private Collider2D gateCollider;

    [Header("Behavior")]
    [SerializeField] private bool stayOpenOnceActivated;

    public bool IsOpen { get; private set; }

    private void Update()
    {
        if (openCondition == null)
        {
            return;
        }

        bool shouldOpen = openCondition.IsSatisfied;

        if (stayOpenOnceActivated && IsOpen)
        {
            return;
        }

        SetOpen(shouldOpen);
    }

    private void SetOpen(bool open)
    {
        if (IsOpen == open)
        {
            return;
        }

        IsOpen = open;

        if (gateVisual != null)
        {
            gateVisual.SetActive(!open);
        }

        if (gateCollider != null)
        {
            gateCollider.enabled = !open;
        }
    }
}