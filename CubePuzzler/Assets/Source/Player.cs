using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Event Handlers

    public event PlayerMoveEventHandler OnMoveBefore;
    public event PlayerChangingSideEventHandler OnChangingSideBefore;
    public event PlayerChangingSideEventHandler OnChangingSideAfter;

    #endregion

    #region Properties

    public bool EnableMovement { get; set; } = true;
    public bool EnableFalling
    {
        get => _enableFalling;
        set => _enableFalling = value;
    }
    public bool IsMoving => _isMoving;

    #endregion

    #region Fields

    [SerializeField] private float _mass;
    private bool _isMoving;
    private bool _isFalling;
    [SerializeField] private bool _enableWallMovement;
    [SerializeField] private bool _enableFalling;
    [SerializeField] private LayerMask _wallCheck;
    [SerializeField] private float _fallCheckLength;

    #endregion

    #region Functions

    /// <summary>
    ///     Moves the player in a direction
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Direction direction)
    {
        var result = CanMove(direction);

        if(!result.CanMove) return;

        OnMoveBefore?.Invoke(new PlayerMoveEventArgs(result.OldSide, result.NewSide, direction));

        _isMoving = true;

        StartCoroutine(GetEnumerator_Move(result));
    }

    /// <summary>
    ///     Checks if the player can move
    /// </summary>
    /// <param name="requestDirection"></param>
    /// <returns></returns>
    private MoveResult CanMove(Direction requestDirection)
    {
        var result = new MoveResult();

        if (!(EnableMovement && !_isMoving && !_isFalling))
            return result;

        var directionVector = requestDirection.ToVector3();

        if (Physics.Raycast(transform.position, directionVector, out var hitDirection, 1, _wallCheck, QueryTriggerInteraction.Ignore))
        {
            if (_enableWallMovement)
            {
                result.CanMove = true;
                result.ChangingSide = true;
                result.ChangingToInnerWall = true;
                result.RaycastHitNormal = hitDirection.normal;
                result.OldSide = LevelManager.Instance.State.Side;
                result.NewSide = hitDirection.normal.ToSide();
            }
        }
        else
        {
            var rayOrigin = transform.position + directionVector * 1.1f;
            var rayDirection = ((transform.position + LevelManager.Instance.State.Down * 1.1f) - rayOrigin).normalized;

            Physics.Raycast(rayOrigin, rayDirection, out var hitDown, 1.5f, _wallCheck, QueryTriggerInteraction.Ignore);

            if (hitDown.normal == LevelManager.Instance.State.Up)
                result.CanMove = true;
            else if (_enableWallMovement)
            {
                result.CanMove = true;
                result.ChangingSide = true;
                result.ChangingToOuterWall = true;
                result.RaycastHitNormal = hitDown.normal;
                result.OldSide = LevelManager.Instance.State.Side;
                result.NewSide = hitDown.normal.ToSide();
            }
            else if (CanFall())
            {
                Physics.Raycast(rayOrigin, LevelManager.Instance.State.Down, out hitDown, _fallCheckLength, _wallCheck, QueryTriggerInteraction.Ignore);

                result.CanMove = true;
                result.WillFall = true;
                result.FallDistance = hitDown.transform ? hitDown.distance : Mathf.Infinity;
            }
        }

        if (result.CanMove)
        {
            result.Direction = requestDirection;

            if (result.ChangingToInnerWall)
            {
                var state = LevelManager.Instance.State.CalculateNextState(result.NewSide, requestDirection);

                result.DirectionVector = LevelState.CalculateDirectionVector(state, requestDirection);
                result.Pivot = transform.position + LevelManager.Instance.State.Up * .5f + directionVector * .5f;
            }
            else
            {
                result.DirectionVector = directionVector;
                result.Pivot = transform.position + LevelManager.Instance.State.Down * .5f + directionVector * .5f;
            }

            result.Axis = requestDirection.CalculateRotationAxis();
        }

        return result;
    }

    /// <summary>
    ///     Returns true if the player can fall
    /// </summary>
    /// <returns></returns>
    private bool CanFall() => EnableFalling;

    /// <summary>
    ///     Returns an enumerator that moves the cube based on a set of conditions provided by <paramref name="result"/>
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private IEnumerator GetEnumerator_Move(MoveResult result)
    {
        PlayerChangingSideEventArgs changingSideEventArgs = null;

        if (result.ChangingSide)
        {
            var newPosition = transform.position + result.DirectionVector;
            if(result.ChangingToInnerWall)
                newPosition += LevelManager.Instance.PreviousState.Down;

            changingSideEventArgs = new PlayerChangingSideEventArgs(result.OldSide, result.NewSide, result.ChangingToInnerWall, transform.position, newPosition, result.Direction);

            OnChangingSideBefore?.Invoke(changingSideEventArgs);
        }

        yield return GetEnumerator_Rotate(result);

        if(result.ChangingSide)
            OnChangingSideAfter?.Invoke(changingSideEventArgs);

        if (result.WillFall)
            StartCoroutine(GetEnumerator_Fall(result.FallDistance));

        _isMoving = false;
    }

    /// <summary>
    ///     Returns an enumerator that rotates the cube based on a set of conditions provided by <paramref name="result"/>
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private IEnumerator GetEnumerator_Rotate(MoveResult result)
    {
        var angle = 0f;
        var targetAngle = result.ChangingSide ? (result.ChangingToInnerWall ? 90 : 180) : 90;

        yield return new WaitUntil(() =>
        {
            var delta = GameManager.Instance.GlobalSettings.PlayerRotationSpeed * Time.deltaTime;

            angle += delta;

            transform.RotateAround(result.Pivot, result.Axis, delta);

            if (angle >= targetAngle)
            {
                var difference = angle - targetAngle;

                transform.RotateAround(result.Pivot, result.Axis, -difference);

                var position = transform.position;
                var x = (float)Math.Round(position.x * 2) * .5f;
                var y = (float)Math.Round(position.y * 2) * .5f;
                var z = (float)Math.Round(position.z * 2) * .5f;
                transform.position = new Vector3(x, y, z);
                return true;
            }

            return false;
        });
    }

    /// <summary>
    ///     Returns an enumerator to make the player fall
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    private IEnumerator GetEnumerator_Fall(float distance)
    {
        var actualDistance = distance - .5f;
        var direction = LevelManager.Instance.State.Down;

        _isFalling = true;

        yield return new WaitUntil(() =>
        {
            var delta = _mass * Time.deltaTime;
            var newPosition = transform.position;

            actualDistance -= delta;
            newPosition = transform.position + direction * delta;

            if (actualDistance < 0)
            {
                newPosition += direction * actualDistance;
                actualDistance = 0;
            }

            transform.position = newPosition;

            return actualDistance == 0;
        });

        _isFalling = false;
    }

    #endregion

    #region Classes & Structs

    private struct MoveResult
    {
        public bool CanMove { get; set; }
        public bool ChangingSide { get; set; }
        public bool ChangingToInnerWall { get; set; }
        public bool ChangingToOuterWall { get; set; }
        public Side OldSide { get; set; }
        public Side NewSide { get; set; }
        public bool WillFall { get; set; }
        public float FallDistance { get; set; }
        public Vector3 RaycastHitNormal { get; set; }
        public Direction Direction { get; set; }
        public Vector3 DirectionVector { get; set; }
        public Vector3 Pivot { get; set; }
        public Vector3 Axis { get; set; }
    }

    #endregion
}
