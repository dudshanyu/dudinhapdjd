using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroler : MonoBehaviour
{
    public int coin = 0;
    public TMP_Text coinText;
    public float movespeed;
    public float maxVelocity;
    public float rayDistance;
    public LayerMask groundLayer;
    public float jumpForce;
    
    private Gamecontrol _gamecontrol;
    private PlayerInput _playerinput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;
    private Vector2 _movement;

    private Vector2 _moveInput;
    private bool _isGrounded;

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

        if (obj.action.name.CompareTo(_gamecontrol.Gameplay.Jump.name) == 0)
        {
            if (obj.performed) Jump();
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
        LimitVelocity();
    }

    private void LimitVelocity()
    {
        Vector3 velocity = _rigidbody.velocity;
        if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = maxVelocity;
        velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);
        _rigidbody.velocity = velocity;
    }

    private void Jump()
    {
        if (_isGrounded) _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(origin: transform.position, direction: Vector3.down, rayDistance,
            groundLayer);
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(start:transform.position, dir:Vector3.down * rayDistance, Color.yellow);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
            
        {
            coin++;
            coinText.text = coin.ToString();
            Destroy(other.gameObject);
           
            
        }
    }
    
}
    

