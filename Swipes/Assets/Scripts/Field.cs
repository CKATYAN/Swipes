using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private int amountTile_X;
    private int amountTile_Y;

    public static Field Instance;

    [Header("Field Properties")]
    public float TileSize; // - размер клетки
    public float Spacing; // - расстояние между клетками
    public int InitTilesCount; // - начальное количество клеток (должно задаватся через файл)

    [Space(10)]
    [SerializeField] private Tile tilePref;
    [SerializeField] private RectTransform rt;
    [SerializeField] private RectTransform rt_tile;

    private Tile[,] field;

    private bool anyTileMoved;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        GenerateField();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector2.down);
#endif
    }

    private void OnInput(Vector2 direction)
    {
        if (GameController.GameStarted)
            return;

        anyTileMoved = false;
        ResetTilesHasMoved();

        Move(direction);
        if (anyTileMoved)
        {
            GameController.Instance.AddMoves(1);
            CheckGameResult();
        }
    }

    private void Move(Vector2 direction)
    {
        int startX = direction.x > 0 ? amountTile_X - 1 : 0;
        int startY = direction.y < 0 ? amountTile_Y - 1 : 0;
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        if (direction.x != 0)
            for (int y = 0; y < amountTile_Y; y++)
                for (int x = startX; x >= 0 && x < amountTile_X; x -=dir) {
                    var tile = field[x, y];

                    if (tile.IsEmpty)
                        continue;

                    var tileToMerge = FindTileToMerge(tile, direction);
                    if (tileToMerge != null) { 
                        tile.MergeWithTile(tileToMerge);
                        anyTileMoved = true;

                        continue;
                    }

                    var emptyTile = FindEmptyTile(tile, direction);
                    if (emptyTile != null && field[x, y].Flag != 2) {
                        tile.MoveToTile(emptyTile);
                        anyTileMoved = true;
                    }
                }
        else if (direction.y != 0)
            for (int x = 0; x < amountTile_X; x++)
                for (int y = startY; y >= 0 && y < amountTile_Y; y -= dir) {
                    var tile = field[x, y];

                    if (tile.IsEmpty)
                        continue;

                    var tileToMerge = FindTileToMerge(tile, direction);
                    if (tileToMerge != null) {
                        tile.MergeWithTile(tileToMerge);
                        anyTileMoved = true;

                        continue;
                    }

                    var emptyTile = FindEmptyTile(tile, direction);
                    if (emptyTile != null && field[x,y].Flag != 2) {
                        tile.MoveToTile(emptyTile);
                        anyTileMoved = true;
                    }
                }

    }

    private Tile FindTileToMerge(Tile tile, Vector2 direction)
    {
        int startX = tile.X + (int)direction.x;
        int startY = tile.Y - (int)direction.y;

        for (int x = startX, y = startY;
        x >= 0 && x < amountTile_X && y >= 0 && y < amountTile_Y;
        x += (int)direction.x, y -= (int)direction.y) {
            if (field[x, y].IsEmpty)
                continue;

            //if (field[x, y].Flag == tile.Flag && !field[x, y].HasMerged)
            //    return field[x, y];

            break;
        }

        return null;
    }

    private Tile FindEmptyTile(Tile tile, Vector2 direction)
    {
        Tile emptyTile = null;

        int startX = tile.X + (int)direction.x;
        int startY = tile.Y - (int)direction.y;

        for (int x = startX, y = startY;
        x >= 0 && x < amountTile_X && y >= 0 && y < amountTile_Y;
        x += (int)direction.x, y -= (int)direction.y) {
            if (field[x, y].IsEmpty) {
                emptyTile = field[x, y];
                break;
            }
            else
                break;
        }

        return emptyTile;
    }

    private void CheckGameResult()
    {

    }

    private void CreateField() // создаем поле
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

    public void GenerateField() // должно принять файл с координатами
    {
        if (field == null)
            CreateField();

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                field[x, y].SetFlag(x, y, 0);

        for (int i = 0; i < InitTilesCount; i++)
            GenerateTile(2);
        GenerateTile(1);
    }

    private void GenerateTile(int flag) // должно принимать координаты
    {
        var emptyTiles = new List<Tile>();

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                if (field[x,y].IsEmpty)
                    emptyTiles.Add(field[x, y]);

        if (emptyTiles.Count == 0)
            throw new System.Exception("There is no any empty cell!");

        //Check(1,1); // здесь нужно подавать координаты
        //var tile = emptyTiles[1]; // здесь нужно превести координаты в индекс

        var tile = emptyTiles[Random.Range(0, emptyTiles.Count)]; // delete later

        tile.SetFlag(tile.X, tile.Y, flag);
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

    private void ResetTilesHasMoved()
    {
        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                field[x, y].ResetHasMerged();
    }
}

