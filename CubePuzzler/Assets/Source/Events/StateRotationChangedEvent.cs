using System;

public delegate void StateRotationChangedEventHandler(StateRotationChangedEventArgs e);

public class StateRotationChangedEventArgs : EventArgs
{
    #region Properties

    public Direction Direction { get; }

    #endregion

    #region Constructors

    public StateRotationChangedEventArgs(Direction direction)
    {
        this.Direction = direction;
    }

    #endregion
}
