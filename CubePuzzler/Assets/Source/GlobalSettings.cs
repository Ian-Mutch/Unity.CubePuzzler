using UnityEngine;

[CreateAssetMenu(fileName = "NewGlobalSettings", menuName = "CubePuzzler/Global Settings", order = 0)]
public class GlobalSettings : ScriptableObject
{
    #region Properties

    public float PlayerRotationSpeed => this._playerRotationSpeed;

    #endregion

    #region Fields

    [SerializeField] private float _playerRotationSpeed;

    #endregion
}
