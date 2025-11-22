using UnityEngine;

public class enemyParticleSystem : MonoBehaviour
{
    public static enemyParticleSystem instance { get; private set; }
    [SerializeField] private ParticleSystem DeathPsPrefab;
    [SerializeField] private ParticleSystem DamagePsPrefab;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {   
    }



    static public void playDeathParticles(Vector2 pos)
    {
        ParticleSystem ps = Instantiate(instance.DeathPsPrefab, pos, Quaternion.identity);
        ps.Play();
        Destroy(ps.gameObject, 5);
    }
    static public void playDamageParticles(Vector2 pos, Quaternion rot)
    {
        ParticleSystem ps = Instantiate(instance.DamagePsPrefab, pos, rot);
        ps.Play();

        Destroy(ps.gameObject, 5);
    }

}