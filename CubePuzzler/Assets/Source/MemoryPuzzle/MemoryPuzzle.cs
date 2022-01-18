using UnityEngine;

public class MemoryPuzzle : Triggerable
{
    #region Properties

    private MemoryPuzzleGroup CurrentPuzzleGroup
    {
        get
        {
            if (this._puzzleGroupIndex == -1)
                return null;
            return this._puzzleGroups[this._puzzleGroupIndex];
        }
    }

    #endregion

    #region Fields

    [SerializeField] private MemoryPuzzleBlock[] _blocks;
    [SerializeField] private MemoryPuzzleGroup[] _puzzleGroups;
    private int _puzzleGroupIndex = -1;
    private bool _isComplete;

    #endregion

    #region Functions

    public override void Trigger()
    {
        if(!this._isComplete)
        {
            if (this.CurrentPuzzleGroup != null && this.CurrentPuzzleGroup.IsRunning)
            {
                this.Reset();
            }
            else
            {
                this._puzzleGroupIndex++;

                this.CurrentPuzzleGroup.OnComplete += this.CurrentPuzzleGroup_OnComplete;
                this.CurrentPuzzleGroup.OnFailed += this.CurrentPuzzleGroup_OnFailed;
                this.CurrentPuzzleGroup.BeginPuzzle();
            }
        }
    }

    private void Reset()
    {
        foreach (var block in this._blocks)
        {
            block.Highlight(Color.red, 1);
        }

        this.CurrentPuzzleGroup?.EndPuzzle();

        this._puzzleGroupIndex = -1;
    }

    #endregion

    #region Events

    private void CurrentPuzzleGroup_OnFailed()
    {
        this.Reset();
    }

    private void CurrentPuzzleGroup_OnComplete()
    {
        if(this._puzzleGroupIndex == this._puzzleGroups.Length - 1)
        {
            this._isComplete = true;
        }

        this.CurrentPuzzleGroup.OnComplete -= this.CurrentPuzzleGroup_OnComplete;
        this.CurrentPuzzleGroup.OnFailed -= this.CurrentPuzzleGroup_OnFailed;
    }

    #endregion

    #region Classes

    [System.Serializable]
    public class MemoryPuzzleGroup
    {
        #region Event Handlers

        public event MemoryPuzzleGroupEventHandler OnComplete;
        public event MemoryPuzzleGroupEventHandler OnFailed;

        #endregion

        #region Properties

        public bool IsRunning { get; private set; }

        #endregion

        #region Fields

        [SerializeField] private MemoryPuzzleBlock[] _blocks;
        private int _lastActivatedBlockIndex = -1;

        #endregion

        #region Functions

        public void BeginPuzzle()
        {
            this._lastActivatedBlockIndex = -1;

            foreach (var block in this._blocks)
            {
                block.enabled = true;
                block.OnTriggered += this.Block_OnTriggered;
                block.Highlight(Color.cyan, 1);
            }

            this.IsRunning = true;
        }

        public void EndPuzzle()
        {
            foreach (var block in this._blocks)
            {
                block.enabled = false;
                block.OnTriggered -= this.Block_OnTriggered;
            }

            this.IsRunning = false;
        }

        private void Complete()
        {
            foreach (var block in this._blocks)
            {
                block.Highlight(Color.green, 1);
            }

            this.EndPuzzle();

            this.OnComplete?.Invoke();
        }

        private void Failed()
        {
            this.EndPuzzle();

            this.OnFailed?.Invoke();
        }

        #endregion

        #region Events

        private void Block_OnTriggered(MemoryPuzzleBlockEventArgs e)
        {
            var success = true;

            if (this._lastActivatedBlockIndex == -1)
            {
                if (this._blocks[0] != e.Block)
                {
                    success = false;
                }
            }
            else if (this._blocks[this._lastActivatedBlockIndex + 1] != e.Block)
            {
                success = false;
            }

            if (!success)
            {
                this.Failed();
            }
            else
            {
                this._lastActivatedBlockIndex++;

                if (this._lastActivatedBlockIndex == this._blocks.Length - 1)
                {
                    this.Complete();
                }
            }
        }

        #endregion
    }

    #endregion
}
