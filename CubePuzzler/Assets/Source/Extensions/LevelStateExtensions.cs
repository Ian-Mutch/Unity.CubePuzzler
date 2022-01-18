using UnityEngine;

public static class LevelStateExtensions
{
    #region Functions

    /// <summary>
    ///     Calculates the next state based on side and direction moved
    /// </summary>
    /// <param name="self"></param>
    /// <param name="side"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static LevelState CalculateNextState(this LevelState self, Side side, Direction direction)
    {
        var result = new LevelState(self)
        {
            Side = side,
            Up = side.ToDirectionVector()
        };

        switch (direction)
        {
            case Direction.Forward:
            case Direction.Back:
                result.Forward = Vector3.Cross(result.Right, result.Up);
                break;
            case Direction.Right:
            case Direction.Left:
                result.Right = Vector3.Cross(result.Up, result.Forward);
                break;
        }

        return result;
    }

    /// <summary>
    ///     Rotates the state
    /// </summary>
    /// <param name="direction"></param>
    public static LevelState RotateState(this LevelState self, Direction direction)
    {
        var result = new LevelState(self);

        switch (direction)
        {
            case Direction.Left:
                result.Forward = -self.Right;
                result.Right = self.Forward;
                break;
            case Direction.Right:
                result.Forward = self.Right;
                result.Right = -self.Forward;
                break;
            default:
                throw new System.Exception("Only left or right directions can be handled in the function RotateState");
        }

        return result;
    }


    /// <summary>
    ///     Converts a side to a direction vector
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Vector3 ToDirectionVector(this Side self)
    {
        switch (self)
        {
            case Side.Top:
                return Vector3.up;
            case Side.Bottom:
                return Vector3.down;
            case Side.Right:
                return Vector3.right;
            case Side.Left:
                return Vector3.left;
            case Side.Front:
                return Vector3.forward;
            default: // Side.Back:
                return Vector3.back;
        }
    }

    /// <summary>
    ///     Converts a vector to a side
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Side ToSide(this Vector3 self)
    {
        switch (self)
        {
            case var v when v == Vector3.up:
                return Side.Top;
            case var v when v == Vector3.down:
                return Side.Bottom;
            case var v when v == Vector3.left:
                return Side.Left;
            case var v when v == Vector3.right:
                return Side.Right;
            case var v when v == Vector3.forward:
                return Side.Front;
            case var v when v == Vector3.back:
                return Side.Back;
        }

        throw new System.Exception($"Cannot convert Vector3 {self} to Side");
    }

    /// <summary>
    ///     Converts a vector to a direction
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Direction ToDirection(this Vector3 self)
    {
        switch (self)
        {
            case var v when v == LevelManager.Instance.State.Forward:
                return Direction.Forward;
            case var v when v == LevelManager.Instance.State.Back:
                return Direction.Back;
            case var v when v == LevelManager.Instance.State.Right:
                return Direction.Right;
            case var v when v == LevelManager.Instance.State.Left:
                return Direction.Left;
            case var v when v == LevelManager.Instance.State.Up:
                return Direction.Up;
            case var v when v == LevelManager.Instance.State.Down:
                return Direction.Down;
        }

        throw new System.Exception($"Cannot convert Vector3 {self} to Direction");
    }

    /// <summary>
    ///     Converts a direction to a vector
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Vector3 ToVector3(this Direction self) => LevelState.CalculateDirectionVector(LevelManager.Instance.State, self);

    #endregion
}
