using UnityEngine;
using static UnityEngine.Mathf;

class Parallax : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private string playerTag = "Player";

    private PlayerController playerController;
    private Transform lastChildObject = null;
    private Vector2 parallaxVector;
    private float length;
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerController>();
        parallaxVector = transform.position;
        lastChildObject = transform.GetChild(transform.childCount - 1);
    }

    private void Start()
    {
        length = transform.position.y - lastChildObject.transform.position.y;
    }

    private void Update()
    {
        float position = Repeat(Time.time * playerController.MovementSpeed, length);
        transform.position = parallaxVector + Vector2.down * position;
    }
    #endregion
}