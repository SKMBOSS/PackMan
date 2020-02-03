using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] GameObject _blockRayCast;

    [SerializeField] SpriteRenderer[] _unitPosSprites;
    [Space]
    [SerializeField] GameObject _ghostTargets;

    public void OpenPauseWindow()
    {
        Time.timeScale = 0.0F;
        _blockRayCast.SetActive(true);
        gameObject.SetActive(true);
    }

    public void HidePauseWindow()
    {
        Time.timeScale = 1.0F;
        _blockRayCast.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene((int)SceneBuildIndex.Title);
        HidePauseWindow();
    }

    public void ClearStage()
    {
        MainGameManager.Instance.StageClear();
        HidePauseWindow();
    }

    public void DiePackMan()
    {
        MainGameManager.Instance.ReceiveAtePackMan();
        HidePauseWindow();
    }

    public void UpPackManLife()
    {
        MainGameManager.Instance.UpPackManLife();
        HidePauseWindow();
    }

    public void OnOffPosAndTarGet()
    {
        foreach(SpriteRenderer unitPosSprite in _unitPosSprites)
        {
            if(unitPosSprite.enabled == true)
            {
                unitPosSprite.enabled = false;
            }
            else
            {
                unitPosSprite.enabled = true;
            }
        }

        _ghostTargets.SetActive(!_ghostTargets.activeSelf);

        HidePauseWindow();
    }
}
