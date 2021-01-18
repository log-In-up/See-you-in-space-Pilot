using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Script paremeters
    [Header("HoldButton characteristics")]
    [SerializeField] Color pressedColor = Color.gray;
#pragma warning disable 649
    [SerializeField] UnityEvent onHold;
#pragma warning restore 649

    Color standardColor;
    Image image;
    bool buttonDown;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        image = GetComponent<Image>();        
    }

    void Start()
    {
        standardColor = image.color;
    }

    void Update()
    {
        if (buttonDown && onHold != null)
            onHold.Invoke();
    }
    #endregion

    #region Custom methods
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        buttonDown = true;
        image.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        buttonDown = false;
        image.color = standardColor;
    }
    #endregion
}
