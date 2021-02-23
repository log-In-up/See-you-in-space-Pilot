using System.Collections;
using UnityEngine;
using static UnityEngine.Random;

class RandomPlanetGeneration : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private GameObject[] planets;
    [SerializeField] private bool createPlanets = true;
    [SerializeField] private float createTime = 3.0f, planetSpeedMultiplier = 1.15f;
    [SerializeField] private string playerTag = "Player";

    private const byte zero = 0, ten = 10;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private PlayerController playerMovement;
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerController>();
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        StartCoroutine(CreatePlanet());
    }
    #endregion

    #region Custom methods
    private IEnumerator CreatePlanet()
    {
        while (createPlanets)
        {
            yield return new WaitForSeconds(createTime);
            Planet();
        }
    }

    private void Planet()
    {
        Vector2 movementPlanet = new Vector2(Range(-screenBounds.x - ten, screenBounds.x + ten),
            screenBounds.y + ten);
        int randomNumber = Range(zero, planets.Length);

        GameObject planet = Instantiate(planets[randomNumber], movementPlanet, Quaternion.identity);

        Rigidbody2D rigbodyPlanet = planet.GetComponent<Rigidbody2D>();
        float speed = playerMovement.MovementSpeed * planetSpeedMultiplier;
        rigbodyPlanet.AddForce(Vector2.down * speed, ForceMode2D.Impulse);
    }
    #endregion
}