using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

public class star_particles : MonoBehaviour
{
    ParticleSystem.Particle[] particles;
    ParticleSystem particleSystem;
    int numAlive;
    bool itRan;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        InitializeIfNeeded();
        particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        particleSystem.SetParticles(particles, numAlive);
        particleSystem.Emit(emitOverride, 5000);
        numAlive = particleSystem.GetParticles(particles);
        if (itRan == false)
        {
            CallOnce();
        }
    }

    private void CallOnce()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].position = new Vector3(Random.Range(-500f, 500f), Random.Range(-500f, 500f), Random.Range(500f, 500f));
            particles[i].velocity = new Vector3(0, 0, 0);
        }
        itRan = true;
    }
    void InitializeIfNeeded()
    {
        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem == null || particles.Length < particleSystem.main.maxParticles)
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }
}
