using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class ParticleProjectile : MonoBehaviour
{
    [SerializeField] ParticleSystem rifleParticleSystem;
    ParticleSystem.Particle[] particles;
    public GameObject target = null;
    [SerializeField] private float speed;
    [SerializeField] private float dammage;
    private int particlesAlive;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;
        particles = new ParticleSystem.Particle[rifleParticleSystem.main.maxParticles];
        particlesAlive = rifleParticleSystem.GetParticles(particles);
        float step = speed * Time.deltaTime;
        Vector3 distance;
        for (int i = 0; i < particlesAlive; i++) {
            distance = this.transform.InverseTransformDirection(target.transform.position - this.transform.TransformPoint(particles[i].position));
            
            if (distance.sqrMagnitude >= 0.01f) {
                particles[i].position += Vector3.Normalize(distance) * step;
            }
            //particles[i].position = Vector3.LerpUnclamped(particles[i].position, distance, step);
        }
        rifleParticleSystem.SetParticles(particles, particlesAlive);
    }

    private void OnParticleCollision(GameObject other) {
        if (!other) return;
        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyAI>().TakeDamage(dammage);
            other.GetComponent<EnemyAI>().putStagger((other.transform.position - this.transform.position).normalized * 0.01f);
        }
    }
}
