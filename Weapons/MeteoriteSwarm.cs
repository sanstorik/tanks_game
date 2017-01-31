using UnityEngine;

public class MeteoriteSwarm : MonoBehaviour, IPoolableComponent {

    public GameObject meteorPrefab;
    System.Random rand = new System.Random();

    public void Despawned()
    {
       TurnController.INSTANCE.SwitchTurn();
    }

    public void Spawned()
    {
            AudioController.INSTANCE.PlayAudio(AudioClipType.BY_FIRE);
            Invoke("_Despawn", 15f);
            StartCoroutine(_SpawnWithDelay());
            ScreenOpacity.INSTANCE.MakeSmoothDarker(0.4f, 4f);
    }

    private Vector3 RandomPosition()
    {
        Vector3 pos = new Vector3(
             rand.Next((int)CameraController.INSTANCE.leftCorner.x, (int)CameraController.INSTANCE.rightCorner.x),
            CameraController.INSTANCE.spawnSpot.transform.position.y,
            0);

        return pos;
    }

    private void _Despawn()
    {
        PoolingSystem.Despawn(gameObject);
        ScreenOpacity.INSTANCE.MakeSmoothNormal(2f);
    }

    System.Collections.IEnumerator _SpawnWithDelay()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 30; i++)
        {
            var go = PoolingSystem.Spawn(meteorPrefab, RandomPosition());
            go.transform.localScale = Vector3.one * (rand.Next(200, 350) / 1000f);
            go.transform.GetComponent<Rigidbody2D>().velocity =
                new Vector2((rand.Next(200, 350) / -100), (rand.Next(100, 400) / -100));
            go.GetComponent<Meteor>().ChangeParticleSystemSize();

            if (i % 16 == 0 && i > 0)
            {
                AudioController.INSTANCE.PlayAudio(AudioClipType.DIE_INSECT);
            }

            if (i % 4 == 0)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        yield return new WaitForSeconds(1.5f);
        AudioController.INSTANCE.PlayAudio(AudioClipType.SEE_YOU_BURN);
    }

}
