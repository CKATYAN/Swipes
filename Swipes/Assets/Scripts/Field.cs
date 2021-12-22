using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private int amountTile_X;
    private int amountTile_Y;

    [Header("Field Properties")]
    public float TileSize;
    public float Spacing;

    [Space(10)]
    [SerializeField]
    private Tile tilePref;
    [SerializeField]
    private RectTransform rt;
    [SerializeField]
    private RectTransform rt_tile;

    private Tile[,] field;

    private void Start()
    {
        GenerateFiled();
    }

    private void CreateFiled()
    {
        rt_tile.sizeDelta = new Vector2(TileSize, TileSize);

        amountTile_X = (int)((rt.sizeDelta.x - Spacing) / (TileSize + Spacing));
        amountTile_Y = (int)((rt.sizeDelta.y - Spacing) / (TileSize + Spacing));

        float sizex = rt.sizeDelta.x;
        float sizey = rt.sizeDelta.y;

        float startX = -(rt.sizeDelta.x / 2) + (TileSize / 2) + 2*Spacing;
        float startY = (rt.sizeDelta.y / 2) - (TileSize / 2) - 2*Spacing;

        field = new Tile[amountTile_X, amountTile_Y];

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++) {
                var tile = Instantiate(tilePref, transform, false);
                var position = new Vector2(startX + (x * (TileSize + Spacing)), startY - (y * (TileSize + Spacing)));
                tile.transform.localPosition = position;

                field[x, y] = tile;

                tile.SetValue(x, y, 0);
            }
    }

    public void GenerateFiled()
    {
        if (field == null)
            CreateFiled();

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                field[x, y].SetValue(x, y, 0);
    }
}
