using UnityEngine;

public class AllButtonsCondition : GateCondition
{
    [SerializeField] private PressureButton[] pressureButtons;

    public override bool IsSatisfied
    {
        get
        {
            if (pressureButtons == null || pressureButtons.Length == 0)
            {
                return false;
            }

            foreach (PressureButton button in pressureButtons)
            {
                if (button == null || !button.IsPressed)
                {
                    return false;
                }
            }

            return true;
        }
    }
}