using UnityEngine;
using static UnityEngine.Mathf;
using static UnityEngine.GameObject;

class PlayerController : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject shortLaser, explosionVFX, flashVFX;
    [SerializeField] private Transform gunBarrel, barrelVFX;
    [SerializeField] private float fireRate = 0.25f, health = 100.0f, laserSpeed = 15.0f, movementSpeed = 10.0f;
    [SerializeField] private string fire = "Fire1", joystickTag = "Joystick", healthBarTag = "HealthBar";

    private float playerHeight, playerWidth, shotDelay;
    private GameObject laser;
    private Rigidbody2D rigidbody2d, rigidbody2dLaser;
    private Vector2 movement, screenBounds;
    private JoystickController joystickController;
    private UIHealthBar uIHealthBar;

    public float MovementSpeed => movementSpeed;

    public float MaxHealth { get; private set; }
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        MaxHealth = health;

        joystickController = FindGameObjectWithTag(joystickTag).GetComponent<JoystickController>();
        uIHealthBar = FindGameObjectWithTag(healthBarTag).GetComponent<UIHealthBar>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    private void Start()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3
            (
            Screen.width,
            Screen.height,
            mainCamera.transform.position.z
            ));

        uIHealthBar.UpdateHealth(MaxHealth);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void LateUpdate()
    {
        Vector2 viewPosition = transform.position;
        viewPosition.x = Clamp(viewPosition.x, screenBounds.x + playerWidth, (screenBounds.x * -1) - playerWidth);
        viewPosition.y = Clamp(viewPosition.y, screenBounds.y + playerHeight, (screenBounds.y * -1) - playerHeight);
        transform.position = viewPosition;
    }
    #endregion

    #region Custom methods
    private void PlayerInput()
    {
        movement.y = joystickController.Vertical();
        movement.x = joystickController.Horizontal();

        if (Input.GetButtonDown(fire) && Time.time > shotDelay)
        {
            shotDelay = Time.time + fireRate;
            Shot();
        }
    }

    private void Movement()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + (movement * movementSpeed) * Time.fixedDeltaTime);
    }

    private void Shot()
    {
        laser = Instantiate(shortLaser, gunBarrel.position, Quaternion.identity);
        rigidbody2dLaser = laser.GetComponent<Rigidbody2D>();
        rigidbody2dLaser.AddForce(gunBarrel.up * laserSpeed, ForceMode2D.Impulse);
        Instantiate(flashVFX, barrelVFX.position, Quaternion.identity);
    }

    public void TakeDamage(float takingDamage)
    {
        health -= takingDamage;

        if (health <= 0.0f)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        uIHealthBar.UpdateHealth(health);
    }

    public void TakePowerUp(float takingHealth)
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
}