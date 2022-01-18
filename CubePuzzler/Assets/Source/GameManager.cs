using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Properties

    public static GameManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                }

                return _instance;
            }
        }
    }
    public GlobalSettings GlobalSettings => this._globalSettings;

    #endregion

    #region Fields

    private static object _lock = new object();
    private static GameManager _instance;
    [SerializeField] private GlobalSettings _globalSettings;

    #endregion

    #region Unity Messages

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
}
