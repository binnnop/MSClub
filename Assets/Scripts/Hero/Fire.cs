using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject fireTeleport;
    public GameObject fireBomb;
    public TowerAI selfAtk;
    public ParticleSystem fireParticle;

    void Start()
    {
        fireTeleport = transform.GetChild(1).gameObject;
        fireBomb = transform.GetChild(3).gameObject;
        fireParticle.Stop();
        fireParticle.Clear();
        selfAtk = GetComponent<TowerAI>();
    }

    
    void Update()
    {
        if (!fireParticle.isPlaying && !fireParticle.isEmitting)
        {
            fireParticle.Stop();
            fireParticle.Clear();
        }
    }
    public void ShowEffect()
    {
        fireTeleport.SetActive(true);
    }

    public void HideEffect()
    {
        fireTeleport.SetActive(false);
    }
    public void Atkbuff(cardInfo card)
    {
        fireParticle.Play();
        int increasement = card.price / 20;
        selfAtk.towerAtk += increasement;
        
    }

}
