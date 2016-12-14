using UnityEngine;

public class FerirJogadorControl : MonoBehaviour {

	private LevelManager level;
	public int danoCausado;


	// Use this for initialization
	void Start () {
		level = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void executarFerimento(Collider2D outro) {
        level.FerirJogador(danoCausado,Global.obterPorDescricao(outro.tag));
    }

	void OnTriggerEnter2D(Collider2D outro){
        executarFerimento(outro);
    }

    void OnTriggerStay2D(Collider2D outro) {
        executarFerimento(outro);
    }
}
