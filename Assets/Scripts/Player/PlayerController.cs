using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>, IDamagable
{
    public bool IsMoving => movement != Vector2.zero;
    public bool CanTakeDamage => canTakeDamage;

    [SerializeField] float movementSpeed = 5f;

    // Dash parameters
    [SerializeField] float dashThrust = 7;
    [SerializeField] float dashTime = 0.3f;
    [SerializeField] float dashCooldown = 0.5f;

    PlayerControls controls;
    PlayerCombat playerCombat;
    SpecialAttackHandler specialAttackHandler;

    Vector2 movement;
    Rigidbody2D rb;
    Bounds bounds;
    Shield playerShield;

    GameObject currentSpecialAttack = null;

    bool isDashing = false;
    bool canDash = true;
    bool canTakeDamage = true;



    protected override void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        playerCombat = GetComponent<PlayerCombat>();
        playerShield = GetComponent<Shield>();
        specialAttackHandler = GetComponent<SpecialAttackHandler>();
    }

    void Start()
    {
        bounds = GameManager.Instance.GetCameraBounds();
    }

    void OnEnable()
    {
        controls.Enable();
        EnemyController.OnEnemyDeath += IncreaseSpecialAttackBar;
    }

    void OnDisable()
    {
        controls.Disable();
        EnemyController.OnEnemyDeath -= IncreaseSpecialAttackBar;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) { return; }

        PlayerInput();
        RotatePlayer();
        Fire();
        HandleDash();
        HandleSpecialAttackInput();
        // FOR TESTING
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddBulletSpread();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    void PlayerInput()
    {
        movement = controls.Player.Move.ReadValue<Vector2>();
    }

    void Move()
    {
        Vector2 newPosition = rb.position + movement * (movementSpeed * Time.fixedDeltaTime);

        newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
        newPosition.y = Mathf.Clamp(newPosition.y, bounds.min.y, bounds.max.y);

        rb.MovePosition(newPosition);
    }

    void RotatePlayer()
    {
        float angleOffset = -90f;
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPosition.z = 0;

        Vector3 direction = mouseWorldPosition - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }

    void Fire()
    {
        if (controls.Player.Fire.IsPressed())
        {
            playerCombat.ProcessFire();
        }
    }

    void HandleSpecialAttackInput()
    {
        if (controls.Player.SpecialAttack.IsPressed() && playerCombat.CanFireSpecialAttack)
        {
            if (currentSpecialAttack == null)
            {
                currentSpecialAttack = specialAttackHandler.FireRandomSpecialAttack();
                playerCombat.ResetSpecialAttackBar();
            }
        }
    }

    // TODO - maybe create a combined scripts that will hold both knockback and the dashing
    void HandleDash()
    {
        if (controls.Player.Dash.WasPressedThisFrame() && !isDashing && canDash)
        {
            isDashing = true;
            canDash = false;
            canTakeDamage = false;
            rb.velocity = movement * dashThrust;
            Vector2 dashDirection = Vector2.one * movement;
            rb.AddForce(dashDirection, ForceMode2D.Impulse);
            OnDashEnd();
        }
    }

    void OnDashEnd()
    {
        StartCoroutine(ResetTakeDamageRoutine());
        StartCoroutine(DashCooldownRoutine());
        StartCoroutine(DashEndRoutine());
    }

    IEnumerator ResetTakeDamageRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        canTakeDamage = true;
    }
    IEnumerator DashEndRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector2.zero;
        isDashing = false;
    }

    public void TakeDamage(string damageDealerTag, int damage)
    {
        playerCombat.TakeDamage(damageDealerTag, damage, canTakeDamage);
    }

    public void AddBulletSpread()
    {
        playerCombat.AddBulletSpread();
    }

    public void ActivateShield()
    {
        playerShield.ActivateShield();
    }

    public void AddHealth()
    {
        playerCombat.AddHealth();
    }

    public void IncreaseSpecialAttackBar()
    {
        playerCombat.IncreaseSpecialAttackBar();
    }


}
