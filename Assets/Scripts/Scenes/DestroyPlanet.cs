using UnityEngine;

class DestroyPlanet : MonoBehaviour
{
    #region Script paremeters
#pragma warning disable 649
    [SerializeField] Camera mainCamera;
#pragma warning disable 649

    Vector2 screenBounds;
    #endregion

    #region MonoBehaviour API
    void Start()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            mainCamera.transform.position.z));
    }

    void Update()
    {
        Clean();
    }
    #endregion

    #region Custom methods
    void Clean()
    {
        if (transform.position.y >= screenBounds.y + 20.0f || transform.position.y <= screenBounds.y - 20.0f)
            Destroy(gameObject);
    }
    #endregion
}
