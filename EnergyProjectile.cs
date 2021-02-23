using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] Camera mainCamera;
    [SerializeField] float damage = 20.0f;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemy = collision.GetComponent<EnemyAI>();
        if (enemy != null)
            enemy.TakeDamage(damage);
        
        Destroy(gameObject);
    }
    #endregion

    #region Custom methods
    void Clean()
    {
        if (transform.position.y >= screenBounds.y + 20.0f ||transform.position.y <= screenBounds.y - 20.0f)
            Destroy(gameObject);
    }
    #endregion
}
