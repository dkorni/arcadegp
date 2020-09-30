using Assets.Scripts.Controllers.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitPopupController : MonoBehaviour
{
    [SerializeField] private Text _remainingSecondsText;
    [SerializeField] private float _remainingSeconds;
    [SerializeField] private string _nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(Timer.WaitForAndRun(_remainingSeconds, Tick));
    }

    /// <summary>
    /// Load scene if time is elapsed.
    /// </summary>
    private void Tick()
    {
        _remainingSeconds--;
        _remainingSecondsText.text = _remainingSeconds.ToString();

        if (_remainingSeconds == 0)
        {
            SceneManager.LoadScene(_nextSceneName);
            return;
        }

        StartCoroutine(Timer.WaitForAndRun(_remainingSeconds, Tick));
    }
}
