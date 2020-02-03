using System;
using UnityEngine;
using UnityEngine.UI;

public class FileNameEditField : MonoBehaviour
{
    [SerializeField] Text _inputText;
    [SerializeField] int _selectedNewSlotNumber;
    [SerializeField] GameObject _blockRayCastSecond;

    public event Action<string> OnClickSaveButtonInEditFieldEvent;

    public int SelectedNewSlotNumber { set { _selectedNewSlotNumber = value; } }

    private void HideEditField()
    {
        gameObject.SetActive(false);
    }

    public void ShowEditField()
    {
        gameObject.SetActive(true);
    }

    public void ClickSaveButton()
    {
        OnClickSaveButtonInEditFieldEvent(_selectedNewSlotNumber + "_" + _inputText.text);
        HideEditField();
        _blockRayCastSecond.SetActive(false);
    }

}
