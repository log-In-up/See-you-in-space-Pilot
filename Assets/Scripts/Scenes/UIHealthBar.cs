using UnityEngine.UI;
using UnityEngine;

class UIHealthBar : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Text value;

    private PlayerController playerController;
    private float calculatedWidth, widthMultiplier;
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        widthMultiplier = healthBar.sizeDelta.x / playerController.MaxHealth;
    }
    #endregion

    #region Custom methods
    public void UpdateHealth(float health)
    {
        calculatedWidth = health * widthMultiplier;
        healthBar.sizeDelta = new Vector2(calculatedWidth, healthBar.sizeDelta.y);
        value.text = $"{health} / {playerController.MaxHealth}";
    }
    #endregion
}