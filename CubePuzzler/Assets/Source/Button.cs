using UnityEngine;

public class Button : MonoBehaviour
{
    #region Fields

    [SerializeField] private Triggerable _target;

    #endregion

    #region Unity Messages

    /// <summary>
    ///     Called when a trigger collider enters this objects trigger collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        this._target.Trigger();
    }

    #endregion
}
