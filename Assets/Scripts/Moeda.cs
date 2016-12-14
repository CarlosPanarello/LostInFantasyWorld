using UnityEngine;
using System.Collections;

public class Moeda : MonoBehaviour {
	public LevelManager level;

	public int valorMoeda;

	// Use this for initialization
	void Start () {
		level = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			level.AddMoeda (valorMoeda);
			Destroy (gameObject);
		}
	}
}
