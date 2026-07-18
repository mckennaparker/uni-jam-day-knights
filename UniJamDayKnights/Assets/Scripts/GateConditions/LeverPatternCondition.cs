using UnityEngine;

public class LeverPatternCondition : GateCondition
{
    [System.Serializable]
    public class LeverRequirement
    {
        public Lever lever;
        public bool shouldBeOn;
    }

    [SerializeField] private LeverRequirement[] requirements;
    [SerializeField] private PuzzleProgressIndicator progressIndicator;

    public override bool IsSatisfied
    {
        get
        {
            if (requirements == null || requirements.Length == 0)
            {
                return false;
            }

            foreach (LeverRequirement requirement in requirements)
            {
                if (requirement.lever == null)
                {
                    return false;
                }

                if (requirement.lever.IsActivated != requirement.shouldBeOn)
                {
                    return false;
                }
            }

            return true;
        }
    }

    private void Start()
    {
        foreach (LeverRequirement requirement in requirements)
        {
            if (requirement.lever != null)
            {
                requirement.lever.OnStateChanged += HandleLeverStateChanged;
            }
        }

        UpdateProgress();
    }

    private void OnDestroy()
    {
        if (requirements == null)
        {
            return;
        }

        foreach (LeverRequirement requirement in requirements)
        {
            if (requirement.lever != null)
            {
                requirement.lever.OnStateChanged -= HandleLeverStateChanged;
            }
        }
    }

    private void HandleLeverStateChanged(
        Lever lever,
        bool isActivated
    )
    {
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        if (progressIndicator == null)
        {
            return;
        }

        int correctCount = 0;

        foreach (LeverRequirement requirement in requirements)
        {
            if (requirement.lever == null)
            {
                continue;
            }

            if (requirement.lever.IsActivated ==
                requirement.shouldBeOn)
            {
                correctCount++;
            }
        }

        progressIndicator.SetProgress(correctCount);
    }
}