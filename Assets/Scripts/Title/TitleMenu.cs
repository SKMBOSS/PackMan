using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneBuildIndex
{
    Title,
    MainGame,
    Editor,
}

public class TitleMenu : MonoBehaviour
{
    private void Start()
    {
        SaveOriginalMapFromResToLocal();
    }


    private void SaveOriginalMapFromResToLocal()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(@"OriginalMap/OriginalMap");
        string jsonData = textAsset.text;

        FileIO.CreateJsonFile(Application.persistentDataPath + @"/Maps/OriginalMap", "OriginalMap", jsonData);
    }

    public void SelectMainGameSlot()
    {
        SceneManager.LoadScene((int)SceneBuildIndex.MainGame);
    }

    public void SelectEditorSlot()
    {
        SceneManager.LoadScene((int)SceneBuildIndex.Editor);
    }

}
