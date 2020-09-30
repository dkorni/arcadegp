using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarController : MonoBehaviour, IUiController
{
    private Slider _healthBar;

    // Start is called before the first frame update
    public void StartUI()
    {
        _healthBar = GetComponent<Slider>();
        var player = GameManager.Instance.PlayerController;
        _healthBar.maxValue = player.Health;
        _healthBar.value = player.Health;

        // subscribe to OnHealthChanged event
        player.OnHealthChanged.AddListener(UpdateUi);
    }

    /// <summary>
    /// Updates the UI.
    /// </summary>
    /// <param name="value">The value.</param>
    private void UpdateUi(object value)
    {
        _healthBar.value = (float) value;
    }
}
