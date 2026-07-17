using UnityEngine;

public class FadeBootstrap : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManagerPrefab;

    private void Awake()
    {
        if (FadeManager.Instance == null)
        {
            Instantiate(fadeManagerPrefab);
        }
    }
}
