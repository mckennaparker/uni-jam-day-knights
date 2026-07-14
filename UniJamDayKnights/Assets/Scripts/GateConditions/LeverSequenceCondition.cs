using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSequenceCondition : GateCondition
{
    [Header("Correct Lever Order")]
    [SerializeField] private Lever[] correctSequence;

    [Header("Reset Behavior")]
    [SerializeField] private float resetDelay = 0.5f;

    private readonly List<Lever> enteredSequence = new();

    private bool completed;
    private bool isResetting;

    public override bool IsSatisfied => completed;

    private void Start()
    {
        if (correctSequence == null || correctSequence.Length == 0)
        {
            Debug.LogError("No lever sequence has been assigned.", this);
            return;
        }

        foreach (Lever lever in correctSequence)
        {
            if (lever != null)
            {
                lever.OnStateChanged += HandleLeverStateChanged;
            }
        }

        // start as all levers off
        ResetAllLeversImmediately();
    }

    private void OnDestroy()
    {
        if (correctSequence == null)
        {
            return;
        }

        foreach (Lever lever in correctSequence)
        {
            if (lever != null)
            {
                lever.OnStateChanged -= HandleLeverStateChanged;
            }
        }
    }

    private void HandleLeverStateChanged(
        Lever changedLever,
        bool isActivated
    )
    {
        if (completed || isResetting)
        {
            return;
        }

        // reset if lever turnned off while on
        if (!isActivated)
        {
            if (enteredSequence.Contains(changedLever))
            {
                Debug.Log("A lever was turned off during the sequence.");
                StartCoroutine(ResetAfterDelay());
            }

            return;
        }

        // same lever activated again
        if (enteredSequence.Contains(changedLever))
        {
            Debug.Log("The same lever was activated more than once.");
            StartCoroutine(ResetAfterDelay());
            return;
        }

        enteredSequence.Add(changedLever);

        Debug.Log(
            $"Lever input {enteredSequence.Count}/" +
            $"{correctSequence.Length}: {changedLever.name}"
        );

        // # of current on levers don't match the answer
        if (CountActivatedLevers() != enteredSequence.Count)
        {
            Debug.Log("Lever states do not match the recorded sequence.");
            StartCoroutine(ResetAfterDelay());
            return;
        }

        // wait for all levers on
        if (enteredSequence.Count < correctSequence.Length)
        {
            return;
        }

        CheckCompletedSequence();
    }

    private void CheckCompletedSequence()
    {
        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (enteredSequence[i] != correctSequence[i])
            {
                Debug.Log("Wrong lever sequence.");
                StartCoroutine(ResetAfterDelay());
                return;
            }
        }

        // Check for all levers ON
        if (CountActivatedLevers() != correctSequence.Length)
        {
            Debug.Log("Not all levers are activated.");
            StartCoroutine(ResetAfterDelay());
            return;
        }

        completed = true;
        Debug.Log("Lever sequence completed.");
    }

    private int CountActivatedLevers()
    {
        int count = 0;

        foreach (Lever lever in correctSequence)
        {
            if (lever != null && lever.IsActivated)
            {
                count++;
            }
        }

        return count;
    }

    private IEnumerator ResetAfterDelay()
    {
        if (isResetting)
        {
            yield break;
        }

        isResetting = true;

        yield return new WaitForSeconds(resetDelay);

        ResetAllLeversImmediately();

        isResetting = false;
    }

    private void ResetAllLeversImmediately()
    {
        enteredSequence.Clear();

        foreach (Lever lever in correctSequence)
        {
            if (lever != null)
            {
                lever.SetActivated(false);
            }
        }
    }
}