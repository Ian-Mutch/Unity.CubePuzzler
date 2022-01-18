using System;

public delegate void MemoryPuzzleBlockEventHandler(MemoryPuzzleBlockEventArgs e);

public class MemoryPuzzleBlockEventArgs : EventArgs
{
    #region Properties

    public MemoryPuzzleBlock Block { get; }

    #endregion

    #region Constructors

    public MemoryPuzzleBlockEventArgs(MemoryPuzzleBlock block)
    {
        this.Block = block;
    }

    #endregion
}
