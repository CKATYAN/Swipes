using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private int amountTile_X;
    private int amountTile_Y;

    [Header("Field Properties")]
    public float TileSize; // - размер клетки
    public float Spacing; // - расстояние между клетками
    public int InitTilesCount; // - начальное количество клеток (должно задаватся через файл)

    [Space(10)]
    [SerializeField] private Tile tilePref;
    [SerializeField] private RectTransform rt;
    [SerializeField] private RectTransform rt_tile;

    private Tile[,] field;

    private void Start()
    {
        GenerateFiled();
    }

    private void CreateFiled() // создаем поле
    {
        rt_tile.sizeDelta = new Vector2(TileSize, TileSize);

        amountTile_X = (int)((rt.sizeDelta.x - Spacing) / (TileSize + Spacing));
        amountTile_Y = (int)((rt.sizeDelta.y - Spacing) / (TileSize + Spacing));

        float startX = -(rt.sizeDelta.x / 2) + (TileSize / 2) + 2*Spacing;
        float startY = (rt.sizeDelta.y / 2) - (TileSize / 2) - 2*Spacing;

        field = new Tile[amountTile_X, amountTile_Y];

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++) {
                var tile = Instantiate(tilePref, transform, false);
                var position = new Vector2(startX + (x * (TileSize + Spacing)), startY - (y * (TileSize + Spacing)));
                tile.transform.localPosition = position;

                field[x, y] = tile;

                tile.SetFlag(x, y, 0);
            }
    }

    public void GenerateFiled() // должно принять файл с координатами
    {
        if (field == null)
            CreateFiled();

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                field[x, y].SetFlag(x, y, 0);

        for (int i = 0; i < InitTilesCount; i++)
            GenerateTile();
    }

    private void GenerateTile() // должно принимать координаты
    {
        var emptyTiles = new List<Tile>();

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                emptyTiles.Add(field[x, y]);

        if (emptyTiles.Count == 0)
            throw new System.Exception("There is no any empty cell!");

        //Check(1,1); // здесь нужно подавать координаты
        //var tile = emptyTiles[1]; // здесь нужно превести координаты в индекс

        var tile = emptyTiles[Random.Range(0, emptyTiles.Count)]; // delete later

        tile.SetFlag(tile.X, tile.Y, 1);
    }

    //private void Check(int x_now, int y_now) // проверка на близ стоящие фигуры
    //{
    //    int t = 0;

    //    for (int x = x_now - 1; x < x_now + 1; x++)
    //        for (int y = y_now - 1; y < y_now + 1; y++)
    //            if (field[x, y].IsEmpty && y != 0 && x != 0)
    //                t++; // здесь увиличиваем размер фигуры
    //                    // смотрим только на вверх вниз влево вправо
    //}
}
