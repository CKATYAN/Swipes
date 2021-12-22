using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Flag { get; private set; }

    [SerializeField]
    private Image image;

    public void SetValue(int x, int y, int flag)
    {
        X = x;
        Y = y;
        Flag = flag;

        UpdateTile();
    }

    public void UpdateTile()
    {
        image.color = ColorManager.Instance.TileColors[Flag];
    }
}
