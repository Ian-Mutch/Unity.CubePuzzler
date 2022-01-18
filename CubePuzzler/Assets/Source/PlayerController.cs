using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Player _player;

    #endregion

    #region Unity Messages

    /// <summary>
    ///     Called every frame
    /// </summary>
    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            this._player.Move(Direction.Forward);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            this._player.Move(Direction.Back);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            this._player.Move(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            this._player.Move(Direction.Right);
        }

        if(!this._player.IsMoving)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                LevelManager.Instance.RotateState(Direction.Left);
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                LevelManager.Instance.RotateState(Direction.Right);
            }
        }
    }

    #endregion
}
