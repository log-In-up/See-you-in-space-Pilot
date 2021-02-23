using Pathfinding;
using UnityEngine;
using static UnityEngine.Mathf;

class EnemyAI : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject energyBall, explosionVFX, flashVFX, powerUp;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Transform flashShot, gunBarrel;
    [SerializeField] private float ballVelocity = 10.0f, distanceToTarget = 5.0f, fireRate = 0.5f, 
        health = 100.0f, movementSpeed = 10.0f, scoreValue = 15.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float dropChance = 0.33f;
    [SerializeField] private string playerTag = "Player";

    private GameMaster gameMaster;
    private GameObject ball, target;
    private Path path;
    private Rigidbody2D rigidbody2d, rigidbody2dBall;
    private Seeker seeker;
    private Vector2 screenBounds;

#pragma warning disable 414
    private bool reachedEndOfPath = false;
#pragma warning restore 414
    private float distanceBetweenTargets, maxHealth, nextShot, widthMultiplier;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = 3.0f;
    private const float zero = 0.0f;
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    private void Start()
    {
        maxHealth = health;
        widthMultiplier = healthBar.sizeDelta.x / maxHealth;

        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            mainCamera.transform.position.z));
        target = GameObject.FindGameObjectWithTag(playerTag);

        InvokeRepeating(nameof(UpdatePath), zero, 0.5f);
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        Movement();
        Rotation(target.transform.position);
    }

    private void Update()
    {
        if (ObjectInCamera(transform.position) && Time.time > nextShot && target != null)
        {
            nextShot = Time.time + fireRate;
            Shooting();
        }
    }
    #endregion

    #region Custom methods
    private void Movement()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 moveDirection = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody2d.position).normalized;
        Vector2 force = moveDirection * movementSpeed * Time.deltaTime;

        rigidbody2d.AddForce(force);

        float distance = Vector2.Distance(rigidbody2d.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Rotation(Vector2 target)
    {
        float rotationOffset = 90.0f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Atan2(direction.y, direction.x) * Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + rotationOffset));
    }

    private void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rigidbody2d.position, target.transform.position + (Vector3.up * distanceToTarget),
                OnPathComplete);
        }
    }

    private void Shooting()
    {
        ball = Instantiate(energyBall, gunBarrel.position, Quaternion.identity);
        rigidbody2dBall = ball.GetComponent<Rigidbody2D>();
        rigidbody2dBall.AddForce(-(gunBarrel.up * ballVelocity), ForceMode2D.Impulse);
        Instantiate(flashVFX, flashShot.position, Quaternion.identity);
    }

    private bool ObjectInCamera(Vector2 position)
    {
        if (position.x <= Abs(screenBounds.x) && position.y <= Abs(screenBounds.y))
            return true;
        else
            return false;
    }

    public void TakeDamage(float takingDamage)
    {
        health -= takingDamage;
        float calculatedWidth = health * widthMultiplier;
        healthBar.sizeDelta = new Vector2(calculatedWidth, healthBar.sizeDelta.y);

        if (health <= zero)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            PowerUpDrop();
            gameMaster.UpdateScore(scoreValue);
            Destroy(gameObject);
        }
    }

    private void PowerUpDrop()
    {
        if (Random.value <= dropChance)
            Instantiate(powerUp, transform.position, Quaternion.identity);
    }
    #endregion
}