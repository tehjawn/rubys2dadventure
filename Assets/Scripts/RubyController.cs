using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
  Rigidbody2D rigidbody2d;
  float horizontal;
  float vertical;

  int currentHealth;
  public int maxHealth = 5;
  public int health { get { return currentHealth; } }

  public float timeInvincible = 2.0f;
  bool isInvincible;
  float invincibleTimer;
  private Color normalPlayerLightColor = new Color(1f, .76f, .53f);
  private Color damagedPlayerLightColor = new Color(1f, .4f, .3f);

  public float speed = 5.0f;

  public UnityEngine.Experimental.Rendering.Universal.Light2D playerLight;

  Animator animator;
  Vector2 lookDirection = new Vector2(1, 0);

  public GameObject projectilePrefab;

  // Start is called before the first frame update
  void Start()
  {
    rigidbody2d = GetComponent<Rigidbody2D>();
    GameObject mPlayerLight = GameObject.FindWithTag("PlayerLight");
    playerLight = mPlayerLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    currentHealth = 1;
    ChangeHealth(4);
  }

  void Awake()
  {
    GameObject playerSprite = GameObject.FindWithTag("PlayerSprite");
    if (playerSprite != null)
      animator = playerSprite.GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    horizontal = Input.GetAxis("Horizontal");
    vertical = Input.GetAxis("Vertical");

    Vector2 move = new Vector2(horizontal, vertical);

    if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
    {
      lookDirection.Set(move.x, move.y);
      lookDirection.Normalize();
    }

    if (animator != null)
    {
      animator.SetFloat("Look X", lookDirection.x);
      animator.SetFloat("Look Y", lookDirection.y);
      animator.SetFloat("Speed", move.magnitude);
    }

    if (isInvincible)
    {
      invincibleTimer -= Time.deltaTime;

      if (invincibleTimer < 0)
      {
        playerLight.color = normalPlayerLightColor;
        isInvincible = false;
      }
      else
      {
        if (playerLight != null)
        {
          playerLight.color = damagedPlayerLightColor;
        }
      }
    }

    if (Input.GetKeyDown(KeyCode.C))
    {
      LaunchCog();
    }

    if (Input.GetKeyDown(KeyCode.X))
    {
      RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
      if (hit.collider != null)
      {
        FrogController froggy = hit.collider.GetComponent<FrogController>();
        if (froggy != null)
        {
          froggy.DisplayDialog();
        }
      }
    }
  }

  void FixedUpdate()
  {

    Vector2 position = rigidbody2d.position;

    position.x = position.x + speed * horizontal * Time.deltaTime;
    position.y = position.y + speed * vertical * Time.deltaTime;

    rigidbody2d.MovePosition(position);
  }

  public void ChangeHealth(int amount)
  {
    if (amount < 0)
    {
      if (isInvincible)
        return;

      isInvincible = true;
      invincibleTimer = timeInvincible;
    }

    currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    UpdatePlayerLightStrength(currentHealth);

    UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
  }

  private void UpdatePlayerLightStrength(int currentHealth)
  {
    playerLight.intensity = .95f + (currentHealth * .05f);
    playerLight.pointLightInnerRadius = (currentHealth * .1f);
    if (currentHealth == 5)
    {
      playerLight.pointLightOuterRadius = 5;
    }
    else
    {
      playerLight.pointLightOuterRadius = 1.25f + (currentHealth * .25f);
    }
  }

  private void LaunchCog()
  {
    GameObject cogProjectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    CogBullet cogProjectile = cogProjectileObject.GetComponent<CogBullet>();
    if (cogProjectile != null)
    {
      cogProjectile.Launch(lookDirection, 300);
      animator.SetTrigger("Launch");
    }
  }
}
