using System;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSlot : MonoBehaviour
{
    [SerializeField] int _slotNumber;
    [SerializeField] Text _fileName;
    private string _originFileName;

    private Button _button;
    public event Action<string> OnClickEvent;

    public int SlotNumber { get { return _slotNumber; } set { _slotNumber = value; } }
    public string NewFileName { get { return (_slotNumber.ToString() +"_"+ _fileName.text); } }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickDeleteSlot);
    }

    public void HideDeleteSlot()
    {
        gameObject.SetActive(false);
    }

    public void ShowDeleteSlot()
    {
        gameObject.SetActive(true);
    }

    public void SetFileNameText(string fileName)
    {
        _originFileName = fileName;
        _fileName.text = _originFileName.Substring(2);
    }

    public void ClickDeleteSlot()
    {
        OnClickEvent(_originFileName);
    }
}
