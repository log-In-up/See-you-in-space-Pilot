using UnityEngine;

class PowerUp : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private float healthRecovery = 20.0f, powerUpSpeed = 5.0f, scoreValue = 5.0f;
    [SerializeField] private string playerTag = "Player";

    private GameMaster gameMaster;
    private Rigidbody2D rigidbody2d;
    private Transform powerUp;

    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        powerUp = GetComponent<Transform>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PowerUpSpeed();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(playerTag))
        {
            gameMaster.UpdateScore(scoreValue);
            PickUp(collision);
        }
    }
    #endregion

    #region Custom methods
    private void PickUp(Collider2D player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.TakePowerUp(healthRecovery);
        Destroy(gameObject);
    }

    private void PowerUpSpeed()
    {
        rigidbody2d.AddForce(-powerUp.up * powerUpSpeed, ForceMode2D.Impulse);
    }
    #endregion
}