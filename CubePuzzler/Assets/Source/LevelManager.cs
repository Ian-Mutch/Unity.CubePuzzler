using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Event Handlers

    public event StateRotationChangedEventHandler OnStateRotated;

    #endregion

    #region Properties

    public static LevelManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LevelManager>();
                }

                return _instance;
            }
        }
    }
    public LevelState PreviousState { get; private set; }
    public LevelState State { get; private set; } = new LevelState();

    #endregion

    #region Fields

    private static object _lock = new object();
    private static LevelManager _instance;
    [SerializeField] private Player _player;

    #endregion

    #region Unity Messages

    /// <summary>
    ///     Called when the component/gameobject is enabled
    /// </summary>
    private void OnEnable()
    {
        this._player.OnMoveBefore += this.Player_OnMoveBefore;
    }

    /// <summary>
    ///     Called before start
    /// </summary>
    private void Awake()
    {
        if (_instance != null && _instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    #endregion

    #region Functions

    /// <summary>
    ///     Rotates the current level state
    /// </summary>
    /// <param name="direction"></param>
    public void RotateState(Direction direction)
    {
        this.PreviousState = this.State;
        this.State = this.State.RotateState(direction);

        this.OnStateRotated?.Invoke(new StateRotationChangedEventArgs(direction));
    }

    #endregion

    #region Events

    /// <summary>
    ///     Handles the player event <see cref="Player.OnMoveBefore"/>
    /// </summary>
    /// <param name="e"></param>
    private void Player_OnMoveBefore(PlayerMoveEventArgs e)
    {
        if (e.OldSide != e.NewSide)
        {
            this.PreviousState = this.State;
            this.State = this.State.CalculateNextState(e.NewSide, e.Direction);
        }
    }

    #endregion
}
