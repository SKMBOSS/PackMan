using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] int _slotNumber;
    [SerializeField] Text _fileName;
    private string _originFileName;
    private bool _isNewSlot = true;

    private Button _button;
    public event Action<string> OnClickFileSlotEvent;
    public event Action<int> OnClickNewSlotEvent;

    public int SlotNumber { get { return _slotNumber; } set { _slotNumber = value; } }
    public bool IsNewSlot { get { return _isNewSlot; } set { _isNewSlot = value; } }
    public string FileName { set { _fileName.text = value; } }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickSaveSlot);
    }

    public void HideSaveSlot()
    {
        gameObject.SetActive(false);
    }

    public void ShowSaveSlot()
    {
        gameObject.SetActive(true);
    }

    public void SetFileNameText(string fileName)
    {
        _originFileName = fileName;
        _fileName.text = _originFileName.Substring(2);
    }

    public void ClickSaveSlot()
    {
        if(_isNewSlot)
        {
            OnClickNewSlotEvent(_slotNumber);
        }
        else
        {
            OnClickFileSlotEvent(_originFileName);
        }
    }
}
