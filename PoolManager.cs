using UnityEngine;
using System.Collections;

public class PoolManager : MonoBehaviour {
    static PoolManager instance;

    //in enum order
    public GameObject[] weaponPrefabs;
    public GameObject[] superWeaponPrefabs;
    public GameObject meteorPrefab;

    public GameObject explosionPrefab;
    public GameObject lineRendererPrefab;
    public GameObject bananaEmitter;

    public GameObject[] otherPrefabs;
    public GameObject starsPrefab;
    public GameObject tankFirePrefab;
    public GameObject treasurePrefab;
    public GameObject confettiPrefab;

    public GameObject damageTextPrefab;

    public Transform poolHolder;

    public GameObject poisonHitPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i=0; i < weaponPrefabs.Length; i++)
        {
            PoolingSystem.Prespawn(weaponPrefabs[i], 1, poolHolder);
        }

        for(int i=0; i < superWeaponPrefabs.Length; i++)
        {
            PoolingSystem.Prespawn(superWeaponPrefabs[i], 1, poolHolder);
        }

        PoolingSystem.Prespawn(bananaEmitter, 6, poolHolder);
        PoolingSystem.Prespawn(meteorPrefab, 19, poolHolder);
        PoolingSystem.Prespawn(explosionPrefab, 19, poolHolder);
        PoolingSystem.Prespawn(lineRendererPrefab, 1, poolHolder);
        PoolingSystem.Prespawn(tankFirePrefab, 1, poolHolder);
        PoolingSystem.Prespawn(poisonHitPrefab, 3, poolHolder);
        PoolingSystem.Prespawn(treasurePrefab, 2, poolHolder);
        PoolingSystem.Prespawn(starsPrefab, 2, poolHolder);
        PoolingSystem.Prespawn(confettiPrefab, 1, poolHolder);

        PoolingSystem.Prespawn(damageTextPrefab, 10, poolHolder);

        for (int i = 0; i < otherPrefabs.Length; i++)
            PoolingSystem.Prespawn(otherPrefabs[i], 2, poolHolder);

        CameraController.INSTANCE.gameIsStarted = true;
    }

    public GameObject GetWeaponPrefab(WeaponType type)
    {
        return weaponPrefabs[(int)type];
    }

    public GameObject GetWeaponPrefab(SuperWeaponType type)
    {
        return superWeaponPrefabs[(int)type];
    }

    public GameObject GetTankFirePrefab()
    {
        return tankFirePrefab;
    }

    public GameObject GetDamageTextPrefab()
    {
        return damageTextPrefab;
    }

    static public PoolManager INSTANCE
    {
        get { return instance; }
    }
}
