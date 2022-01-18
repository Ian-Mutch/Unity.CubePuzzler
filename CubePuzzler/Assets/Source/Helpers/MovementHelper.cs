using UnityEngine;

public static class MovementHelper
{
    #region Functions

    /// <summary>
    ///     Calculates a rotation axis based on a direction of movement
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 CalculateRotationAxis(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Vector3.Cross(LevelManager.Instance.State.Up, LevelManager.Instance.State.Forward);
            case Direction.Back:
                return Vector3.Cross(LevelManager.Instance.State.Up, LevelManager.Instance.State.Back);
            case Direction.Left:
                return Vector3.Cross(LevelManager.Instance.State.Up, LevelManager.Instance.State.Left);
            case Direction.Right:
                return Vector3.Cross(LevelManager.Instance.State.Up, LevelManager.Instance.State.Right);
            default:
                throw new System.Exception($"{direction} is not supported when calculating a rotation axis"); //TODO: Change this to use a custom typed exception
        }
    }

    #endregion
}
