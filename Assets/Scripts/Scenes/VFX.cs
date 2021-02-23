using System.Collections;
using UnityEngine;

public class VFX : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private float destructionTime;
    #endregion

    #region MonoBehaviour API
    private void OnEnable()
    {
        StartCoroutine(Destruction());
    }
    #endregion

    #region Custom methods
    private IEnumerator Destruction()
    {
        yield return new WaitForSeconds(destructionTime);
        Destroy(gameObject);
    }
    #endregion
}