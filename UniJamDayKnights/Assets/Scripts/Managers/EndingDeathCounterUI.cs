using TMPro;
using UnityEngine;

public class EndingDeathCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text deathCountText;

    private void Start()
    {
        if (deathCountText == null)
        {
            Debug.LogError("EndingDeathCounterUI: Death Count Text is not assigned.");
            return;
        }

        if (DeathCounterManager.Instance == null ||
            !DeathCounterManager.Instance.ShowCounter)
        {
            deathCountText.gameObject.SetActive(false);
            return;
        }

        deathCountText.gameObject.SetActive(true);
        deathCountText.text =
            $"Total Deaths: {DeathCounterManager.Instance.DeathCount}";
    }
}