using TMPro;
using UnityEngine;

public class DeathCounterManager : MonoBehaviour
{
    public static DeathCounterManager Instance { get; private set; }

    public int DeathCount { get; private set; }

    public bool ShowCounter { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddDeath()
    {
        DeathCount++;
    }

    public void ResetCounter()
    {
        DeathCount = 0;
    }
}