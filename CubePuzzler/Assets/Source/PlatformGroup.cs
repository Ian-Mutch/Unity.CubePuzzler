using System.Collections;
using UnityEngine;

public class PlatformGroup : Triggerable
{
    #region Fields

    [SerializeField] private Platform[] _platforms;
    [SerializeField] private float _startingHeight;
    [SerializeField] private float _targetHeight;
    [SerializeField] private float _raiseInterval;
    [SerializeField] private float _raiseSpeed;
    private bool _canRaise = true;

    #endregion

    #region Unity Messages

    /// <summary>
    ///     Called on the first frame
    /// </summary>
    private void Start()
    {
        foreach (var platform in this._platforms)
        {
            platform.Height = this._startingHeight;
        }
    }

    #endregion

    #region Functions

    /// <summary>
    ///     Raises the platforms if they can be raised
    /// </summary>
    public override void Trigger()
    {
        if (this._canRaise)
        {
            this._canRaise = false;

            this.StartCoroutine(this.GetEnumerator_RaisePlatforms());
        }
    }

    /// <summary>
    ///     Returns an enumerator to raise each platform
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetEnumerator_RaisePlatforms()
    {
        var count = this._platforms.Length;

        for (int i = 0; i < count; i++)
        {
            this.StartCoroutine(this.GetEnumerator_RaisePlatform(this._platforms[i]));

            if (i < count - 1)
            {
                yield return new WaitForSeconds(this._raiseInterval);
            }
        }
    }

    /// <summary>
    ///     Returns an enumerator to raise a platform
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    private IEnumerator GetEnumerator_RaisePlatform(Platform platform)
    {
        yield return new WaitUntil(() =>
        {
            platform.Height = Mathf.MoveTowards(platform.Height, this._targetHeight, this._raiseSpeed * Time.deltaTime);

            return platform.Height == this._targetHeight;
        });

        this._canRaise = true;
    }

    #endregion
}
