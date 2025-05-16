using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContol : MonoBehaviour
{
    //Movimiento
    private Rigidbody2D _rigidBody;
    public float inputHorizontal;
    public float playerSpeed = 4.5f;
    
    public BoxCollider2D _boxCollider;
    //Salto
    private GroundSensor _groundSensor;
    public float jumpForce = 10;
    
    //Dash
    [SerializeField] private float _dashForce = 20;
    [SerializeField] private float _dashDuration = 2f;
    [SerializeField] private float _dashCoolDown = 1;
    private bool _canDash = true;
    private bool _isDahsing = false;

    //Animaciones
    private Animator _animator;
    //Sonidos

    private float _currentHealth;
    private float _maxHealth = 1;
    [SerializeField] private Image _healthBar;
    [SerializeField] private AudioClip _damage;
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar.fillAmount = _maxHealth;

    }
    
    void Update()
    {
        //DASH
        if(Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }

        if(_isDahsing)
        {
            return;
        }

        //MOVEMENT
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if(inputHorizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsWalking", true);
        }
        else if(inputHorizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        //JUMP
        if(Input.GetButtonDown("Jump"))
        {
            if(_groundSensor.isGrounded || _groundSensor.canDoubleJump)
            {
                Jump();  
            }

        }

        _animator.SetBool("IsJumping", !_groundSensor.isGrounded);
    }

    void FixedUpdate() 
    {
         //DASH 
        if(_isDahsing)
        {
            return;
        }
        
        _rigidBody.velocity = new Vector2(inputHorizontal * playerSpeed, _rigidBody.velocity.y);    
    }

    void Jump()
    {

        if(!_groundSensor.isGrounded)
        {
            _groundSensor.canDoubleJump = false;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
        }

        _rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    IEnumerator Dash()
    {
        _animator.SetTrigger("IsDashing");
        Debug.Log("Dash Iniciado");
        float gravity = _rigidBody.gravityScale;
        _rigidBody.gravityScale = 0;
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
        _isDahsing = true;
        _canDash = false;
        _rigidBody.AddForce(transform.right * _dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_dashDuration);
        _rigidBody.gravityScale = gravity;
        _isDahsing = false;
        yield return new WaitForSeconds(_dashCoolDown);
        _canDash = true;
        Debug.Log("Dash Finalizado");
    }

    public void TakeDamage(float _damage)
    {
        _healthBar.fillAmount = _currentHealth -= _damage;

        if(_currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        _spriteRenderer.enabled = false;
        _boxCollider.enabled = false;
        _rigidBody.gravityScale = 0;
        Destroy(_groundSensor.gameObject);
        inputHorizontal = 0;
        Destroy(gameObject);
    }

}
