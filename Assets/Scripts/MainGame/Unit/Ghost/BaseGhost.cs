using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGhost : BaseUnit
{
	[SerializeField] SpriteRenderer _spriteRenderer;

	[SerializeField] protected Transform _targetTile;
	protected Vector3 _scatterTargetPos;
	protected Vector3 _ghostHousePos;

	protected MoveDirection _selectedDirection;

	protected bool _isAnyGhostAtePackMan;
	protected bool _isDie;
	protected bool _isInGhostHouse;
	protected bool _isEatablePackMan;
	[SerializeField] protected bool _isPossibleTurnBack;

	protected int _firstInGhostHouseMoveUpDwonCount;
	protected float _normalSpeed = 5.5f;
	protected const float _scareModeSpeed = 3.5f;
	protected const float _dieModeSpeed = 30.0f;
	protected const float _exitGhostHouseSpeed = 2.0f;
	protected const float _firstInGhostHouseSpeed = 4.0f;

	protected const float _hitBoxHalfHeight = 0.5f;
	protected const float _hitBoxHalfWidth = 0.5f;

	protected Vector3 PackManPos { get { return MainGameManager.Instance.PancManPosition; } }
	protected Vector3 PackManSpritePos { get { return MainGameManager.Instance.PackManSpritePosition; } }
	protected MoveDirection PackManCurrentDirection { get { return MainGameManager.Instance.PackManCurrentDirection; } }

	protected event Action MoveInAndOutGhostHouseEvent;

	protected abstract Vector3 GetTargetPosition();

	protected virtual void Update()
	{
		if (MainGameManager.Instance.IsPlay)
		{
			if (!_isAnyGhostAtePackMan)
			{
				if (_isInGhostHouse)
				{
					MoveInAndOutGhostHouseEvent?.Invoke();
				}

				else
				{
					Move();

					CheckCatchOrDie();

					if (_isDie)
					{
						CheckArriveGhostHouse();
					}
				}
			}
		}
	}

	protected void SetScatterTargetPos(int y, int x)
	{
		_scatterTargetPos = _tileMap.GetTile(y, x).transform.position;
	}

	protected override void TranslatePosition()
	{
		CheckTeleport();
		SetUnitInTile();
		CheckConnectedTile();
		CheckSelectedDirection();
	}

	protected override void Move()
	{
		if (_moveDirection == MoveDirection.Right)
		{
			MoveRight();
		}
		else if (_moveDirection == MoveDirection.Left)
		{
			MoveLeft();
		}
		else if (_moveDirection == MoveDirection.Up)
		{
			MoveUp();
		}
		else if (_moveDirection == MoveDirection.Down)
		{
			MoveDown();
		}
	}

	public void ChangeAnimationOtherToScare()
	{
		if (!_isDie)
		{
			if (!_isInGhostHouse)
			{
				_speed = _scareModeSpeed;
			}
			_isEatablePackMan = false;
			_animator.SetBool("scareMode", true);
		}
	}

	public void ChangeAnimationScareToOther()
	{
		if (!_isDie)
		{
			_speed = _normalSpeed;
		}
		_isEatablePackMan = true;
		_animator.SetBool("scareMode", false);
		_animator.SetBool("scareWhiteMode", false);
	}

	public void ChangeAnimationScareToWhiteScare()
	{
		_animator.SetBool("scareWhiteMode", true);
	}

	public void ChangeAnimationWhiteScareToScare()
	{
		_isEatablePackMan = false;
		_animator.SetBool("scareMode", true);
		_animator.SetBool("scareWhiteMode", false);
	}

	protected void CheckArriveGhostHouse()
	{
		if (_tileMap.GetTile(_currentPosY, _currentPosX).transform.position == _ghostHousePos)
		{
			_isInGhostHouse = true;

			if (_currentPosX <= 13)
			{
				MoveInAndOutGhostHouseEvent += MoveRightHalfTileWhenInGhostHouse;
			}
			else
			{
				MoveInAndOutGhostHouseEvent += MoveLeftHalfTileWhenInGhostHouse;
			}
		}
	}

	protected void CheckCatchOrDie()
	{
		if (_isDie)
			return;

		//Check HitBox Range
		if (SpritePos.x >= PackManSpritePos.x - _hitBoxHalfHeight &&
			SpritePos.x <= PackManSpritePos.x + _hitBoxHalfHeight
			&&
			SpritePos.y >= PackManSpritePos.y - _hitBoxHalfWidth &&
			SpritePos.y <= PackManSpritePos.y + _hitBoxHalfWidth)
		{
			if (_isEatablePackMan)
			{
				MainGameManager.Instance.ReceiveAtePackMan();
			}
			else
			{
				_speed = _dieModeSpeed;
				_isDie = true;
				_animator.SetBool("dieMode", true);
				_animator.SetBool("scareMode", false);

				if (_currentPosX <= 13)
				{
					_ghostHousePos = _tileMap.GetTile(11, 13).transform.position;
				}
				else
				{
					_ghostHousePos = _tileMap.GetTile(11, 14).transform.position;
				}
				MainGameManager.Instance.ReceiveGhostIsEaten(this);
			}
		}
	}

	protected void CheckConnectedTile()
	{
		if (_isPossibleTurnBack)
		{
			if(this is Blinky)
			{
				Debug.Log("시발");
			}
			TurnBack();
		}
		else
		{
			CheckConnectedTileEqaulTwo();
			CheckConnectedTileMoreThanTwo();
		}
	}

	protected void CheckSelectedDirection()
	{
		if (_selectedDirection == _moveDirection)
			return;
		else
		{
			if (_selectedDirection == MoveDirection.Right)
			{
				ChangeMoveRightAnimation();
			}
			else if (_selectedDirection == MoveDirection.Left)
			{
				ChangeMoveLeftAnimation();
			}
			else if (_selectedDirection == MoveDirection.Up)
			{
				ChangeMoveUpAnimation();
			}
			else if (_selectedDirection == MoveDirection.Down)
			{
				ChangeMoveDownAnimation();
			}
		}
	}

	public void ReceieveCanTurnBack()
	{
		if (!_isAnyGhostAtePackMan && !_isInGhostHouse )
		{
			_isPossibleTurnBack = true;
		}
	}

	protected void TurnBack()
	{
		if (!_isDie)
		{
			if (_moveDirection == MoveDirection.Right)
			{
				_selectedDirection = MoveDirection.Left;
			}
			else if (_moveDirection == MoveDirection.Left)
			{
				_selectedDirection = MoveDirection.Right;
			}
			else if (_moveDirection == MoveDirection.Up)
			{
				_selectedDirection = MoveDirection.Down;
			}
			else
			{
				_selectedDirection = MoveDirection.Up;
			}
		}
		_isPossibleTurnBack = false;
	}

	protected void CheckConnectedTileEqaulTwo()
	{
		if (_tileMap.GetTile(_currentPosY, _currentPosX).ConnctedTilesInfo.Count == 2)
		{
			foreach (ConnectedTile connectedTile in _tileMap.GetTile(_currentPosY, _currentPosX).ConnctedTilesInfo)
			{
				if ((int)connectedTile == (int)MoveDirection.Right)
				{
					if (_moveDirection == MoveDirection.Left)
						continue;
				}

				else if ((int)connectedTile == (int)MoveDirection.Left)
				{
					if (_moveDirection == MoveDirection.Right)
						continue;
				}

				else if ((int)connectedTile == (int)MoveDirection.Up)
				{
					if (_moveDirection == MoveDirection.Down)
						continue;
				}

				else if ((int)connectedTile == (int)MoveDirection.Down)
				{
					if (_moveDirection == MoveDirection.Up)
						continue;
				}

				_selectedDirection = (MoveDirection)connectedTile;
				break;
			}
		}
	}

	protected void CheckConnectedTileMoreThanTwo()
	{
		//priority : Up>Left>Down>Right

		if (_tileMap.GetTile(_currentPosY, _currentPosX).ConnctedTilesInfo.Count > 2)
		{
			float shortestDistance = float.MaxValue;
			float distance = 0.0f;

			foreach (ConnectedTile connectedTile in _tileMap.GetTile(_currentPosY, _currentPosX).ConnctedTilesInfo)
			{
				if ((int)connectedTile == (int)MoveDirection.Up)
				{
					if (_moveDirection == MoveDirection.Down)
						continue;

					distance = _tileMap.GetTile(_currentPosY - 1, _currentPosX).SqrMagnitude(GetTargetPosition());
				}

				else if ((int)connectedTile == (int)MoveDirection.Left)
				{
					if (_moveDirection == MoveDirection.Right)
						continue;

					distance = _tileMap.GetTile(_currentPosY, _currentPosX - 1).SqrMagnitude(GetTargetPosition());
				}

				else if ((int)connectedTile == (int)MoveDirection.Down)
				{
					if (_moveDirection == MoveDirection.Up)
						continue;

					distance = _tileMap.GetTile(_currentPosY + 1, _currentPosX).SqrMagnitude(GetTargetPosition());
				}

				else if ((int)connectedTile == (int)MoveDirection.Right)
				{
					if (_moveDirection == MoveDirection.Left)
						continue;

					distance = _tileMap.GetTile(_currentPosY, _currentPosX + 1).SqrMagnitude(GetTargetPosition());
				}

				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					_selectedDirection = (MoveDirection)connectedTile;
				}
			}
		}
	}

	protected void ResetAnimationPrameters()
	{
		_animator.SetBool("scareMode", false);
		_animator.SetBool("scareWhiteMode", false);
		_animator.SetBool("dieMode", false);
	}

	public void SomeoneAtePacMan()
	{
		_isAnyGhostAtePackMan = true;
	}

	protected void MoveOutGhostHouse()
	{
		_spriteMoveControl.localPosition += Vector3.up * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.y >= 3)
		{
			if (GhostModeChanger.Instance.CurrentGhostMode == GhostMode.Scare)
			{
				_speed = _scareModeSpeed;
			}
			else
			{
				_speed = _normalSpeed;
			}

			MoveInAndOutGhostHouseEvent = null;

			if (_targetTile.position.x >= 14)
			{
				_animator.SetInteger("moveDirection", (int)MoveDirection.Right);
				MoveInAndOutGhostHouseEvent += MoveRightHalfTileWhenOutGhostHouse;
			}
			else
			{
				_animator.SetInteger("moveDirection", (int)MoveDirection.Left);
				MoveInAndOutGhostHouseEvent += MoveLeftHalfTileWhenOutGhostHouse;
			}
		}
	}

	protected void MoveRightHalfTileWhenOutGhostHouse()
	{
		_spriteMoveControl.localPosition += Vector3.right * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.x >= 0.5)
		{
			_isInGhostHouse = false;

			SetStartPos(11, 14);
			_spriteMoveControl.localPosition = Vector3.zero;

			_animator.SetInteger("moveDirection", (int)MoveDirection.Right);
			_moveDirection = MoveDirection.Right;
			_selectedDirection = MoveDirection.Right;

			MoveInAndOutGhostHouseEvent = null;
		}
	}

	protected void MoveLeftHalfTileWhenOutGhostHouse()
	{
		_spriteMoveControl.localPosition -= Vector3.right * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.x <= -0.5)
		{
			_isInGhostHouse = false;

			SetStartPos(11, 13);
			_spriteMoveControl.localPosition = Vector3.zero;

			_animator.SetInteger("moveDirection", (int)MoveDirection.Left);
			_moveDirection = MoveDirection.Left;
			_selectedDirection = MoveDirection.Left;

			MoveInAndOutGhostHouseEvent = null;
		}
	}

	protected void MoveInGhostHouse()
	{
		_spriteMoveControl.localPosition -= Vector3.up * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.y <= -3)
		{
			_isDie = false;
			_speed = _exitGhostHouseSpeed;
			_isEatablePackMan = true;
			_animator.SetBool("dieMode", false);

			_animator.SetInteger("moveDirection", (int)MoveDirection.Up);

			SetPositionInHalfTile(13, 14, 14);
			_spriteMoveControl.localPosition = Vector3.zero;

			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveOutGhostHouse;
		}
	}

	protected void MoveRightHalfTileWhenInGhostHouse()
	{
		_spriteMoveControl.localPosition += Vector3.right * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.x >= 0.5)
		{
			_speed = _normalSpeed;
			ChangeMoveDownAnimation();

			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveInGhostHouse;
		}
	}

	protected void MoveLeftHalfTileWhenInGhostHouse()
	{
		_spriteMoveControl.localPosition -= Vector3.right * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.x <= -0.5)
		{
			_speed = _normalSpeed;
			ChangeMoveDownAnimation();

			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveInGhostHouse;
		}
	}

	protected void MoveUpHalfTileInGhostHouse()
	{
		_spriteMoveControl.localPosition += Vector3.up * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.y >= 0.5)
		{
			if (_firstInGhostHouseMoveUpDwonCount <= 0)
			{
				_spriteMoveControl.localPosition = Vector3.zero;

				if (transform.position.x < _tileMap.GetTile(11, 13).transform.position.x)
				{
					MoveInAndOutGhostHouseEvent = null;
					_animator.SetInteger("moveDirection", (int)MoveDirection.Right);
					MoveInAndOutGhostHouseEvent += MoveRightInGhostHouse;
				}
				else
				{
					MoveInAndOutGhostHouseEvent = null;
					_animator.SetInteger("moveDirection", (int)MoveDirection.Left);
					MoveInAndOutGhostHouseEvent += MoveLeftInGhostHouse;
				}
			}
			else
			{
				_animator.SetInteger("moveDirection", (int)MoveDirection.Down);
				MoveInAndOutGhostHouseEvent = null;
				MoveInAndOutGhostHouseEvent += MoveDownInGhostHouse;
			}
		}
	}

	protected void MoveDownInGhostHouse()
	{
		_spriteMoveControl.localPosition -= Vector3.up * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.y <= -0.5)
		{
			_animator.SetInteger("moveDirection", (int)MoveDirection.Up);
			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveUpInGhostHouse;
		}
	}

	protected void MoveUpInGhostHouse()
	{
		_spriteMoveControl.localPosition += Vector3.up * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.y >= 0.5)
		{
			_animator.SetInteger("moveDirection", (int)MoveDirection.Down);
			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveDownHalfTileInGhostHouse;
		}
	}

	protected void MoveDownHalfTileInGhostHouse()
	{
		_spriteMoveControl.localPosition -= Vector3.up * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.y <= -0.5)
		{
			_firstInGhostHouseMoveUpDwonCount--;

			_animator.SetInteger("moveDirection", (int)MoveDirection.Up);
			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveUpHalfTileInGhostHouse;
		}
	}

	protected void MoveRightInGhostHouse()
	{
		_spriteMoveControl.localPosition += Vector3.right * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.x >= 2)
		{
			_spriteMoveControl.localPosition = Vector3.zero;
			SetPositionInHalfTile(13, 14, 14);

			_speed = _exitGhostHouseSpeed;

			_animator.SetInteger("moveDirection", (int)MoveDirection.Up);
			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveOutGhostHouse;
		}
	}

	protected void MoveLeftInGhostHouse()
	{
		_spriteMoveControl.localPosition -= Vector3.right * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.x <= -2)
		{
			_spriteMoveControl.localPosition = Vector3.zero;
			SetPositionInHalfTile(13, 14, 14);

			_speed = _exitGhostHouseSpeed;

			_animator.SetInteger("moveDirection", (int)MoveDirection.Up);
			MoveInAndOutGhostHouseEvent = null;
			MoveInAndOutGhostHouseEvent += MoveOutGhostHouse;
		}
	}

	public void EnableSpriteRenderer()
	{
		_spriteRenderer.enabled = true;
	}

	public void DisableSpriteRenderer()
	{
		_spriteRenderer.enabled = false;
	}

	protected void SetMoveInAndOutGhostHouseEventNull()
	{
		MoveInAndOutGhostHouseEvent = null;
	}
}
