using UnityEngine;
using System.Collections;

public class NightCar : MonoBehaviour
{
    private IEnumerator Start()
    {
        GlobalEventManager.Instance.LightButton(true);
        yield return new WaitForEndOfFrame();
        GlobalEventManager.Instance.LightButton(true);
        yield return new WaitForEndOfFrame();
        GlobalEventManager.Instance.LightButton(false);
        Destroy(this.gameObject);
    }
}
