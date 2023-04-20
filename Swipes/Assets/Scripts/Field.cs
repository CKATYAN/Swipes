using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance;
    
    private int amountTile_X;
    private int amountTile_Y;

    [Header("Field Properties")]
    public float TileSize; // - размер клетки
    public float Spacing; // - расстояние между клетками
    public int InitTilesCount_wall; // - начальное количество клеток (должно задаватся через файл)
    public int InitTilesCount_package;

    [Space(10)]
    [SerializeField] 
    private Tile tilePref;
    [SerializeField] 
    private RectTransform rt;
    [SerializeField] 
    private RectTransform rt_tile;

    private Tile[,] field;

    private bool anyTileMoved;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SwipeDetection.SwipeEvent += OnInput;
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GameController.GameStarted)
            GameController.Instance.Reset();
#endif
    }

    private void OnInput(Vector2 direction)
    {
        if (!GameController.GameStarted)
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
                for (int x = startX; x >= 0 && x < amountTile_X; x -= dir)
                {
                    var tile = field[x, y];

                    if (tile.IsEmpty)
                        continue;

                    var tileToMerge = FindTileToMerge(tile, direction);
                    if (tileToMerge != null && field[x, y])
                    {
                        tile.MergeWithTile(tileToMerge);
                        anyTileMoved = true;

                        continue;
                    }

                    var emptyTile = FindEmptyTile(tile, direction);
                    if (emptyTile != null && field[x, y].Flag == 1)
                    {
                        tile.MoveToTile(emptyTile);
                        anyTileMoved = true;
                    }
                }
        else if (direction.y != 0)
            for (int x = 0; x < amountTile_X; x++)
                for (int y = startY; y >= 0 && y < amountTile_Y; y -= dir)
                {
                    var tile = field[x, y];

                    if (tile.IsEmpty)
                        continue;

                    var tileToMerge = FindTileToMerge(tile, direction);
                    if (tileToMerge != null && field[x, y])
                    {
                        tile.MergeWithTile(tileToMerge);
                        anyTileMoved = true;

                        continue;
                    }

                    var emptyTile = FindEmptyTile(tile, direction);
                    if (emptyTile != null && field[x, y].Flag == 1)
                    {
                        tile.MoveToTile(emptyTile);
                        anyTileMoved = true;
                    }
                }

    }

    private void CheckGameResult()
    {
        int amount_tileFlag_1 = 0;

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                if (field[x, y].Flag == 1)
                {
                    amount_tileFlag_1++;
                }
        if (amount_tileFlag_1 == 0)
        {
            GameController.Instance.Win();
            return;
        }
        else if (amount_tileFlag_1 != 0 && amount_tileFlag_1 < InitTilesCount_package)
        {
            GameController.Instance.Lose();
            return;
        }
    }

    private void CreateField() // создаем поле
    {
        rt_tile.sizeDelta = new Vector2(TileSize, TileSize);

        amountTile_X = (int)((rt.sizeDelta.x - Spacing) / (TileSize + Spacing));
        amountTile_Y = (int)((rt.sizeDelta.y - Spacing) / (TileSize + Spacing));

        float startX = -(rt.sizeDelta.x / 2) + (TileSize / 2) + 2 * Spacing;
        float startY = (rt.sizeDelta.y / 2) - (TileSize / 2) - 2 * Spacing;

        field = new Tile[amountTile_X, amountTile_Y];

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
            {
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

        for (int i = 0; i < InitTilesCount_wall; i++)
            GenerateTile(2);
        for (int i = 0; i < InitTilesCount_package; i++)
            GenerateTile(1);
        for (int i = 0; i < InitTilesCount_package; i++)
            GenerateTile(3);
    }

    private void GenerateTile(int flag) // должно принимать координаты
    {
        var emptyTiles = new List<Tile>();

        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                if (field[x, y].IsEmpty)
                    emptyTiles.Add(field[x, y]);

        if (emptyTiles.Count == 0)
            throw new System.Exception("There is no any empty cell!");

        //Check(1,1); // здесь нужно подавать координаты
        //var tile = emptyTiles[1]; // здесь нужно превести координаты в индекс

        var tile = emptyTiles[Random.Range(0, emptyTiles.Count)]; // delete later

        tile.SetFlag(tile.X, tile.Y, flag, false);

        TileAnimationController.Instance.SmoothAppear(tile);
    }

    private void ResetTilesHasMoved()
    {
        for (int x = 0; x < amountTile_X; x++)
            for (int y = 0; y < amountTile_Y; y++)
                field[x, y].ResetHasMerged();
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

            if (Mathf.Abs(tile.X - x) == 1 || Mathf.Abs(tile.Y - y) == 1)
                if (field[x, y].Flag == 3 && tile.Flag == 1 && !field[x, y].HasMerged)
                    return field[x, y];
                
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
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty) {
                emptyTile = field[x, y];
                break;
            }
            else
                break;
        }

        return emptyTile;
    }
}

