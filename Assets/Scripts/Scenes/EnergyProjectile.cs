using UnityEngine;

class EnergyProjectile : MonoBehaviour
{
    #region Script paremeters
    [Header("Projectile characteristics")]
    [SerializeField] float damage = 20.0f;
#pragma warning disable 649
    [Header("Object references")]
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject destructionVFX;
    [SerializeField] Tags tags;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tags.playerTag) && !transform.CompareTag(tags.playerProjectile))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.TakeDamage(damage);

            OnHit();
        }
        else if (collision.CompareTag(tags.enemyTag) && !transform.CompareTag(tags.enemyProjectile))
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            enemy.TakeDamage(damage);

            OnHit();
        }
        else
            return;
    }
    #endregion

    #region Custom methods
    void Clean()
    {
        if (transform.position.y >= screenBounds.y + 20.0f || transform.position.y <= screenBounds.y - 20.0f)
            Destroy(gameObject);
    }

    void OnHit()
    {
        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion

    #region Nested classes
    [System.Serializable]
    class Tags
    {
        [SerializeField] internal string enemyProjectile = "EnemyProjectile";
        [SerializeField] internal string enemyTag = "Enemy";
        [SerializeField] internal string playerProjectile = "PlayerProjectile";
        [SerializeField] internal string playerTag = "Player";
    }
    #endregion
}