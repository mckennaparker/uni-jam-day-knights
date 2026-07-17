using UnityEngine;

public class AllLeversCondition : GateCondition
{
    [SerializeField] private Lever[] levers;

    public override bool IsSatisfied
    {
        get
        {
            if (levers == null || levers.Length == 0)
            {
                return false;
            }

            foreach (Lever lever in levers)
            {
                if (lever == null)
                {
                    return false;
                }

                if (!lever.IsActivated)
                    return false;
            }
            return true;
        }
    }
}