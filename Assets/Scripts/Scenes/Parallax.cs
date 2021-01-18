using UnityEngine;

class Parallax : MonoBehaviour
{
    #region Script paremeters
#pragma warning disable 649
    [SerializeField] PlayerController playerMovement;
#pragma warning restore 649

    float length;

    Transform lastChildObject = null;
    Vector2 parallaxVector;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        parallaxVector = transform.position;
        lastChildObject = transform.GetChild(transform.childCount - 1);
    }

    void Start()
    {
        length = transform.position.y - lastChildObject.transform.position.y;
    }

    void Update()
    {
        float position = Mathf.Repeat(Time.time * playerMovement.MovementSpeed, length);
        transform.position = parallaxVector + Vector2.down * position;
    }
    #endregion
}