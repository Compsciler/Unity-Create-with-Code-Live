using UnityEngine;
using UnityEngine.Events;

public class WinParticles : MonoBehaviour
{
    public ParticleSystem particles;

    void Update()
    {
        if (!PlayerController.isWon)
        {
            particles.Play();
        }
    }
}