using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    [Component("PC Input")]
    public class CarPCInput : MonoBehaviour
    {
#if !UNITY_ANDROID

        private bool _isPause = false;

        private void Awake()
        { _isPause = false; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isPause = !_isPause;
                GlobalEventManager.Instance.PauseButton(_isPause);
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                GlobalEventManager.Instance.GasButton(true);
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                GlobalEventManager.Instance.GasButton(false);

            if (Input.GetKeyDown(KeyCode.Space))
                GlobalEventManager.Instance.BrakeButton(true);
            if (Input.GetKeyUp(KeyCode.Space))
                GlobalEventManager.Instance.BrakeButton(false);

            if (Input.GetKeyDown(KeyCode.R))
                GlobalEventManager.Instance.GearButton(true);
            if (Input.GetKeyUp(KeyCode.R))
                GlobalEventManager.Instance.GearButton(false);

            if (Input.GetKeyDown(KeyCode.L))
                GlobalEventManager.Instance.LightButton(true);
            if (Input.GetKeyUp(KeyCode.L))
                GlobalEventManager.Instance.LightButton(false);
        }
#endif
    }
}
