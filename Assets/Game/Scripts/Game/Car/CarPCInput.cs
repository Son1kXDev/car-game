using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Game
{
    public class CarPCInput : MonoBehaviour
    {
        [SerializeField] private bool _isEnabled;
        [SerializeField] private Button _menuButton;
        [SerializeField] private GameObject _gasButton;
        [SerializeField] private GameObject _brakeButton;
        [SerializeField] private GameObject _lightButton;
        [SerializeField] private GameObject _switchGearButton;

        private void Awake()
        {
            if (_isEnabled == false) Destroy(this);
        }

        private void Update()
        {
            //menu button
            if (Input.GetKeyDown(KeyCode.Escape)) _menuButton.onClick?.Invoke();

            //gas button
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                ExecuteEvents.Execute<IPointerDownHandler>(_gasButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                ExecuteEvents.Execute<IPointerUpHandler>(_gasButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);

            //brake button
            if (Input.GetKeyDown(KeyCode.Space))
                ExecuteEvents.Execute<IPointerDownHandler>(_brakeButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            if (Input.GetKeyUp(KeyCode.Space))
                ExecuteEvents.Execute<IPointerUpHandler>(_brakeButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);

            //gear switch button
            if (Input.GetKeyDown(KeyCode.R))
                ExecuteEvents.Execute<IPointerDownHandler>(_switchGearButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            if (Input.GetKeyUp(KeyCode.R))
                ExecuteEvents.Execute<IPointerUpHandler>(_switchGearButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);

            //light switch button
            if (Input.GetKeyDown(KeyCode.L))
                ExecuteEvents.Execute<IPointerDownHandler>(_lightButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            if (Input.GetKeyUp(KeyCode.L))
                ExecuteEvents.Execute<IPointerUpHandler>(_lightButton,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }

    }
}
