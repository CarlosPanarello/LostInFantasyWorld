using UnityEngine;
using System.Collections;

public class CheckPointControl : MonoBehaviour {

    public AudioSource ativado;

	public bool checkPointAtivo;
    public Global.tipoPlayer tipoPlayer;
    private Animator animacao;

    // Use this for initialization
    void Start () {
        animacao = GetComponent<Animator>();
        animacao.SetBool("ativo", checkPointAtivo);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    private void acaoAtivarCheckPoint() {

    }

    private void ativarBandeira(Collider2D outro,string tag) {
        if (outro.tag == tag && !checkPointAtivo) {
            animacao.SetBool("ativo", true);
            checkPointAtivo = true;
            ativado.Play();
        }
    }


	void OnTriggerEnter2D(Collider2D outro){

        ativarBandeira(outro, tipoPlayer.ToString());

	}
}
