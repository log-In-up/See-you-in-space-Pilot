using UnityEngine.UI;
using UnityEngine;

class UIHealthBar : MonoBehaviour
{
    #region Script paremeters
#pragma warning disable 649
    [Header("Object references")]
    [SerializeField] GameObject player;
    [SerializeField] RectTransform healthBar;
    [SerializeField] Text value;
#pragma warning restore 649

    PlayerController playerController;

    float calculatedWidth, widthMultiplier;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        widthMultiplier = healthBar.sizeDelta.x / playerController.MaxHealth;
    }
    #endregion

    #region Custom methods
    internal void UpdateHealth(float health)
    {
        calculatedWidth = health * widthMultiplier;
        healthBar.sizeDelta = new Vector2(calculatedWidth, healthBar.sizeDelta.y);
        value.text = string.Format("{0} / {1}", health.ToString(), playerController.MaxHealth.ToString());
    }
    #endregion
}