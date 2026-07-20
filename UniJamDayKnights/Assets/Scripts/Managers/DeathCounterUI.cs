using TMPro;
using UnityEngine;

public class DeathCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Update()
    {
        if (DeathCounterManager.Instance == null)
        {
            text.enabled = false;
            return;
        }

        bool show = DeathCounterManager.Instance.ShowCounter;

        text.enabled = show;

        if (show)
            text.text = $"Deaths: {DeathCounterManager.Instance.DeathCount}";
    }
}