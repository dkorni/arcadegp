using Assets.Scripts.Controllers.Utils;
using UnityEngine;

public class WinPopupController : MonoBehaviour
{
    [SerializeField] private float _delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer.WaitForAndRun(_delay, ()=>transform.GetChild(0).gameObject.SetActive(true)));
    }
}
