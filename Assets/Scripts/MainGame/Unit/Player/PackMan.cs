using UnityEngine;

public class PackMan : BaseUnit
{
	private bool _isMoveable = true;
	private bool _isAlive = true;
	private bool _isGetReadyDie = false;
	private bool _isInHalfTile = true;

	private float _currentDieWaitTime = 0.0f;

	private MoveDirection _inputDirection;

	public MoveDirection CurrentDirection { get { return _moveDirection; } }

	private void Update()
	{
		if (MainGameManager.Instance.IsPlay)
		{
			if (_isAlive)
			{
				if (_isInHalfTile)
				{
					MoveLeftHalfTile();
				}

				else
				{
					GetKey();

					if (_isMoveable)
					{
						Move();
					}
				}
			}

			else
			{
				if (_isGetReadyDie)
				{
					CheckDieAnimationEnd();
				}

				else
				{
					WaitDieForSeconds(1.0f);
				}
			}
		}
	}

	public override void ReadyForStart()
	{
		_isMoveable = true;
		_isAlive = true;
		_isGetReadyDie = false;
		_isInHalfTile = true;

		_currentDieWaitTime = 0.0f;

		SetPositionInHalfTile(13, 14, 23);

		_animator.SetInteger("moveDirection", (int)MoveDirection.Left);
		_moveDirection = MoveDirection.Left;
		_inputDirection = MoveDirection.Left;
	}

	protected override void TranslatePosition()
	{
		CheckTeleport();
		SetUnitInTile();
		EatFood();
		CheckInputDirection();
	}

	protected override void Move()
	{
		if (_moveDirection == MoveDirection.Right)
		{
			if (IsObstacle(_currentPosY, _currentPosX + 1))
			{
				Stop();
			}
			else
			{
				MoveRight();
			}
		}
		else if (_moveDirection == MoveDirection.Left)
		{
			if (IsObstacle(_currentPosY, _currentPosX - 1))
			{
				Stop();
			}
			else
			{
				MoveLeft();
			}
		}
		else if (_moveDirection == MoveDirection.Up)
		{
			if (IsObstacle(_currentPosY - 1, _currentPosX))
			{
				Stop();
			}
			else
			{
				MoveUp();
			}
		}
		else if (_moveDirection == MoveDirection.Down)
		{
			if (IsObstacle(_currentPosY + 1, _currentPosX))
			{
				Stop();
			}
			else
			{
				MoveDown();
			}
		}
	}

	protected void Stop()
	{
		_animator.speed = 0;
		_isMoveable = false;
	}

	public void PrepareDie()
	{
		_isAlive = false;
		_animator.speed = 0;
	}

	private bool IsObstacle(int y, int x)
	{
		//Prevent OutofRange X
		if (x == _tileMap.MaxSizeX) x = 0;
		else if (x == -1) x = _tileMap.MaxSizeX - 1;
		//Prevent OutofRange Y
		if (y == _tileMap.MaxSizeY) y = 0;
		else if (y == -1) y = _tileMap.MaxSizeY - 1;

		return _tileMap.GetTile(y, x).IsObstacle;
	}

	private void EatFood()
	{
		if (_tileMap.GetTile(_currentPosY, _currentPosX).TileType == TileType.Food)
		{
			_tileMap.SetTargetToBlankTile(_currentPosY, _currentPosX);
			MainGameManager.Instance.ReceivePackManAteFoodOrBigFoodWithScore(10);
		}

		else if (_tileMap.GetTile(_currentPosY, _currentPosX).TileType == TileType.BigFood)
		{
			_tileMap.SetTargetToBlankTile(_currentPosY, _currentPosX);
			_tileMap.RemoveBlinkFoodInBigFoodTiles(_tileMap.GetTile(_currentPosY, _currentPosX));

			GhostModeChanger.Instance.ReceiveAteBigFood();
			MainGameManager.Instance.ReceivePackManAteFoodOrBigFoodWithScore(50);
		}
	}

