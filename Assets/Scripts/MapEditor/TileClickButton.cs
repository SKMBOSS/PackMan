using UnityEngine;
using UnityEngine.UI;

public class TileClickButton : MonoBehaviour
{
    [SerializeField] Image _selectedOutline;
    [SerializeField] TileType _tileType;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickTileButton);
    }

    public void ClickTileButton()
    {
        SelectedTileSaver.Instance.SetSelectedTileSprite(_tileType);
        _selectedOutline.enabled = true;
    }
}
