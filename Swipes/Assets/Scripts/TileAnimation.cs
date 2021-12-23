using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TileAnimation : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private float moveTime = .1f;

    private Sequence sequence;
    public void Move(Tile from, Tile to, bool isMerging)
    {
        from.CancelAnimation();
        to.SetAnimation(this);

        image.color = ColorManager.Instance.TileColors[from.Flag];


        transform.position = from.transform.position;

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad));

        if (isMerging)
        {
            sequence.AppendCallback(() =>
            {
                image.color = ColorManager.Instance.TileColors[to.Flag];
            });

            sequence.Append(transform.DOScale(1.2f, .1f));
            sequence.Append(transform.DOScale(1, .1f));
        }

        sequence.AppendCallback(() =>
        {
            to.UpdateTile();
            Destroy();
        });
    }

    public void Appear(Tile tile)
    {
        tile.CancelAnimation();
        tile.SetAnimation(this);

        image.color = ColorManager.Instance.TileColors[tile.Flag];

        transform.position = tile.transform.position;
        transform.localScale = Vector2.one;

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(1.2f, .1f * 2));
        sequence.Append(transform.DOScale(1f, .1f * 2));

        sequence.AppendCallback(() =>
        {
            tile.UpdateTile();
            Destroy();
        });
    }
    public void Destroy()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}
