using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditTile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] TileType _tileType;
    [SerializeField] bool _isTouchAble = true;
    private Image _image;

    public TileType TileType { get { return _tileType; } set { _tileType = value; } }
    public bool IsTouchAble { get { return _isTouchAble; } }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SelectedTileSaver.Instance.CurrentTileSprite != null && _isTouchAble)
        {
            if (SelectedTileSaver.Instance.IsClicked)
            {
                SetTileInfoForSave();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SelectedTileSaver.Instance.CurrentTileSprite != null && _isTouchAble)
        {
            SelectedTileSaver.Instance.IsClicked = true;
            SetTileInfoForSave();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (SelectedTileSaver.Instance.CurrentTileSprite != null && _isTouchAble)
        {
            SelectedTileSaver.Instance.IsClicked = false;
        }
    }

    private void SetTileInfoForSave()
    {
        _image.sprite = SelectedTileSaver.Instance.CurrentTileSprite;
        _tileType = SelectedTileSaver.Instance.TileType;
    }

    public void SetTileInfoForLoad(TileType tileType)
    {
        _image.sprite = SelectedTileSaver.Instance.GetTileSpriteByTileType(tileType);
        _tileType = tileType;
    }
}
