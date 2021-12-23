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
    public bool HasMerged { get; private set; }

    [SerializeField] 
    private Image image;
    [SerializeField] 
    private RectTransform rt_tile;

    public void SetFlag(int x, int y, int flag) // - установка флага
    {
        X = x;
        Y = y;
        Flag = flag;

        UpdateTile();
    }

    public void ChangeFlag()
    {
        Flag = 3;
        HasMerged = true;
        UpdateTile();
    }

    public void ResetHasMerged()
    {
        HasMerged = false;
    }

    public void MergeWithTile(Tile otherTile)
    {
        otherTile.ChangeFlag();
        SetFlag(X, Y, 0);

        UpdateTile();
    }

    public void MoveToTile(Tile target)
    {
        target.SetFlag(target.X, target.Y, Flag);
        SetFlag(X, Y, 0);

        UpdateTile();
    }

    public void UpdateTile() // - изменение цвета
    {
        image.color = ColorManager.Instance.TileColors[Flag];
    }
}
