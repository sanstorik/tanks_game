using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour, IPoolableComponent {
    [SerializeField]  protected float damage = 10;
    [SerializeField]  protected float explosionRange = 2;
    [SerializeField]  protected float forceOnFire = 8;
    [SerializeField]  protected GameObject explosionPrefab;
    [SerializeField]  protected bool shouldBeFollowedByCamera = true;
    [SerializeField]  protected bool isSingleWeapon = true;

    [HideInInspector]
    protected Rigidbody2D rigidBody;

    protected GameObject explosionObject;

    protected GameObject fireEffectObject;

    protected LayerMask tankMask;

    protected WeaponType weaponType;

    protected AudioSource audioSource;


    /// <param name="force"> Could varie from min(0.3) to max(1) force</param>

    public virtual void FireAndAddForce(float force = 1)
    {
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.AddRelativeForce(new Vector2(forceOnFire * 0.2f, forceOnFire * force), ForceMode2D.Impulse);
    }

    public virtual void Start() {
        tankMask = LayerMask.GetMask("Tanks");
    }

    public abstract void OnCollisionEnter2D(Collision2D collision);

    public virtual void CheckForEnemiesAndDealDamage()
    {
        var enemiesHit = Physics2D.OverlapCircleAll(transform.position, explosionRange, tankMask);
        TankController controller;

        for(int i=0; i < enemiesHit.Length; i ++)
        {
            controller = enemiesHit[i].GetComponent<TankController>();
            if (controller != null)
            {
                controller.RecieveDamage(damage);
            }
        }
    }

    public virtual void DespawnExplosionAfterDelay(float delay)
    {
        Invoke("_Despawn", delay);
    }

    void _Despawn()
    {
        if(explosionObject != null)
            PoolingSystem.Despawn(explosionObject);
    }

    public virtual void Spawned()
    {
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody2D>();

        if (isSingleWeapon)
        {
            if (shouldBeFollowedByCamera)
                CameraController.INSTANCE.SetTrackTarget(transform);
            else
                CameraController.INSTANCE.ResetTarget();

            fireEffectObject = PoolingSystem.Spawn(PoolManager.INSTANCE.GetTankFirePrefab(), transform.position);
            Invoke("_DespawnFireEffectObjectAfterDelay", 4f);
        }
    }

    private void _DespawnFireEffectObjectAfterDelay()
    {
        PoolingSystem.Despawn(fireEffectObject);
    }

    public virtual void RotateObjectTheWayItFacing()
    {
        float angle = Mathf.Atan2(rigidBody.velocity.y, rigidBody.velocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public virtual void Despawned()
    {
        if (isSingleWeapon)
        {
            CameraController.INSTANCE.ResetTarget();
            TurnController.INSTANCE.SwitchTurn();
        }
    }
}
