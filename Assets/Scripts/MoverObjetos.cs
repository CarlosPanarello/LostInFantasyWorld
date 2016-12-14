using UnityEngine;
using System.Collections;

public class MoverObjetos : MonoBehaviour {

	public GameObject objeto;
	public Transform comeco;
	public Transform fim;

    public bool startMovimento;

    public float moveSpeed;

	private Vector3 alvoAtual;

	// Use this for initialization
	void Start () {
		alvoAtual = fim.position;
	}
	
    private void movimentar() {
        objeto.transform.position =
        Vector3.MoveTowards(objeto.transform.position, alvoAtual, moveSpeed * Time.deltaTime);

        if (objeto.transform.position == fim.position) {
            alvoAtual = comeco.transform.position;
        }

        if (objeto.transform.position == comeco.position) {
            alvoAtual = fim.transform.position;
        }
    }

	// Update is called once per frame
	void Update () {
        if (startMovimento) {
            movimentar();
        } 
	}
}
