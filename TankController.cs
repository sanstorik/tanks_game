using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class TankController : MonoBehaviour {

    public Transform shootPosition;
    public Transform canon;
    [SerializeField]
    Image healthbar;

    Rigidbody2D rigidBody;

    public Weapon currentWeapon;
    public const float SPEED = 20f;
    const float MAX_HEALTH = 100f;
    float health = MAX_HEALTH;

    bool isMoving = false;

    public const float DELAY_BEFORE_ACTION = 3f;
    public const float MOVING_TIME = 3f;

    // in range 0.3 - 1
    float powerOfFireProjectile = 0.3f;

    private void Start()
    {
        currentWeapon = new Weapon();
        currentWeapon.caster = this;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Shoot()
    {
        FreezeRotation(true);
        CameraController.INSTANCE.MoveSmoothToPosition(shootPosition.position);
        StartCoroutine(_ShootWithDelay());
    }

    public void MoveRight()
    {
        FreezeRotation(false);
        StartCoroutine(_Move(Vector2.right * SPEED * Time.deltaTime));
        Invoke("Stop", MOVING_TIME + DELAY_BEFORE_ACTION);
    }
    
    public void MoveLeft()
    {
        FreezeRotation(false);
        StartCoroutine(_Move(Vector2.left * SPEED * Time.deltaTime));
        Invoke("Stop", MOVING_TIME + DELAY_BEFORE_ACTION);
    }

    public void Stop()
    {
        isMoving = false;
    }

    public void RecieveDamage(float damage)
    {
        rigidBody.AddRelativeForce(new Vector2(0.3f, 0.3f), ForceMode2D.Impulse);

        health -= damage;
        healthbar.DOFillAmount((health / 100f), 1f);

        Vector3 pos = new Vector3(
            transform.position.x + new System.Random().Next(-1, 1),
            transform.position.y + 0.5f,
            0);
        var dmgTxt = PoolingSystem.Spawn(PoolManager.INSTANCE.GetDamageTextPrefab(), pos);
        dmgTxt.GetComponent<DamageText>().SetDamageValue(damage);

        if (health <= 0)
            Die();
    }

    public void GainHealth(float health)
    {
        this.health += health;

        if (this.health > 100)
          this.health = 100;

         Vector3 pos = new Vector3(
            transform.position.x + new System.Random().Next(-1, 1),
            transform.position.y + 0.5f,
            0);

        var healTxt = PoolingSystem.Spawn(PoolManager.INSTANCE.GetDamageTextPrefab(), pos);
        healTxt.GetComponent<DamageText>().SetHealingValue(health);

        PoolingSystem.Spawn(PoolManager.INSTANCE.GetWeaponPrefab(SuperWeaponType.HEAL),
            transform.position);

        healthbar.DOFillAmount((this.health / 100f), 1f);
    }

    public void FreezeRotation(bool freeze)
    {
        if(rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        if (freeze)
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        else
            rigidBody.constraints = RigidbodyConstraints2D.None;
    }

    public void UseSuperWeapon()
    {
        FreezeRotation(true);
        StartCoroutine(_UseSuperWeaponWithDelay());
    }

    public void RotateCannon(Vector3 rotation)
    {
        canon.localEulerAngles = rotation;
    }

    IEnumerator _Move(Vector2 velocity)
    {
        yield return new WaitForSeconds(DELAY_BEFORE_ACTION);
        isMoving = true;

        while (isMoving)
        {
            rigidBody.velocity = velocity;
            yield return null;
        }
    }

    IEnumerator _ShootWithDelay()
    {
        yield return new WaitForSeconds(DELAY_BEFORE_ACTION);

        AudioController.INSTANCE.PlayAudio(AudioClipType.FIRE_GIRL);
        yield return new WaitForSeconds(0.8f);

        var rotation = shootPosition.transform.rotation.eulerAngles;
        rotation.z -= 90;
        Quaternion rot = Quaternion.Euler(rotation);

        currentWeapon.Fire(shootPosition.position, rot, powerOfFireProjectile);
    }

    IEnumerator _UseSuperWeaponWithDelay()
    {
        yield return new WaitForSeconds(DELAY_BEFORE_ACTION);

        currentWeapon.UseSuperWeapon(Quaternion.identity);
    }

    void Die()
    {
        PoolingSystem.Spawn(PoolManager.INSTANCE.explosionPrefab, transform.position);
        gameObject.SetActive(false);
        AudioController.INSTANCE.PlayAudio(AudioClipType.HOORAY);
    }

    public float PowerOfFireProjectile
    {
        get { return powerOfFireProjectile; }
        set { powerOfFireProjectile = value; }
    }

}
