using UnityEngine;

public class PuzzleProgressIndicator : MonoBehaviour
{
    [SerializeField] private IndicatorLight[] lights;

    private int currentProgress = -1;

    public void SetProgress(int completedCount)
    {
        completedCount = Mathf.Clamp(
            completedCount,
            0,
            lights.Length
        );

        if (completedCount == currentProgress)
        {
            return;
        }

        currentProgress = completedCount;

        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] == null)
            {
                continue;
            }

            lights[i].SetOn(i < completedCount);
        }
    }
}