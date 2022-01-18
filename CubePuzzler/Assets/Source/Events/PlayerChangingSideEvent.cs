using System;
using UnityEngine;

public delegate void PlayerChangingSideEventHandler(PlayerChangingSideEventArgs e);

public class PlayerChangingSideEventArgs : EventArgs
{
    #region Properties

    public Side OldSide { get; set; }
    public Side NewSide { get; set; }
    public bool ChangingToInnerSide { get; set; }
    public Vector3 OldPosition { get; set; }
    public Vector3 NewPosition { get; set; }
    public Direction Direction { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    ///     Creates an instance of <see cref="PlayerChangingSideEventArgs"/>
    /// </summary>
    /// <param name="oldSide"></param>
    /// <param name="newSide"></param>
    /// <param name="changingToInnerSide"></param>
    /// <param name="oldPosition"></param>
    /// <param name="newPosition"></param>
    /// <param name="direction"></param>
    public PlayerChangingSideEventArgs(Side oldSide, Side newSide, bool changingToInnerSide, Vector3 oldPosition, Vector3 newPosition, Direction direction)
    {
        this.OldSide = oldSide;
        this.NewSide = newSide;
        this.ChangingToInnerSide = changingToInnerSide;
        this.OldPosition = oldPosition;
        this.NewPosition = newPosition;
        this.Direction = direction;
    }

    #endregion
}
