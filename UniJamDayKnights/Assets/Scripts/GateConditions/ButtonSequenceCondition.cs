using UnityEngine;

public class ButtonSequenceCondition : GateCondition
{
    [SerializeField] private PressureButton[] correctSequence;

    private int currentIndex;
    private bool completed;

    public override bool IsSatisfied => completed;

    private void OnEnable()
    {
        foreach (PressureButton button in correctSequence)
        {
            if (button != null)
            {
                button.OnPressed += HandleButtonPressed;
            }
        }
    }

    private void OnDisable()
    {
        foreach (PressureButton button in correctSequence)
        {
            if (button != null)
            {
                button.OnPressed -= HandleButtonPressed;
            }
        }
    }

    private void HandleButtonPressed(PressureButton pressedButton)
    {
        if (completed || correctSequence.Length == 0)
        {
            return;
        }

        if (pressedButton == correctSequence[currentIndex])
        {
            currentIndex++;

            if (currentIndex >= correctSequence.Length)
            {
                completed = true;
            }
        }
        else
        {
            currentIndex = 0;
        }
    }
}