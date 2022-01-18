using UnityEngine;

public class LevelState
{
    #region Properties

    public Side Side { get; set; }
    public Vector3 Up { get; set; } = Vector3.up;
    public Vector3 Down => -this.Up;
    public Vector3 Forward { get; set; } = Vector3.forward;
    public Vector3 Back => -this.Forward;
    public Vector3 Right { get; set; } = Vector3.right;
    public Vector3 Left => -this.Right;

    #endregion

    #region Constructors

    /// <summary>
    ///     Creates an instance of <see cref="LevelState"/>
    /// </summary>
    public LevelState()
    {

    }

    /// <summary>
    ///     Creates an instance of <see cref="LevelState"/> from an existing instance of <see cref="LevelState"/>
    /// </summary>
    /// <param name="levelState"></param>
    public LevelState(LevelState levelState)
    {
        this.Side = levelState.Side;
        this.Up = levelState.Up;
        this.Forward = levelState.Forward;
        this.Right = levelState.Right;
    }

    #endregion

    #region Functions

    /// <summary>
    ///     Calculates the direction vector from a state and given direction
    /// </summary>
    /// <param name="state"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 CalculateDirectionVector(LevelState state, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return state.Up;
            case Direction.Down:
                return state.Down;
            case Direction.Forward:
                return state.Forward;
            case Direction.Back:
                return state.Back;
            case Direction.Right:
                return state.Right;
            default: //Direction.Left
                return state.Left;
        }
    }

    #endregion
}
