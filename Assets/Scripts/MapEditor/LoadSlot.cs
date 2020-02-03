using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    [SerializeField] int _slotNumber;
    [SerializeField] Text _fileName;
    private string _originFileName;

    private Button _button;
    public event Action<string> OnClickEvent;

    public int SlotNumber { get { return _slotNumber; } set { _slotNumber = value; } }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickLoadSlot);
    }

    public void HideLoadSlot()
    {
        gameObject.SetActive(false);
    }

    public void ShowLoadSlot()
    {
        gameObject.SetActive(true);
    }

    public void SetFileNameText(string fileName)
    {
        _originFileName = fileName;
        _fileName.text = _originFileName.Substring(2);
    }

    public void ClickLoadSlot()
    {
        OnClickEvent(_originFileName);
    }
}
