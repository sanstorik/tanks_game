using UnityEngine;
using System.Collections;

public class HommingMissile : Rocket
{
    public GameObject lineRendererPrefab;
    public GameObject xMarkTheSpotPrefab;
    public AudioClip targetAquired;

    bool isStopped = false;
    bool movingTowardsTarget = false;
    bool nonStatic = false;
    RaycastHit2D hit;

    GameObject xMarkObject;
    GameObject lineObject;
    LineRenderer line;

    Transform enemyTarget;

    float rotateSpeed;

    public override void Spawned()
    {
        base.Spawned();
        rigidBody.isKinematic = false;
        isStopped = false;
        movingTowardsTarget = false;
        nonStatic = false;

        Invoke("ChangeKinematicOfRocket", 1.5f);
    }


    public override void Start()
    {
        base.Start();
        weaponType = WeaponType.HOMMING_MISSILE;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (lineObject != null && xMarkObject != null)
        {
            PoolingSystem.Despawn(lineObject);
            PoolingSystem.Despawn(xMarkObject);
        }
    }

    public void CasterIsPlayer(bool isCasterPlayer)
    {
        rotateSpeed = isCasterPlayer ? -1f : 1f;
    }

    void FixedUpdate()
    {
        if (!isStopped && !movingTowardsTarget)
            RotateObjectTheWayItFacing();

        else if(isStopped && !nonStatic)
        {
            transform.RotateAround(transform.position, Vector3.forward, rotateSpeed);

            hit = Physics2D.Raycast(transform.position, transform.up , 50, tankMask);
            if (hit && hit.collider.gameObject.GetComponent<TankController>() != null)
            {
                AimWhenEnemyWasFound();
            }
        }
        else if (movingTowardsTarget)
        {
            MoveToTarget();
            line.SetPosition(0, transform.position);
            line.SetPosition(1, enemyTarget.position * 0.95f);
        }
    }


    void AimWhenEnemyWasFound()
    {
        AudioController.INSTANCE.PlayAudio(targetAquired);

        enemyTarget = hit.collider.transform;
        StartCoroutine(_MoveAfterDelay(4f));
        nonStatic = true;

        lineObject = PoolingSystem.Spawn(lineRendererPrefab, transform.position);
        line = lineObject.GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, enemyTarget.position);

        xMarkObject = PoolingSystem.Spawn(xMarkTheSpotPrefab, enemyTarget.position);
    }


    void ChangeKinematicOfRocket()
    {
        if (gameObject.activeSelf)
        {
            rigidBody.isKinematic = !rigidBody.isKinematic;
            isStopped = !isStopped;
        }
    }

    void MoveToTarget()
    {
        var dir = transform.position - enemyTarget.position;
        if (dir.sqrMagnitude < 0.01f)
            OnCollisionEnter2D(new Collision2D());

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = Vector3.MoveTowards(transform.position, enemyTarget.position, Time.deltaTime * 10);
    }

    IEnumerator _MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        movingTowardsTarget = true;
        ChangeKinematicOfRocket();
    }
}