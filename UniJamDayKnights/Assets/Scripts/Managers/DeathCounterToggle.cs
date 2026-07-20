using UnityEngine;
using UnityEngine.UI;

public class DeathCounterToggle : MonoBehaviour
{
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void Start()
    {
        if (DeathCounterManager.Instance == null)
            return;

        toggle.SetIsOnWithoutNotify(
            DeathCounterManager.Instance.ShowCounter
        );
    }

    private void OnToggleChanged(bool value)
    {
        if (DeathCounterManager.Instance != null)
            DeathCounterManager.Instance.ShowCounter = value;
    }
}