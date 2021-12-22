using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int X { get; private set; } // - коорд по Х
    public int Y { get; private set; } // - коорд по Y

    public int Flag { get; private set; } // - флаг, показывающий какая это фигура

    public bool IsEmpty => Flag == 0; // - флаг

    [SerializeField] private Image image;
    [SerializeField] private RectTransform rt_tile;

    public void SetFlag(int x, int y, int flag) // - установка флага
    {
        X = x;
        Y = y;
        Flag = flag;

        UpdateTile();
    }

    public void UpdateTile() // - изменение цвета
    {
        image.color = ColorManager.Instance.TileColors[Flag];
    }
}
