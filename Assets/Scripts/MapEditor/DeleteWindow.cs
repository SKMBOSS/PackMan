using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWindow : MonoBehaviour
{
    [SerializeField] DeleteSlot[] _deleteSlots;
    [Space]
    [SerializeField] GameObject _blockRayCast;

    private void Awake()
    {
        InitDeleteSlots();
    }

    private void Start()
    {
        for (int i = 0; i < _deleteSlots.Length; i++)
        {
            _deleteSlots[i].OnClickEvent += DeleteFile;
        }
    }

    public void OpenDeleteWindow()
    {
        _blockRayCast.SetActive(true);
        ShowDeleteWindow();

        string folderPath = Application.persistentDataPath + @"/Maps";
        string fileExtension = ".json";

        List<string> fileNames = FileIO.GetFileNamesInFolder(folderPath, fileExtension);

        for (int i = 0; i < fileNames.Count; i++)
        {
            _deleteSlots[i].SetFileNameText(fileNames[i]);
            _deleteSlots[i].ShowDeleteSlot();
        }
    }

    public void HideDeleteWindow()
    {
        _blockRayCast.SetActive(false);
        HideDeleteSlots();
        gameObject.SetActive(false);
    }

    private void ShowDeleteWindow()
    {
        gameObject.SetActive(true);
    }

    private void HideDeleteSlots()
    {
        foreach (DeleteSlot deleteSlot in _deleteSlots)
        {
            deleteSlot.HideDeleteSlot();
        }
    }

    private void InitDeleteSlots()
    {
        int slotNumber = 0;
        foreach (DeleteSlot deleteSlot in _deleteSlots)
        {
            deleteSlot.SlotNumber = slotNumber++;
        }
    }

    private void DeleteFile(string fileName)
    {
        string folderPath = Application.persistentDataPath + @"/Maps";
        string fileExtension = ".json";

        System.IO.File.Delete(folderPath + @"/" + fileName + fileExtension);
        ReNameFile(folderPath, fileExtension);

        HideDeleteWindow();
    }

    private void ReNameFile(string folderPath, string fileExtension)
    {
        System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(folderPath);
        int currentDeleteSlotNumber = 0;

        foreach (System.IO.FileInfo fileInfo in directoryInfo.GetFiles())
        {
            if (fileInfo.Extension.ToLower().CompareTo(fileExtension) == 0)
            {
                string newFileName = folderPath + @"/" + currentDeleteSlotNumber++.ToString() + "_" + fileInfo.Name.Substring(2);
                System.IO.File.Move(fileInfo.FullName, newFileName);
            }
        }
    }
}
