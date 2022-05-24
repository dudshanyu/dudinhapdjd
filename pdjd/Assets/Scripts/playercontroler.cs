using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroler : MonoBehaviour
{
    public float movespeed;
    
    private Gamecontrol _gamecontrol;
    private PlayerInput _playerinput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private Vector2 _movement;

    private void OnEnable()
    {
        //inicializacao de variavel
        _gamecontrol = new Gamecontrol();
        
        //referencias dos componentes no mesmo objeto na unity
        _playerinput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        
        //referencia para a camera main guardada na classe camera
        _mainCamera = Camera.main;
        
        //atribuindo ao delegate do action triggered no player input
        _playerinput.onActionTriggered += OnActionTriggered;

    }

    private void OnDisable()
    {
        _playerinput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        //comecando o nome do action que este chegando com o nome do action de moviment
        if(obj.action.name.CompareTo(_gamecontrol.Gameplay.Movement.name) == 0)
        {
            //atribuir ao moveinput o valor proveniente ao input do jogador com o Vector2
            _movement = obj.ReadValue<Vector2>();
        }
        
    }

    private void Move()
    {
        //calcule o movimento no eixo da camera para o movimento frente/tras
        Vector3 moveVertical = _mainCamera.transform.forward * _movement.y;
        
        //calcula o movimento no eixo da camera para o movimento esquerda/direita
        Vector3 moveHorizontal = _mainCamera.transform.right * _movement.x;
        
        // adiciona a forca no objeto atraves do rigidbody, com intensidade definida por moveSpeed
        _rigidbody.AddForce((moveVertical + moveHorizontal) * movespeed * Time.fixedDeltaTime);

    }

    private void FixedUpdate()
    {
        Move();
    }
}
