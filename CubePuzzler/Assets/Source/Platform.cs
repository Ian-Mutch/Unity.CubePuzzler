using UnityEngine;

public class Platform : MonoBehaviour
{
    #region Properties

    public float Height
    {
        get => this.transform.localPosition.y;
        set
        {
            var position = this.transform.localPosition;
            position.y = value;

            this.transform.localPosition = position;
        }
    }

    #endregion
}
