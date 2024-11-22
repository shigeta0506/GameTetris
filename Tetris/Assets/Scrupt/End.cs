using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public void OnEndButtonPressed()
    {
        Debug.Log("ÉQÅ[ÉÄÇèIóπÇµÇ‹Ç∑");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
