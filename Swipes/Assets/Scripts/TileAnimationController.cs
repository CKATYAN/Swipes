using UnityEngine;
using DG.Tweening;

public class TileAnimationController : MonoBehaviour
{
    public static TileAnimationController Instance { get; private set; }

    [SerializeField]
    private TileAnimation animationPref;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DOTween.Init();
    }

    public void SmoothTransition(Tile from, Tile to, bool isMerging)
    {
        Instantiate(animationPref, transform, false).Move(from, to, isMerging);
    }

    public void SmoothAppear(Tile tile)
    {
        Instantiate(animationPref, transform, false).Appear(tile);
    }
}
