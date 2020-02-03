using UnityEngine;

public class Clyde : BaseGhost
{
    private bool _isClydeMode = false;

    public override void ReadyForStart()
    {
        _isAnyGhostAtePackMan = false;
        _isDie = false;
        _isInGhostHouse = true;
        _isEatablePackMan = true;
		_isPossibleTurnBack = false;

		_firstInGhostHouseMoveUpDwonCount = 10;
        _speed = _firstInGhostHouseSpeed;

        SetPositionInHalfTile(15, 16, 14);
        SetScatterTargetPos(30, 0);

        _animator.SetInteger("moveDirection", (int)MoveDirection.Up);
        ResetAnimationPrameters();

        SetMoveInAndOutGhostHouseEventNull();
        MoveInAndOutGhostHouseEvent += MoveUpHalfTileInGhostHouse;
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
            CanChangeCyldeModeWithInXTilesOfPackMan(7);

            if (_isClydeMode)
            {
                _targetTile.position = _scatterTargetPos;
                return _targetTile.position;
            }
            else
            {
                _targetTile.position = PackManPos;
                return _targetTile.position;
            }
        }

        else
        {
            _targetTile.position = _scatterTargetPos;
            return _targetTile.position;
        }
    }

    private void CanChangeCyldeModeWithInXTilesOfPackMan(int rangeX)
    {
        float minX = PackManPos.x - rangeX;
        float maxX = PackManPos.x + rangeX;

        float minY = PackManPos.y - rangeX;
        float maxY = PackManPos.y + rangeX;

        if (minX <= transform.position.x && transform.position.x <= maxX &&
            minY <= transform.position.y && transform.position.y <= maxY)
        {
            _isClydeMode = true;
        }
        else
        {
            _isClydeMode = false;
        }
    }
}

