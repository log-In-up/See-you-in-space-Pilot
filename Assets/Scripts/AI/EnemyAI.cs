using Pathfinding;
using UnityEngine;

class EnemyAI : MonoBehaviour
{
    #region Script paremeters
    [Header("AI characteristics")]
    [SerializeField] float projectileVelocity = 10.0f;
    [SerializeField] float distanceToTarget = 5.0f;
    [SerializeField, Range(0.0f, 1.0f)] float dropChance = 0.33f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float health = 100.0f;
    [SerializeField] float movementSpeed = 10.0f;
    [SerializeField] float scoreValue = 15.0f;
    [SerializeField] string playerTag = "Player";
#pragma warning disable 649
    [Header("Object references")]
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject energyBall, explosionVFX, flashVFX, powerUp;
    [SerializeField] RectTransform healthBar;
    [SerializeField] Transform flashShot, gunBarrel;
#pragma warning restore 649

    GameMaster gameMaster;
    GameObject ball, target;
    Path path;
    Rigidbody2D rigidbody2d, rigidbody2dBall;
    Seeker seeker;
    Vector2 screenBounds;

#pragma warning disable 414
    bool reachedEndOfPath = false;
#pragma warning restore 414
    float maxHealth, nextShot, widthMultiplier;
    readonly float nextWaypointDistance = 3.0f;
    int currentWaypoint = 0;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    void Start()
    {
        maxHealth = health;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            mainCamera.transform.position.z));
        target = GameObject.FindGameObjectWithTag(playerTag);

        widthMultiplier = healthBar.sizeDelta.x / maxHealth;

        InvokeRepeating(nameof(UpdatePath), 0.0f, 0.5f);
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        Movement();
        Rotation(target.transform.position);
    }

    void Update()
    {
        if (ObjectInCamera(transform.position) && Time.time > nextShot && target != null)
        {
            nextShot = Time.time + fireRate;
            Shooting();
        }
    }

    void OnDestroy()
    {
        gameMaster.UpdateScore(scoreValue);
    }
    #endregion

    #region Custom methods
    bool ObjectInCamera(Vector2 position)
    {
        if (position.x <= Mathf.Abs(screenBounds.x) && position.y <= Mathf.Abs(screenBounds.y))
            return true;
        else
            return false;
    }

    void Movement()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else reachedEndOfPath = false;

        Vector2 moveDirection = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody2d.position).normalized;
        Vector2 force = moveDirection * movementSpeed * Time.deltaTime;

        rigidbody2d.AddForce(force);

        float distance = Vector2.Distance(rigidbody2d.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Rotation(Vector2 target)
    {
        float rotationOffset = 90.0f;
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + rotationOffset));
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rigidbody2d.position, target.transform.position + (Vector3.up * distanceToTarget),
                OnPathComplete);
    }

    void Shooting()
    {
        ball = Instantiate(energyBall, gunBarrel.position, Quaternion.identity);
        rigidbody2dBall = ball.GetComponent<Rigidbody2D>();
        rigidbody2dBall.AddForce(-(gunBarrel.up * projectileVelocity), ForceMode2D.Impulse);
        Instantiate(flashVFX, flashShot.position, Quaternion.identity);
    }

    internal void TakeDamage(float takingDamage)
    {
        health -= takingDamage;
        float calculatedWidth = health * widthMultiplier;
        healthBar.sizeDelta = new Vector2(calculatedWidth, healthBar.sizeDelta.y);

        if (health <= 0.0f)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            if (Random.value <= dropChance)
                Instantiate(powerUp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion
}