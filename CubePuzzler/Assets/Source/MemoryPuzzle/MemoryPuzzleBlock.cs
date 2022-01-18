using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MemoryPuzzleBlock : MonoBehaviour
{
    #region Event Handlers

    public event MemoryPuzzleBlockEventHandler OnTriggered;

    #endregion

    #region Fields

    private MeshRenderer _meshRenderer;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        this._meshRenderer = this.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        this.OnTriggered?.Invoke(new MemoryPuzzleBlockEventArgs(this));
    }

    #endregion

    #region Functions

    public void Highlight(Color color, float time)
    {
        this.StartCoroutine(this.GetEnumerator_Highlight(color, time));
    }

    private IEnumerator GetEnumerator_Highlight(Color color, float time)
    {
        this._meshRenderer.material.color = color;

        yield return new WaitForSeconds(time);

        this._meshRenderer.material.color = Color.white;
    }

    #endregion
}
