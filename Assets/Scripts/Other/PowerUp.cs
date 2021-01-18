using UnityEngine;

class PowerUp : MonoBehaviour
{
    #region Script paremeters
    [Header("PowerUp characteristics")]
    [SerializeField] float healthRecovery = 20.0f;
    [SerializeField] float powerUpSpeed = 5.0f;
    [SerializeField] float scoreValue = 5.0f;
    [SerializeField] string playerTag = "Player";

    GameMaster gameMaster;
    Rigidbody2D rigidbody2d;
    Transform powerUp;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        powerUp = GetComponent<Transform>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        PowerUpSpeed();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(playerTag))
        {
            gameMaster.UpdateScore(scoreValue);
            PickUp(collision);
        }
    }
    #endregion

    #region Custom methods
    void PickUp(Collider2D player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.TakePowerUp(healthRecovery);
        Destroy(gameObject);
    }

    void PowerUpSpeed()
    {
        rigidbody2d.AddForce(powerUp.up * (-powerUpSpeed), ForceMode2D.Impulse);
    }
    #endregion
}