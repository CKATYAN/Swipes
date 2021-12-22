using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color[] CellColors;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
