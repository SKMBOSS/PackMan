using UnityEngine;

public class Inky : BaseGhost
{
    [SerializeField] Transform _blinky;

    public override void ReadyForStart()
    {
        _isAnyGhostAtePackMan = false;
        _isDie = false;
        _isInGhostHouse = true;
        _isEatablePackMan = true;
		_isPossibleTurnBack = false;

		_firstInGhostHouseMoveUpDwonCount = 5;
        _speed = _firstInGhostHouseSpeed;

        SetPositionInHalfTile(11, 12, 14);
        SetScatterTargetPos(30, 27);

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
            if (PackManCurrentDirection == MoveDirection.Right)
            {
                Vector2 pivot = _blinky.position;
                Vector2 target = PackManPos + new Vector3(2, 0, 0);

                _targetTile.position = GetVectorBetweenTwoPoints(pivot, target);
                return _targetTile.position;
            }

            else if (PackManCurrentDirection == MoveDirection.Left)
            {
                Vector2 pivot = _blinky.position;
                Vector2 target = PackManPos + new Vector3(-2, 0, 0);

                _targetTile.position = GetVectorBetweenTwoPoints(pivot, target);
                return _targetTile.position;

            }

            else if (PackManCurrentDirection == MoveDirection.Up)
            {
                Vector2 pivot = _blinky.position;
                Vector2 target = PackManPos + new Vector3(0, 2, 0);

                _targetTile.position = GetVectorBetweenTwoPoints(pivot, target);

                _targetTile.position = PackManPos + new Vector3(0, 2, 0);
                return _targetTile.position;

            }

            else
            {
                Vector2 pivot = _blinky.position;
                Vector2 target = PackManPos + new Vector3(0, -2, 0);

                _targetTile.position = GetVectorBetweenTwoPoints(pivot, target);
                return _targetTile.position;
            }
        }

        else //Scatter & Scare Mode
        {
            _targetTile.position = _scatterTargetPos;
            return _targetTile.position;
        }
    }

    private Vector2 GetVectorBetweenTwoPoints(Vector2 pivot, Vector2 target)
    {
        return target * 2 - pivot;
    }
}

