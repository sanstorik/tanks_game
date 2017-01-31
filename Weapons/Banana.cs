using UnityEngine;

public class Banana : WeaponBehaviour {

    public GameObject bananaEmitterPrefab;
    System.Random rand = new System.Random();
    float force;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        explosionObject = PoolingSystem.Spawn(explosionPrefab, transform.position);
        DespawnExplosionAfterDelay(2f);
        CheckForEnemiesAndDealDamage();

        SpawnAnotherBananas();
        PoolingSystem.Despawn(gameObject);
    }

    private void FixedUpdate()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        euler.z -= forceOnFire * force / 1.5f;
        transform.rotation = Quaternion.Euler(euler);
    }

    public override void FireAndAddForce(float force)
    {
        this.force = force;
        base.FireAndAddForce(force);
    }

    public override void Despawned()
    {
        CameraController.INSTANCE.ResetTarget();
        Invoke("SwitchTurnAfterDelay", 5f);
    }

    void SwitchTurnAfterDelay()
    {
        TurnController.INSTANCE.SwitchTurn();
    }

    public void SpawnAnotherBananas()
    {
        GameObject obj;

        for(int i=0; i < 6; i++)
        {
            obj = PoolingSystem.Spawn(bananaEmitterPrefab, transform.position);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(rand.Next(-200,200)/100f, 4f), ForceMode2D.Impulse);
        }
    }
}
