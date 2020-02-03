using UnityEngine;

public enum GhostMode
{
    Scatter,
    Chase,
    PermanentChase,
    Scare,
}

public class GhostModeChanger : MonoBehaviour
{
    private static GhostModeChanger _instance;
    public static GhostModeChanger Instance { get { return _instance; } }
    private void Awake() { _instance = this; }

    [SerializeField] BaseGhost[] _ghosts;
    [Space]
    [SerializeField] GhostMode _currentGhostMode;
    [SerializeField] GhostMode _previousGhostMode;
    private bool _isChangeScatterToOtherOrChaseToOther = false;
    private int _receiveTurnBackCount = 0;
    [Space]
    [SerializeField] int _scatterModeEndCount = 0;
    [SerializeField] float _scatterModeResetTime = 7f;
    [SerializeField] float _currentScatterModeTime = 0.0f;
    [Space]
    [SerializeField] float _chaseModeResetTime = 20.0f;
    [SerializeField] float _currentChaseModeTime = 0.0f;
    [Space]
    [SerializeField] float _scareModeResetTime = 7.0f;
    [SerializeField] float _currentScareModeTime = 0.0f;

    private bool _isStartWhiteMode = false;

    public GhostMode CurrentGhostMode { get { return _currentGhostMode; } }

    public void ReadyForStart()
    {
        _currentGhostMode = GhostMode.Scatter;
        _previousGhostMode = GhostMode.Scatter;

        _receiveTurnBackCount = 0;

        _scatterModeEndCount = 0;
        _scatterModeResetTime = 7f;
        _currentScatterModeTime = 0.0f;

        _chaseModeResetTime = 20.0f;
        _currentChaseModeTime = 0.0f;

        _scareModeResetTime = 7.0f;
        _currentScareModeTime = 0.0f;

        _isStartWhiteMode = false;
    }

    private void Update()
    {
        if (MainGameManager.Instance.IsPlay)
        {
            if (_currentGhostMode == GhostMode.Scatter)
            {
                _currentScatterModeTime += Time.deltaTime;
                if (_currentScatterModeTime >= _scatterModeResetTime)
                {
                    _scatterModeEndCount++;
                    _currentScatterModeTime = 0.0f;

                    if (_scatterModeEndCount < 4)
                    {
                        ChangeMode(GhostMode.Chase);
                        _scatterModeResetTime -= 1.0f;
                    }
                    else
                    {
                        ChangeMode(GhostMode.PermanentChase);
                    }
                }
            }
            else if (_currentGhostMode == GhostMode.Chase)
            {
                _currentChaseModeTime += Time.deltaTime;
                if (_currentChaseModeTime >= _chaseModeResetTime)
                {
                    _currentChaseModeTime = 0.0f;

                    ChangeMode(GhostMode.Scatter);
                }
            }
            else if (_currentGhostMode == GhostMode.Scare)
            {
                if (_scareModeResetTime - _currentScareModeTime <= 2.0f && _isStartWhiteMode == false)
                {
                    foreach (BaseGhost ghost in _ghosts)
                    {
                        ghost.ChangeAnimationScareToWhiteScare();
                    }
                    _isStartWhiteMode = true;
                }

                _currentScareModeTime += Time.deltaTime;
                if (_currentScareModeTime >= _scareModeResetTime)
                {
                    _currentScareModeTime = 0.0f;

                    foreach (BaseGhost ghost in _ghosts)
                    {
                        ghost.ChangeAnimationScareToOther();
                    }

                    _isStartWhiteMode = false;
                    ChangeMode(_previousGhostMode);

                    MainGameManager.Instance.ReceiveScareModeEnd();
                }
            }
        }
    }

    private void ChangeMode(GhostMode ghostMode)
    {
        if (_currentGhostMode != GhostMode.Scare)
        {
			foreach (BaseGhost ghost in _ghosts)
			{
				ghost.ReceieveCanTurnBack();
			}
        }

        _previousGhostMode = _currentGhostMode;
        _currentGhostMode = ghostMode;
    }

    public void ReceiveAteBigFood()
    {
        if (_currentGhostMode == GhostMode.Scare)
        {
            _currentScareModeTime = 0.0f;
            _isStartWhiteMode = false;

            foreach (BaseGhost ghost in _ghosts)
            {
                ghost.ChangeAnimationWhiteScareToScare();
            }
        }
        else
        {
            foreach (BaseGhost ghost in _ghosts)
            {
                ghost.ChangeAnimationOtherToScare();
            }

            ChangeMode(GhostMode.Scare);
        }
    }
}
