using UnityEngine;
using DG.Tweening;

public class DamageText : MonoBehaviour, IPoolableComponent {
    TextMesh textMesh;
    MeshRenderer mesh;

	void Start () {
        textMesh = GetComponent<TextMesh>();
        mesh = GetComponent<MeshRenderer>();
        mesh.sortingOrder = 10;
	}
	

    private void MoveRight()
    {
        transform.DOMoveX(transform.position.x + 1f, 1.4f).OnComplete(() => MoveLeft());
    }

    private void MoveLeft()
    {
        transform.DOMoveX(transform.position.x - 1f, 1.4f).OnComplete(() => MoveRight());
    }

    public void SetDamageValue(float damage)
    {
        if(textMesh == null)
            textMesh = GetComponent<TextMesh>();
        textMesh.text = "-" + damage;
    }

    public void SetHealingValue(float heal)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMesh>();
        textMesh.text = "+" + heal;
    }

    public void SetEffect(string effect)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMesh>();
        textMesh.text = effect;
    }

    private void _Despawn()
    {
        PoolingSystem.Despawn(gameObject);
    }

    public void Spawned()
    {
            transform.DOMoveY(transform.position.y + 5f, 5f);

            Invoke("_Despawn", 2.8f);
    }

    public void Despawned()
    {
    }
}
