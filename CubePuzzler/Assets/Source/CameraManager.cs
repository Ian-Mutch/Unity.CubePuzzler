using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private Player _player;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _offset;
    private bool _overrideRotation;

    #endregion

    #region Unity Messages

    /// <summary>
    ///     Called when the component/gameobject is enabled
    /// </summary>
    private void OnEnable()
    {
        LevelManager.Instance.OnStateRotated += this.LevelManager_OnStateRotated;

        this._player.OnChangingSideBefore += this.Player_OnChangingSideBefore;
    }

    /// <summary>
    ///     Called when the component/gameobject is disabled
    /// </summary>
    private void OnDisable()
    {
        if(LevelManager.Instance)
        {
            LevelManager.Instance.OnStateRotated -= this.LevelManager_OnStateRotated;
        }
    }

    /// <summary>
    ///     Called every frame
    /// </summary>
    private void Update()
    {
        if(!this._overrideRotation)
        {
            this._camera.transform.position = this._player.transform.position + this.CalculateOffset(LevelManager.Instance.State);
        }
    }

    #endregion

    #region Functions

    /// <summary>
    ///     Calculates the cameras offset
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateOffset(LevelState state) =>
        state.Right * this._offset.x +
        state.Up * this._offset.y +
        state.Forward * this._offset.z;

    /// <summary>
    ///     Returns an enumerator that rotates the camera
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    private IEnumerator GetEnumerator_Rotate(bool changingToInnerWall, Vector3 oldPosition, Vector3 newPosition)
    {
        var fromPosition = oldPosition + this.CalculateOffset(LevelManager.Instance.PreviousState);
        var toPosition = newPosition + this.CalculateOffset(LevelManager.Instance.State);
        var fromRotation = this._camera.transform.parent.rotation;
        var toRotation = Quaternion.LookRotation(LevelManager.Instance.State.Forward, LevelManager.Instance.State.Up);
        var angle = 0f;
        var targetAngle = changingToInnerWall ? 90f : 180f;

        this._overrideRotation = true;

        yield return new WaitUntil(() =>
        {
            var delta = GameManager.Instance.GlobalSettings.PlayerRotationSpeed * Time.deltaTime;
            angle = Mathf.Clamp(angle + delta, 0, targetAngle);

            this._camera.transform.parent.rotation = Quaternion.Slerp(fromRotation, toRotation, angle / targetAngle);
            this._camera.transform.position = Vector3.Slerp(fromPosition, toPosition, angle / targetAngle);
            
            return angle == targetAngle;
        });

        this._overrideRotation = false;
    }

    #endregion

    #region Events

    /// <summary>
    ///     Handles the level manager event <see cref="LevelManager.OnStateRotated"/>
    /// </summary>
    /// <param name="e"></param>
    private void LevelManager_OnStateRotated(StateRotationChangedEventArgs e)
    {
        this.StartCoroutine(this.GetEnumerator_Rotate(false, this._player.transform.position, this._player.transform.position));
    }

    /// <summary>
    ///     Handles the player event <see cref="Player.OnChangingSideBefore"/>
    /// </summary>
    /// <param name="e"></param>
    private void Player_OnChangingSideBefore(PlayerChangingSideEventArgs e)
    {
        this.StartCoroutine(this.GetEnumerator_Rotate(e.ChangingToInnerSide, e.OldPosition, e.NewPosition));
    }

    #endregion
}
