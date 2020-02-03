using UnityEngine;

public class Blinky : BaseGhost
{
    public override void ReadyForStart()
    {
        _isAnyGhostAtePackMan = false;
        _isDie = false;
        _isInGhostHouse = true;
        _isEatablePackMan = true;
		_isPossibleTurnBack = false;

		_firstInGhostHouseMoveUpDwonCount = 0;
        _speed = _normalSpeed;

        SetPositionInHalfTile(13, 14, 11);
        SetScatterTargetPos(0, 26);

        _animator.SetInteger("moveDirection", (int)MoveDirection.Left);

        SetMoveInAndOutGhostHouseEventNull();
        MoveInAndOutGhostHouseEvent += MoveRightHalfTileWhenOutGhostHouse;
    }

    protected override Vector3 GetTargetPosition()
    {
        if (_isDie)
        {
            return _ghostHousePos;
        }

        if (GhostModeChanger.Instance.CurrentGhostMode == GhostMode.Chase ||
            GhostModeChanger.Instance.CurrentGhostMode == GhostMode.PermanentChase)
        {
            _targetTile.position = PackManPos;
            return _targetTile.position;
        }

        else //Scatter & Scare Mode
        {
            _targetTile.position = _scatterTargetPos;
            return _targetTile.position;
        }
    }
}
