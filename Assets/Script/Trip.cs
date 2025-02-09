using UnityEngine;

public class Trip : MonoBehaviour {
    [SerializeField] float destroyRadius;
    [SerializeField] LayerMask mask;
    [SerializeField] ParticleSystem trapParticle;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 6){
            DestroyItems();
        }
    }
    Collider[] items;
    private void DestroyItems() {
        items = Physics.OverlapSphere(transform.position, destroyRadius,mask);
        for (int i = 0; i < items.Length; i++) {
            Destroy(items[i].gameObject);
        }
        trapParticle.transform.position = transform.position;
        trapParticle.Play();
        source.clip = clip;
        source.Play();
        Destroy(gameObject);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }
}
