using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using static UnityEngine.RectTransformUtility;

class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Script paremeters
    [SerializeField] private string horizontalAxis = "Horizontal", verticalAxis = "Vertical";

    private const byte two = 2;
    private Image joystick, joystickController;
    private Vector2 joystickBias, position;
    private JoystickController instance = null;
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        joystick = GetComponent<Image>();
        joystickController = transform.GetChild(0).GetComponent<Image>();
    }
    #endregion

    #region Custom methods
    public virtual void OnDrag(PointerEventData pointerEventData)
    {
        if (ScreenPointToLocalPointInRectangle(joystick.rectTransform, pointerEventData.position, pointerEventData.pressEventCamera, out position))
        {
            position.x /= joystick.rectTransform.sizeDelta.x;
            position.y /= joystick.rectTransform.sizeDelta.y;

            joystickBias = new Vector2(position.x * two, position.y * two);
            joystickBias = (joystickBias.magnitude > 1.0f) ? joystickBias.normalized : joystickBias;

            joystickController.rectTransform.anchoredPosition = new Vector2(
                joystickBias.x * (joystick.rectTransform.sizeDelta.x / two),
                joystickBias.y * (joystick.rectTransform.sizeDelta.y / two));
        }
    }

    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        OnDrag(pointerEventData);
    }

    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
        joystickBias = Vector2.zero;
        joystickController.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        if (joystickBias.x != 0) return joystickBias.x;
        else return Input.GetAxisRaw(horizontalAxis);
    }

    public float Vertical()
    {
        if (joystickBias.y != 0) return joystickBias.y;
        else return Input.GetAxisRaw(verticalAxis);
    }
    #endregion
}