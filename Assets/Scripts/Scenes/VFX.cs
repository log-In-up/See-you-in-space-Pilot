using System.Collections;
using UnityEngine;

class VFX : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] float lifeTime = 0.3f;
    #endregion

    #region MonoBehaviour API
    void OnEnable()
    {
        StartCoroutine(Destruction());
    }
    #endregion

    #region Custom methods
    IEnumerator Destruction()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    #endregion
}