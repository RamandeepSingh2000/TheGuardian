using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialMenuScript : MonoBehaviour
{
    private MenuInteractionActions _inputActions;
    [SerializeField] private List<RawImage> _explanations;
    [SerializeField] private float _timeToDis = 10f;
    private float _totalTime;


    void Awake()
    {
        _totalTime = _timeToDis;
        _inputActions = new MenuInteractionActions();
    }

    void OnEnable() {
        _inputActions.Player.Menu.performed += Disappear;
        _inputActions.Enable();
    }

    private void Disappear(InputAction.CallbackContext context)
    {
        _timeToDis = 2f;
    }

    void OnDisable() {
        _inputActions.Player.Menu.performed -= Disappear;
        _inputActions.Enable();
    }

    void Start()
    {

    }

    void Update()
    {
        if (_timeToDis > 0)
        {
            _timeToDis -= Time.deltaTime;

            foreach (RawImage image in _explanations)
            {
                Color tempColor = image.color;
                tempColor.a = _timeToDis / _totalTime;
                image.color = tempColor;
            }
        }
    }
}
