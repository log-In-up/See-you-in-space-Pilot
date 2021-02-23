using UnityEngine;

class EnergyProjectile : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject destructionVFX;
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private string enemyProjectile = "EnemyProjectile", enemyTag = "Enemy", playerProjectile = "PlayerProjectile", playerTag = "Player";

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player;
        EnemyAI enemy;

        if (collision.CompareTag(playerTag) && !transform.CompareTag(playerProjectile))
        {
            player = collision.GetComponent<PlayerController>();
            player.TakeDamage(damage);
            InstanitateVFX();
        }
        else if (collision.CompareTag(enemyTag) && !transform.CompareTag(enemyProjectile))
        {
            enemy = collision.GetComponent<EnemyAI>();
            enemy.TakeDamage(damage);
            InstanitateVFX();
        }
        else
            return;
    }
    #endregion

    #region Custom methods
    private void Clean()
    {
        if (transform.position.y >= screenBounds.y + 20.0f || transform.position.y <= screenBounds.y - 20.0f)
            Destroy(gameObject);
    }

    private void InstanitateVFX()
    {
        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion
}