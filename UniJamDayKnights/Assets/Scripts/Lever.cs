using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool IsActivated { get; private set; }

    public void Toggle()
    {
        IsActivated = !IsActivated;
    }

    public void SetActivated(bool activated)
    {
        IsActivated = activated;
    }
}