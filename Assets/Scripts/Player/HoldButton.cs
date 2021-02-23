using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Script paremeters
    [SerializeField] private Color pressedColor = Color.gray;
    [SerializeField] private UnityEvent onHold;

    private Color standardColor;
    private Image image;
    private bool buttonDown;
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        image = GetComponent<Image>();        
    }
    private void Start()
    {
        standardColor = image.color;
    }
    private void Update()
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
