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

    private TileAnimation currentAnimation;
    public void SetFlag(int x, int y, int flag, bool updateUI = true) // - установка флага
    {
        X = x;
        Y = y;
        Flag = flag;

        if (updateUI)
        UpdateTile();
    }

    public void ChangeFlag()
    {
        Flag = 3;
        HasMerged = true;
    }

    public void ResetHasMerged()
    {
        HasMerged = false;
    }

    public void MergeWithTile(Tile otherTile)
    {
        TileAnimationController.Instance.SmoothTransition(this, otherTile, true);
        
        otherTile.ChangeFlag();
        SetFlag(X, Y, 0);
    }

    public void MoveToTile(Tile target)
    {
        TileAnimationController.Instance.SmoothTransition(this, target, false);

        target.SetFlag(target.X, target.Y, Flag, false);
        SetFlag(X, Y, 0);
    }

    public void UpdateTile() // - изменение цвета
    {
        image.color = ColorManager.Instance.TileColors[Flag];
    }

    public void SetAnimation(TileAnimation animation)
    {
        currentAnimation = animation;
    }

    public void CancelAnimation()
    {
        if (currentAnimation != null)
            currentAnimation.Destroy();
    }
}
