using System;

public delegate void PlayerMoveEventHandler(PlayerMoveEventArgs e);

public class PlayerMoveEventArgs : EventArgs
{
    #region Properties

    public Side OldSide { get; set; }
    public Side NewSide { get; set; }
    public Direction Direction { get; }

    #endregion

    #region Constructors

    public PlayerMoveEventArgs(Side oldSide, Side newSide, Direction direction)
    {
        this.OldSide = oldSide;
        this.NewSide = newSide;
        this.Direction = direction;
    }

    #endregion
}