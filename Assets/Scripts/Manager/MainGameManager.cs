using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    private static MainGameManager _instance;
    public static MainGameManager Instance { get { return _instance; } }
    private void Awake() { _instance = this; }

    [SerializeField] PackMan _packMan;
    [Space]
    [SerializeField] BaseGhost[] _ghosts;
    [Space]
    [SerializeField] TileMap _tileMap;
    
    int _foodCount = 0;
    private BaseGhost _eatenGhost;
    private List<string> _fileNames;
    private int _currentFileNumber = 0;

    [Space]
    [SerializeField] GameObject _readyText;
    [SerializeField] GameObject _gameOverText;
    [SerializeField] GameObject _gameClearText;

    [SerializeField] Text _ghostScoreText;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _packManLifeText;

    private int _ghostAteChainCount = 0;
    private int _score = 0;
    private int _packManLife = 3;
  

    private bool _isPlay = false;

    public Vector3 PancManPosition { get { return _packMan.transform.position; } }
	public Vector3 PackManSpritePosition { get { return _packMan.SpritePos; } }
    public MoveDirection PackManCurrentDirection { get { return _packMan.CurrentDirection; } }
    public bool IsPlay { get { return _isPlay; } }

    private void Start()
    {
        _isPlay = false;

        _readyText.SetActive(true);
        _ghostAteChainCount = 0;

        _score = 0;
        SetScoreText();

        _packManLife = 3;
        SetPackManLifeText();

        GetCustomMapFileNameList();
        _currentFileNumber = 0;

        _tileMap.LoadOriginalMap(out _foodCount);

        AppearUnits();
        UnitsAnimationStop();
        ResetUnitsAndGhostModeChanger();

       
        Invoke(nameof(StartGame), 2.0f);
    }

    private void GetCustomMapFileNameList()
    {
        string folderPath = Application.persistentDataPath + @"/Maps";
        string fileExtension = ".json";

        _fileNames = FileIO.GetFileNamesInFolder(folderPath, fileExtension);
    }

    private void ResetUnitsAndGhostModeChanger()
    {
		_readyText.SetActive(true);

        _packMan.ReadyForStart();

        foreach (BaseGhost ghost in _ghosts)
        {
            ghost.ReadyForStart();
        }
        GhostModeChanger.Instance.ReadyForStart();
    }

    private void StartGame()
    {
        _isPlay = true;
        _tileMap.StartBlinkBigFood();
        _packMan.ChangeAnimationIdleToStart();

        _readyText.SetActive(false);

        UnitsAnimationStart();
    }

    private void PauseUnits()
    {
        _isPlay = false;
        UnitsAnimationStop();
    }

    private void ResumeUnits()
    {
        _isPlay = true;
        UnitsAnimationStart();
    }

    private void AppearUnits()
    {
        _packMan.AppraerOnMap();

        foreach (BaseGhost ghost in _ghosts)
        {
            ghost.AppraerOnMap();
        }
    }

    private void UnitsAnimationStart()
    {
        _packMan.OnAnimatorSpeed();

        foreach (BaseGhost ghost in _ghosts)
        {
            ghost.OnAnimatorSpeed();
        }
    }

    private void UnitsAnimationStop()
    {
        _packMan.OffAnimatorSpeed();

        foreach (BaseGhost ghost in _ghosts)
        {
            ghost.OffAnimatorSpeed();
        }
    }

    public void ReceiveGhostIsEaten(BaseGhost ghost)
    {
        PauseUnits();
        _eatenGhost = ghost;
        _eatenGhost.DisableSpriteRenderer();

        _ghostAteChainCount++;
        int chainScore = 200 * _ghostAteChainCount;

        PlusScore(chainScore);
        _ghostScoreText.text = chainScore.ToString();

        _ghostScoreText.gameObject.SetActive(true);
        _ghostScoreText.transform.position = Camera.main.WorldToScreenPoint(ghost.SpritePos);

        Invoke(nameof(EatenEffectPauseEnd), 1.0f);
    }

    private void EatenEffectPauseEnd()
    {
        _eatenGhost.EnableSpriteRenderer();
        _eatenGhost = null;

        _ghostScoreText.gameObject.SetActive(false);

        ResumeUnits();
    }

    public void StageClear()
    {
        _tileMap.StartChangeMapColor();

        foreach (BaseGhost ghost in _ghosts)
        {
            ghost.DisappearFromMap();
        }

        Invoke(nameof(ReadyNextStage), 2.5f);
    }

    private void ReadyNextStage()
    {
        _tileMap.StopChangeMapColor();
        _packMan.DisappearFromMap();

        Invoke(nameof(NextStage), 1.0f);
    }

    private void NextStage()
    {
        _isPlay = false;

        if (_currentFileNumber >= _fileNames.Count)
        {
            _gameClearText.SetActive(true);

            foreach (BaseGhost ghost in _ghosts)
            {
                ghost.DisappearFromMap();
            }

            Invoke(nameof(GameClear), 2.0f);
            return;
        }

        UpPackManLife();

        _tileMap.ClearConnectedInfoInTiles();
        _tileMap.LoadTilesFromCustomFile(_fileNames[_currentFileNumber++], out _foodCount);

        AppearUnits();
        UnitsAnimationStop();
        ResetUnitsAndGhostModeChanger();

        Invoke(nameof(StartGame), 2.0f);
    }

    private void GameClear()
    {
        _gameClearText.SetActive(false);
        SceneManager.LoadScene((int)SceneBuildIndex.Title);
    }

    private void GameOver()
    {
        _gameOverText.SetActive(false);
        SceneManager.LoadScene((int)SceneBuildIndex.Title);
    }


    public void ReceiveAtePackMan()
    {
        _packMan.PrepareDie();

        foreach (BaseGhost ghost in _ghosts)
        {
            ghost.SomeoneAtePacMan();
        }
    }

    public void ReceivePackManReadyToDie()
    {
        foreach (BaseGhost ghost in _ghosts)
        {
            ghost.DisappearFromMap();
        }
    }

    public void ReceivePackManDieComplete()
    {
        _isPlay = false;
        _tileMap.StopBlinkBigFood();

        DownPackManLife();

        if(_packManLife <= 0)
        {
            _gameOverText.SetActive(true);
            Invoke(nameof(GameOver), 2.0f);
        }

        else
        {
            AppearUnits();
            UnitsAnimationStop();
            ResetUnitsAndGhostModeChanger();
            Invoke(nameof(StartGame), 2.0f);
        }
    }

    public void ReceivePackManAteFoodOrBigFoodWithScore(int earnedScore)
    {
        PlusScore(earnedScore);

        _foodCount--;
        if (_foodCount <= 0)
        {
            _packMan.ChangeAnimationMoveToIdle();
            PauseUnits();

            Invoke(nameof(StageClear), 2.0f);
        }
    }

    public void ReceiveScareModeEnd()
    {
        _ghostAteChainCount = 0;
    }

    private string PrependNumberToZero(int number, int limitDigit)
    {
        string stringNumber = number.ToString();
        int numberDigit = stringNumber.Length;

        for (int i = 0; i < limitDigit - numberDigit; i++)
        {
            stringNumber = "0" + stringNumber;
        }

        return stringNumber;
    }

    private void PlusScore(int earnedScore)
    {
        _score += earnedScore;
        SetScoreText();
    }

    private void SetScoreText()
    {
        _scoreText.text = PrependNumberToZero(_score, 5);
    }

    public void UpPackManLife()
    {
        _packManLife++;
        _packManLifeText.text = PrependNumberToZero(_packManLife, 2);
    }

    private void DownPackManLife()
    {
        _packManLife--;
        SetPackManLifeText();
    }

    private void SetPackManLifeText()
    {
        _packManLifeText.text = PrependNumberToZero(_packManLife, 2);
    }
}
