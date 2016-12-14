using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject target;
	public float followAhead;

	public float velocidadeCamera;
	public float velCameraMudJogador;

	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
	
	}
	// Pega a posicao do objeto e adiciona um valor a esquerda ou direita
	private Vector3 pegarPosicao(GameObject objeto){
		Vector3 posicao;
		posicao = new Vector3 (objeto.transform.position.x, objeto.transform.position.y, transform.position.z);

		if (objeto.transform.localScale.x > 0f) {
			posicao = new Vector3 (posicao.x + followAhead, posicao.y, posicao.z);
		} else {
			posicao = new Vector3 (posicao.x - followAhead, posicao.y, posicao.z);
		}
		return posicao;
	}
	
	// Update is called once per frame
	void Update () {
		targetPosition = pegarPosicao (target);

		//transform.position = targetPosition;

		transform.position = Vector3.Lerp (transform.position, targetPosition, velocidadeCamera * Time.fixedDeltaTime);
	}

	public void posicionarCameraAlvo(GameObject alvo){
        target = alvo;
		targetPosition = pegarPosicao (target);

		transform.position = Vector3.Lerp (transform.position, targetPosition, velCameraMudJogador * Time.fixedDeltaTime);
	}
}
