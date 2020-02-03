using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnTitleButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickReturnTitleButton);
    }

    public void ClickReturnTitleButton()
    {
        SceneManager.LoadScene((int)SceneBuildIndex.Title);
    }
}
