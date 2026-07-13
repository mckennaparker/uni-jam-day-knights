using System;
using UnityEngine;

public class SequenceButton : MonoBehaviour
{
    [SerializeField] private int sequenceID;

    public int SequenceID => sequenceID;

    public event Action<SequenceButton> OnButtonPressed;

    public void Press()
    {
        OnButtonPressed?.Invoke(this);
    }
}