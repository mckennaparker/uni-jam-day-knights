using UnityEngine;

public class SingleButtonCondition : GateCondition
{
    [SerializeField] private PressureButton pressureButton;

    public override bool IsSatisfied =>
        pressureButton != null && pressureButton.IsPressed;
}