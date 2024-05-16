using UnityEngine;

public class AutoSetParticleRotation : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public Transform SourceTrans;
    public Transform TargetTrans;
    public float EnemyRotation;
    public float PlayerRotation;

    void Start()
    {
        
    }

    public void OnEnable()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
        var mainModule = particleSystem.main;
        if(SourceTrans.position.z > TargetTrans.position.z)
        {
            mainModule.startRotation = new ParticleSystem.MinMaxCurve(EnemyRotation, EnemyRotation);
        }
        else
        {
            mainModule.startRotation = new ParticleSystem.MinMaxCurve(PlayerRotation, PlayerRotation);
        }
    }
}