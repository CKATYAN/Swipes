using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color[] TileColors;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
