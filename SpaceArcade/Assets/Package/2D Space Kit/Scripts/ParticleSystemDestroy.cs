using UnityEngine;

public class ParticleSystemDestroy : MonoBehaviour
{ 
	void Start () 
	{
		Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
	}
}
