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

    public override bool IsSatisfied
    {
        get
        {
            if (requirements == null || requirements.Length == 0)
            {
                return false;
            }

            foreach (RuneRequirement requirement in requirements)
            {
                if (requirement.runeBlock == null)
                {
                    return false;
                }

                if (requirement.runeBlock.CurrentValue
                    != requirement.requiredValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}