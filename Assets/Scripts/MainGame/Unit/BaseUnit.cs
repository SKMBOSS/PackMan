using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    Right,
    Left,
    Up,
    Down,
}

public abstract class BaseUnit : MonoBehaviour
{
    [SerializeField] protected TileMap _tileMap;
    [Space]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _spriteMoveControl;

    protected MoveDirection _moveDirection;
    protected float _speed = 5.5f;
    protected int _currentPosY;
    protected int _currentPosX;

	public Vector3 SpritePos { get { return _spriteMoveControl.position; } }

	public abstract void ReadyForStart();
    protected abstract void TranslatePosition();
    protected abstract void Move();

    protected void MoveRight()
    {
        _spriteMoveControl.localPosition += Vector3.right * _speed * Time.deltaTime;

        if (_spriteMoveControl.localPosition.x >= 1)
        {
            _spriteMoveControl.localPosition = Vector3.zero;
            _currentPosX++;
            TranslatePosition();
        }
    }

    protected void MoveLeft()
    {
        _spriteMoveControl.localPosition -= Vector3.right * _speed * Time.deltaTime;
        if (_spriteMoveControl.localPosition.x <= -1)
        {
            _spriteMoveControl.localPosition = Vector3.zero;
            _currentPosX--;
            TranslatePosition();
        }
    }

    protected void MoveUp()
    {
        _spriteMoveControl.localPosition += Vector3.up * _speed * Time.deltaTime;

        if (_spriteMoveControl.localPosition.y >= 1)
        {
            _spriteMoveControl.localPosition = Vector3.zero;
            _currentPosY--;
            TranslatePosition();
        }
    }

    protected void MoveDown()
    {
        _spriteMoveControl.localPosition -= Vector3.up * _speed * Time.deltaTime;
        if (_spriteMoveControl.localPosition.y <= -1)
        {
            _spriteMoveControl.localPosition = Vector3.zero;
            _currentPosY++;
            TranslatePosition();
        }
    }

    protected void ChangeMoveRightAnimation()
    {
        _moveDirection = MoveDirection.Right;
        _animator.SetInteger("moveDirection", (int)MoveDirection.Right);
    }

    protected void ChangeMoveLeftAnimation()
    {
        _moveDirection = MoveDirection.Left;
        _animator.SetInteger("moveDirection", (int)MoveDirection.Left);
    }

    protected void ChangeMoveUpAnimation()
    {
        _moveDirection = MoveDirection.Up;
        _animator.SetInteger("moveDirection", (int)MoveDirection.Up);
    }

    protected void ChangeMoveDownAnimation()
    {
        _moveDirection = MoveDirection.Down;
        _animator.SetInteger("moveDirection", (int)MoveDirection.Down);
    }

    protected void SetStartPos(int y, int x)
    {
        _currentPosY = y;
        _currentPosX = x;
        transform.position = _tileMap.GetTile(_currentPosY, _currentPosX).transform.position;
        _spriteMoveControl.localPosition = Vector3.zero;
    }

    protected void SetUnitInTile()
    {
        transform.position = _tileMap.GetTile(_currentPosY, _currentPosX).transform.position;
    }

    protected void SetPositionInHalfTile(int leftX, int rightX, int y)
    {
        float homeStartPosX = (_tileMap.GetTile(14, leftX).transform.position.x + _tileMap.GetTile(14, rightX).transform.position.x) / 2;
        float homeStartPosY = _tileMap.GetTile(y, 13).transform.position.y;

        transform.position = new Vector3(homeStartPosX, homeStartPosY, 0);
        _spriteMoveControl.localPosition = Vector3.zero;
    }

    protected void CheckTeleport()
    {
        if (_currentPosX == _tileMap.MaxSizeX)
            _currentPosX = 0;

        else if (_currentPosX == -1)
            _currentPosX = _tileMap.MaxSizeX - 1;

        if (_currentPosY == _tileMap.MaxSizeY)
            _currentPosY = 0;

        else if (_currentPosY == -1)
            _currentPosY = _tileMap.MaxSizeY - 1;
    }

    public void DisappearFromMap()
    {
        gameObject.SetActive(false);
    }

    public void AppraerOnMap()
    {
        gameObject.SetActive(true);
    }

    public void OnAnimatorSpeed()
    {
        _animator.speed = 1;
    }

    public void OffAnimatorSpeed()
    {
        _animator.speed = 0;
    }
}
