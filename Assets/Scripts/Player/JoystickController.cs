using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Script paremeters
#pragma warning disable 649
    [SerializeField] InputProrerty inputProrerty;
#pragma warning restore 649

    Image joystick, joystickController;
    Vector2 joystickBias;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        joystick = GetComponent<Image>();
        joystickController = transform.GetChild(0).GetComponent<Image>();
    }
    #endregion

    #region Custom methods
    public virtual void OnDrag(PointerEventData pointerEventData)
    {
#pragma warning disable 642
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystick.rectTransform,
            pointerEventData.position, pointerEventData.pressEventCamera, out Vector2 position));
#pragma warning restore 642
        {
            position.x /= joystick.rectTransform.sizeDelta.x;
            position.y /= joystick.rectTransform.sizeDelta.y;

            joystickBias = new Vector2(position.x * 2, position.y * 2);
            joystickBias = (joystickBias.magnitude > 1.0f) ? joystickBias.normalized : joystickBias;

            joystickController.rectTransform.anchoredPosition = new Vector2(
                joystickBias.x * (joystick.rectTransform.sizeDelta.x / 2),
                joystickBias.y * (joystick.rectTransform.sizeDelta.y / 2));
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

    internal float Horizontal()
    {
        if (joystickBias.x != 0) return joystickBias.x;
        else return inputProrerty.Horizontal;
    }

    internal float Vertical()
    {
        if (joystickBias.y != 0) return joystickBias.y;
        else return inputProrerty.Vertical;
    }
    #endregion

    #region Nested classes
    [System.Serializable]
    class InputProrerty
    {
        [SerializeField] string horizontal = "Horizontal";
        [SerializeField] string vertical = "Vertical";

        internal float Horizontal { get { return Input.GetAxisRaw(horizontal); } }
        internal float Vertical { get { return Input.GetAxisRaw(vertical); } }
    }
    #endregion
}