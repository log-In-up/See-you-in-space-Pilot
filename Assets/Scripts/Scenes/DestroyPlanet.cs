using UnityEngine;

class DestroyPlanet : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private Camera mainCamera;
    private Vector2 screenBounds;
    #endregion

    #region MonoBehaviour API
    private void Start()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            mainCamera.transform.position.z));
    }

    private void Update()
    {
        Clean();
    }
    #endregion

    #region Custom methods
    private void Clean()
    {
        if (transform.position.y >= screenBounds.y + 20.0f || transform.position.y <= screenBounds.y - 20.0f)
            Destroy(gameObject);
    }
    #endregion
}
