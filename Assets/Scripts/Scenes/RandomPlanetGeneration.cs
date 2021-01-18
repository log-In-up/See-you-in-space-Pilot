using System.Collections;
using UnityEngine;

class RandomPlanetGeneration : MonoBehaviour
{
    #region Script paremeters
#pragma warning disable 649
    [SerializeField] GameObject[] planets;
#pragma warning restore 649
    [SerializeField] bool createPlanets = true;
    [SerializeField] float createTime = 3.0f;
    [SerializeField] float distanceToScreenBoundaries = 10.0f;
    [SerializeField] float planetSpeedMultiplier = 1.15f;
    [SerializeField] string playerTag = "Player";

    Camera mainCamera;
    PlayerController playerMovement;
    Vector2 screenBounds;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerController>();       
        mainCamera = GetComponent<Camera>();
    }

    void Start()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        StartCoroutine(CreatePlanet());
    }
    #endregion

    #region Custom methods
    IEnumerator CreatePlanet()
    {
        while (createPlanets)
        {
            yield return new WaitForSeconds(createTime);
            Planet();
        }
    }

    void Planet()
    {
        Vector2 movementPlanet = new Vector2(Random.Range(-screenBounds.x - distanceToScreenBoundaries,
            screenBounds.x + distanceToScreenBoundaries), screenBounds.y + distanceToScreenBoundaries);

        int randomNumber = Random.Range(0, planets.Length);

        GameObject planet = Instantiate(planets[randomNumber], movementPlanet, Quaternion.identity);

        Rigidbody2D rigBodyPlanet = planet.GetComponent<Rigidbody2D>();
        float speed = playerMovement.MovementSpeed * planetSpeedMultiplier;
        rigBodyPlanet.AddForce(Vector2.down * speed, ForceMode2D.Impulse);
    }
    #endregion
}