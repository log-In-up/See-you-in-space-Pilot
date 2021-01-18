using UnityEngine;

class PlayerController : MonoBehaviour
{
    #region Script paremeters
    [Header("Player characteristics")]
    [SerializeField] float fireRate = 0.25f;
    [SerializeField] float health = 100.0f;
    [SerializeField] float laserSpeed = 15.0f;
    [SerializeField] float movementSpeed = 10.0f;
#pragma warning disable 649
    [Header("Object references")]
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject shortLaser, explosionVFX, flashVFX;
    [SerializeField] JoystickController joystickController;
    [SerializeField] Transform gunBarrel, barrelVFX;
    [SerializeField] UIHealthBar uIHealthBar;
    [SerializeField] InputProperty inputProperty;
#pragma warning disable 649

    float maxHealth, playerHeight, playerWidth, shotDelay;

    GameObject laser;
    Rigidbody2D rigidbody2d, rigidbody2dLaser;
    Vector2 movement, screenBounds;

    internal float MovementSpeed { get { return movementSpeed; } }
    internal float MaxHealth { get { return maxHealth; } }
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        maxHealth = health;

        rigidbody2d = GetComponent<Rigidbody2D>();
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Start()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            mainCamera.transform.position.z));

        uIHealthBar.UpdateHealth(MaxHealth);
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        PlayerInput();
    }

    void LateUpdate()
    {
        Vector2 viewPosition = transform.position;
        viewPosition.x = Mathf.Clamp(viewPosition.x, screenBounds.x + playerWidth, (screenBounds.x * -1) - playerWidth);
        viewPosition.y = Mathf.Clamp(viewPosition.y, screenBounds.y + playerHeight, (screenBounds.y * -1) - playerHeight);
        transform.position = viewPosition;
    }
    #endregion

    #region Custom methods
    void PlayerInput()
    {
        movement.y = joystickController.Vertical(); // Input for Y axis. 
        movement.x = joystickController.Horizontal(); // Input for X axis.

        if (inputProperty.Fire && Time.time > shotDelay)
        {
            shotDelay = Time.time + fireRate;
            Shot();
        }
    }

    void Movement()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + (movement * movementSpeed) * Time.fixedDeltaTime);
    }

    void Shot()
    {
        laser = Instantiate(shortLaser, gunBarrel.position, Quaternion.identity);
        rigidbody2dLaser = laser.GetComponent<Rigidbody2D>();
        rigidbody2dLaser.AddForce(gunBarrel.up * laserSpeed, ForceMode2D.Impulse);
        Instantiate(flashVFX, barrelVFX.position, Quaternion.identity);
    }

    internal void TakeDamage(float takingDamage)
    {
        health -= takingDamage;

        if (health <= 0.0f)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        uIHealthBar.UpdateHealth(health);
    }

    internal void TakePowerUp(float takingHealth)
    {
        health += takingHealth;

        if (health >= MaxHealth)
            health = MaxHealth;

        uIHealthBar.UpdateHealth(health);
    }

    public void ButtonShot()
    {
        if (Time.time > shotDelay)
        {
            shotDelay = Time.time + fireRate;
            Shot();
        }
    }
    #endregion

    #region Nested classes
    [System.Serializable]
    class InputProperty
    {
        [SerializeField] string fire = "Fire1";
        internal bool Fire { get { return Input.GetButtonDown(fire); } }
    }
    #endregion
}