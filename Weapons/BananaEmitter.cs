using UnityEngine;
using System.Collections;

public class BananaEmitter : WeaponBehaviour {

    IEnumerator MakeInteractibleAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public override void Spawned()
    {
            StartCoroutine(MakeInteractibleAfterDelay());
    }

    private void FixedUpdate()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        euler.z += 3;
        transform.rotation = Quaternion.Euler(euler);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BananaEmitter>() == null)
        {
            explosionObject = PoolingSystem.Spawn(explosionPrefab, transform.position);
            CheckForEnemiesAndDealDamage();
            DespawnExplosionAfterDelay(2f);

            PoolingSystem.Despawn(gameObject);
        }
    }
}
