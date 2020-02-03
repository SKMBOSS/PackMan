using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWindow : MonoBehaviour
{
    [SerializeField] EditTileMap _tileMapEdit;
    [Space]
    [SerializeField] LoadSlot[] _loadSlots;
    [Space]
    [SerializeField] GameObject _blockRayCast;

    private void Awake()
    {
        InitLoadSlots();
    }

    private void Start()
    {
        for (int i = 0; i < _loadSlots.Length; i++)
        {
            _loadSlots[i].OnClickEvent += LoadFile;
        }
    }

    public void OpenLoadWindow()
    {
        _blockRayCast.SetActive(true);
        ShowLoadWindow();

        string folderPath = Application.persistentDataPath + @"/Maps";
        string fileExtension = ".json";

        List<string> fileNames = FileIO.GetFileNamesInFolder(folderPath, fileExtension);
        
        for (int i = 0; i < fileNames.Count; i++)
        {
            _loadSlots[i].SetFileNameText(fileNames[i]);
            _loadSlots[i].ShowLoadSlot();
        }
    }

    public void HideLoadWindow()
    {
        _blockRayCast.SetActive(false);
        HideLoadeSlots();
        gameObject.SetActive(false);
    }

    private void ShowLoadWindow()
    {
        gameObject.SetActive(true);
    }

    private void HideLoadeSlots()
    {
        foreach (LoadSlot loadSlot in _loadSlots)
        {
            loadSlot.HideLoadSlot();
        }
    }

    private void InitLoadSlots()
    {
        int slotNumber = 0;
        foreach(LoadSlot loadSlot in _loadSlots)
        {
            loadSlot.SlotNumber = slotNumber++;
        }
    }

    private void LoadFile(string fileName)
    {
        _tileMapEdit.Load(fileName);
    }
}
