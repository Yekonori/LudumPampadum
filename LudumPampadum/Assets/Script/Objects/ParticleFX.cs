using UnityEngine;

[System.Serializable]
public class ParticleFX
{

    public string name;
    public bool IntOnAwake;

    public GameObject particle;

    [HideInInspector]
    public ParticleSystem systeme;

}
