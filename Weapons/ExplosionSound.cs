using UnityEngine;

public class ExplosionSound : MonoBehaviour, IPoolableComponent {

    public void Despawned()
    {
    }

    public void Spawned()
    {
             AudioController.INSTANCE.PlayAudio(AudioClipType.EXPLOSION);
    }
}
