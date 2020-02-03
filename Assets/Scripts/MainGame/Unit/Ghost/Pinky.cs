using UnityEngine;

public class Pinky : BaseGhost
{
    public override void ReadyForStart()
    {
        _isAnyGhostAtePackMan = false;
        _isDie = false;
        _isInGhostHouse = true;
        _isEatablePackMan = true;
		_isPossibleTurnBack = false;

		_normalSpeed = 5.7f;

        _firstInGhostHouseMoveUpDwonCount = 0;
        _speed = _exitGhostHouseSpeed;

        SetPositionInHalfTile(13, 14, 14);
        SetScatterTargetPos(0, 1);

        _animator.SetInteger("moveDirection", (int)MoveDirection.Up);
        ResetAnimationPrameters();

        SetMoveInAndOutGhostHouseEventNull();
        MoveInAndOutGhostHouseEvent += MoveOutGhostHouse;
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
            if (PackManCurrentDirection == MoveDirection.Right)
            {
                _targetTile.position = PackManPos + new Vector3(4, 0, 0);
                return _targetTile.position;

            }

            else if (PackManCurrentDirection == MoveDirection.Left)
            {
                _targetTile.position = PackManPos + new Vector3(-4, 0, 0);
                return _targetTile.position;

            }

            else if (PackManCurrentDirection == MoveDirection.Up)
            {
                _targetTile.position = PackManPos + new Vector3(0, 4, 0);
                return _targetTile.position;

            }

            else
            {
                _targetTile.position = PackManPos + new Vector3(0, -4, 0);
                return _targetTile.position;
            }
        }

        else //Scatter & Scare Mode
        {
            _targetTile.position = _scatterTargetPos;
            return _targetTile.position;
        }
    }
}
