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

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
        Flag = Flag;

        UpdateTile();
    }

    public void UpdateTile()
    {

    }
}
