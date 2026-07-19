using UnityEngine;

public class HiddenObjectGlyphs : MonoBehaviour
{
    [Header("Glyph Objects")]
    [SerializeField] private GameObject[] glyphObjects;

    private bool[] savedActiveStates;
    private bool isHidden;

    private void Awake()
    {
        savedActiveStates = new bool[glyphObjects.Length];
    }

    public void HideGlyphs()
    {
        if (isHidden)
        {
            return;
        }

        isHidden = true;

        for (int i = 0; i < glyphObjects.Length; i++)
        {
            GameObject glyph = glyphObjects[i];

            if (glyph == null)
            {
                continue;
            }

            savedActiveStates[i] = glyph.activeSelf;
            glyph.SetActive(false);
        }
    }

    public void RestoreGlyphs()
    {
        if (!isHidden)
        {
            return;
        }

        isHidden = false;

        for (int i = 0; i < glyphObjects.Length; i++)
        {
            GameObject glyph = glyphObjects[i];

            if (glyph == null)
            {
                continue;
            }

            glyph.SetActive(savedActiveStates[i]);
        }
    }
}