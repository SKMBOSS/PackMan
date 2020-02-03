using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWindow : MonoBehaviour
{
    [SerializeField] EditTileMap _tileMapEdit;
    [Space]
    [SerializeField] SaveSlot[] _saveSlots;
    [Space]
    [SerializeField] GameObject _blockRayCast;
    [SerializeField] GameObject _blockRayCastSecond;
    [Space]
    [SerializeField] FileNameEditField _inputFiled;

    private void Awake()
    {
        InitSaveSlots();
    }

    private void Start()
    {
        for (int i = 0; i < _saveSlots.Length; i++)
        {
            _saveSlots[i].OnClickFileSlotEvent += SaveFile;
            _saveSlots[i].OnClickNewSlotEvent += OpenInputFiled;
            _inputFiled.OnClickSaveButtonInEditFieldEvent += SaveFile;
        }
    }

    public void OpenSaveWindow()
    {
        _blockRayCast.SetActive(true);
        ShowSaveWindow();

        string folderPath = Application.persistentDataPath + @"/Maps";
        string fileExtension = ".json";

        List<string> fileNames = FileIO.GetFileNamesInFolder(folderPath, fileExtension);

        for (int i = 0; i < fileNames.Count; i++)
        {
            _saveSlots[i].SetFileNameText(fileNames[i]);
            _saveSlots[i].ShowSaveSlot();
            _saveSlots[i].IsNewSlot = false;
        }

        if(fileNames.Count < _saveSlots.Length)
        { 
            _saveSlots[fileNames.Count].ShowSaveSlot();
            _saveSlots[fileNames.Count].FileName = "New";
        }
    }

    public void HideSaveWindow()
    {
        _blockRayCast.SetActive(false);
        HideSaveeSlots();
        gameObject.SetActive(false);
    }

    private void ShowSaveWindow()
    {
        gameObject.SetActive(true);
    }

    private void HideSaveeSlots()
    {
        foreach (SaveSlot saveSlot in _saveSlots)
        {
            saveSlot.HideSaveSlot();
            saveSlot.IsNewSlot = true;
        }
    }

    private void InitSaveSlots()
    {
        int slotNumber = 0;
        foreach (SaveSlot saveSlot in _saveSlots)
        {
            saveSlot.SlotNumber = slotNumber++;
        }
    }

    private void OpenInputFiled(int slotNumber)
    {
        _inputFiled.SelectedNewSlotNumber = slotNumber;

        _blockRayCastSecond.SetActive(true);
        _inputFiled.ShowEditField();
    }

    private void SaveFile(string fileName)
    {
        _tileMapEdit.Save(fileName);
        HideSaveWindow();
    }
}
