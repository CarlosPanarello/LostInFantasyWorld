using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

	public float followAhead;

	public float velocidadeCamera;
	public float velCameraMudJogador;

	private Vector3 targetPosition;

    void Awake() {
        MakeInstance();
    }

    void MakeInstance() {
        if (instance == null) {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        targetPosition = Vector3.zero;
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
        posicionarCameraAlvo(transform.position);
    }

	public void posicionarCameraAlvo(GameObject alvo){
        posicionarCameraAlvo(pegarPosicao(alvo));
	}

    public void posicionarCameraAlvo(Vector3 position) {
        targetPosition = position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, velocidadeCamera * Time.fixedDeltaTime);
    }

    public void posicionarCameraComOutroPlayer(GameObject alvo) {
        posicionarCameraComOutroPlayer(pegarPosicao(alvo));
    }

    public void posicionarCameraComOutroPlayer(Vector3 position) {
        targetPosition = position;
        transform.position = Vector3.Lerp(transform.position, position, velCameraMudJogador * Time.fixedDeltaTime);
    }
}
