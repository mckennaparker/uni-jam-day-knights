using UnityEngine;

public abstract class GateCondition : MonoBehaviour
{
    public abstract bool IsSatisfied { get; }
}