using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public static ParticlePlayer instance;

    public List<ParticleData> particles;

    private void Awake()
    {
        instance = this;
    }
    public void PlayParticle(string name, Vector3 position)
    {
        foreach (var particle in particles)
        {
            if (particle.name==name)
            {
                GameObject particleTemp= Instantiate(particle.particleObject, position,Quaternion.identity);
                particleTemp.GetComponent<ParticleSystem>().Play();
                Destroy(particleTemp, particleTemp.GetComponent<ParticleSystem>().duration);
                return;

            }
        }
        Debug.LogError("Particle ismi bulunamadý!");
    }
}
[System.Serializable]
public class ParticleData
{
    public GameObject particleObject;
    public string name;
}
