using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int X { get; private set; } // - ����� �� �
    public int Y { get; private set; } // - ����� �� Y

    public int Flag { get; private set; } // - ����, ������������ ����� ��� ������

    public bool IsEmpty => Flag == 0; // - ����

    [SerializeField] private Image image;
    [SerializeField] private RectTransform rt_tile;

    public void SetFlag(int x, int y, int flag) // - ��������� �����
    {
        X = x;
        Y = y;
        Flag = flag;

        UpdateTile();
    }

    public void UpdateTile() // - ��������� �����
    {
        image.color = ColorManager.Instance.TileColors[Flag];
    }
}
