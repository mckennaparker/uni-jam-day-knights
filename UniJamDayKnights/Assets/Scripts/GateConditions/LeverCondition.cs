using UnityEngine;

public class LeverCondition : GateCondition
{
    [SerializeField] private Lever lever;

    public override bool IsSatisfied =>
        lever != null && lever.IsActivated;
}