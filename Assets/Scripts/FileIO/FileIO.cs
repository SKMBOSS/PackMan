using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class FileIO
{
    public static string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public static T JsonToOject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    public static void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        if (!Directory.Exists(createPath))
        {
            Directory.CreateDirectory(createPath);
        }

        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public static T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }

    public static List<string> GetFileNamesInFolder(string folderPath, string fileExtension)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        List<string> fileNames = new List<string>();
       
        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
            if (fileInfo.Extension.ToLower().CompareTo(fileExtension) == 0)
            {
                fileNames.Add(fileInfo.Name.Substring(0, fileInfo.Name.Length - fileExtension.Length));
            }
        }
        return fileNames;
    }
}
