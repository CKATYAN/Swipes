using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static int Moves { get; private set; }

    public static bool GameStarted { get; private set; }

    [SerializeField] private TextMeshProUGUI gameResult;
    [SerializeField] private TextMeshProUGUI movesText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void Reset()
    {
        StartGame();
    }

    public void StartGame()
    {
        gameResult.text = "";

        SetMoves(0);
        GameStarted = true;

        Field.Instance.GenerateField();
    }

    public void Win()
    {
        GameStarted = false;
        gameResult.text = "You Win!";
    }

    public void Lose()
    {
        GameStarted = false;
        gameResult.text = "You Lose!";
    }

    public void AddMoves(int moves)
    {
        SetMoves(Moves + moves);
    }

    public void SetMoves(int moves)
    {
        Moves = moves;
        movesText.text = Moves.ToString();
    }
}
