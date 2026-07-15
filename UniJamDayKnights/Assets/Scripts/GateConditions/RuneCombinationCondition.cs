using UnityEngine;

public class RuneCombinationCondition : GateCondition
{
    [System.Serializable]
    private class RuneRequirement
    {
        public RuneCycleBlock runeBlock;
        public int requiredValue;
    }

    [Header("Required Rune Combination")]
    [SerializeField] private RuneRequirement[] requirements;

    [Header("Progress Indicator")]
    [SerializeField] private PuzzleProgressIndicator progressIndicator;

    public override bool IsSatisfied
    {
        get
        {
            if (requirements == null || requirements.Length == 0)
            {
                UpdateIndicator(0);
                return false;
            }

            int correctCount = 0;

            foreach (RuneRequirement requirement in requirements)
            {
                if (requirement.runeBlock == null)
                {
                    continue;
                }

                if (requirement.runeBlock.CurrentValue ==
                    requirement.requiredValue)
                {
                    correctCount++;
                }
            }

            UpdateIndicator(correctCount);

            return correctCount == requirements.Length;
        }
    }

    private void UpdateIndicator(int correctCount)
    {
        if (progressIndicator != null)
        {
            progressIndicator.SetProgress(correctCount);
        }
    }
}