	private void CheckInputDirection()
	{
		if (_inputDirection == _moveDirection)
			return;
		else
		{
			if (_inputDirection == MoveDirection.Right)
			{
				if (!IsObstacle(_currentPosY, _currentPosX + 1))
				{
					ChangeMoveRightAnimation();
				}
			}
			else if (_inputDirection == MoveDirection.Left)
			{
				if (!IsObstacle(_currentPosY, _currentPosX - 1))
				{
					ChangeMoveLeftAnimation();
				}
			}
			else if (_inputDirection == MoveDirection.Up)
			{
				if (!IsObstacle(_currentPosY - 1, _currentPosX))
				{
					ChangeMoveUpAnimation();
				}
			}
			else if (_inputDirection == MoveDirection.Down)
			{
				if (!IsObstacle(_currentPosY + 1, _currentPosX))
				{
					ChangeMoveDownAnimation();
				}
			}
		}
	}

	public void ClickRightArrow()
	{
		if (_isAlive && !_isInHalfTile)
		{
			_inputDirection = MoveDirection.Right;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY, _currentPosX + 1))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveRightAnimation();
				}
			}
		}
	}

	public void ClickLeftArrow()
	{
		if (_isAlive && !_isInHalfTile)
		{
			_inputDirection = MoveDirection.Left;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY, _currentPosX - 1))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveLeftAnimation();
				}
			}
		}
	}

	public void ClickUpArrow()
	{
		if (_isAlive && !_isInHalfTile)
		{
			_inputDirection = MoveDirection.Up;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY - 1, _currentPosX))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveUpAnimation();
				}
			}
		}
	}

	public void ClickDownArrow()
	{
		if (_isAlive && !_isInHalfTile)
		{
			_inputDirection = MoveDirection.Down;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY + 1, _currentPosX))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveDownAnimation();
				}
			}
		}
	}

	private void GetKey()
	{
		if (Input.GetKey(KeyCode.RightArrow) == true)
		{
			_inputDirection = MoveDirection.Right;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY, _currentPosX + 1))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveRightAnimation();
				}
			}
		}

		if (Input.GetKey(KeyCode.LeftArrow) == true)
		{
			_inputDirection = MoveDirection.Left;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY, _currentPosX - 1))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveLeftAnimation();
				}
			}
		}

		if (Input.GetKey(KeyCode.UpArrow) == true)
		{
			_inputDirection = MoveDirection.Up;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY - 1, _currentPosX))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveUpAnimation();
				}
			}
		}

		if (Input.GetKey(KeyCode.DownArrow) == true)
		{
			_inputDirection = MoveDirection.Down;

			if (!_isMoveable)
			{
				if (!IsObstacle(_currentPosY + 1, _currentPosX))
				{
					_animator.speed = 1;
					_isMoveable = true;
					ChangeMoveDownAnimation();
				}
			}
		}
	}

	private void WaitDieForSeconds(float seconds)
	{
		_currentDieWaitTime += Time.deltaTime;

		if (_currentDieWaitTime >= seconds)
		{
			_currentDieWaitTime = 0.0f;
			_isGetReadyDie = true;
			_animator.speed = 1;
			_animator.SetBool("isDie", true);

			MainGameManager.Instance.ReceivePackManReadyToDie();
		}
	}

	private void CheckDieAnimationEnd()
	{
		if (!_animator.IsInTransition(0))
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				DisappearFromMap();
				MainGameManager.Instance.ReceivePackManDieComplete();
			}
		}
	}

	public void ChangeAnimationIdleToStart()
	{
		_animator.SetBool("isWaitStart", false);
	}

	public void ChangeAnimationMoveToIdle()
	{
		_animator.SetBool("isWaitStart", true);
		_animator.SetBool("isClear", true);
	}

	protected void MoveLeftHalfTile()
	{
		_spriteMoveControl.localPosition -= Vector3.right * _speed * Time.deltaTime;

		if (_spriteMoveControl.localPosition.x <= -0.5)
		{
			_isInHalfTile = false;

			SetStartPos(23, 13);
			_spriteMoveControl.localPosition = Vector3.zero;
		}
	}
}
