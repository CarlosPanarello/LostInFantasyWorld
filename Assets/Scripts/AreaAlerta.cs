using UnityEngine;
using System.Collections;

public class AreaAlerta : MonoBehaviour {


    public GameObject objetoComAnimacao;
    public GameObject objetoComMovimento;
    public float larguraAreaAlerta;

    private MoverObjetos scriptMovimento;
    private Animator anim;

    // Use this for initialization
    void Start () {
        scriptMovimento = objetoComMovimento.GetComponent<MoverObjetos>();
        anim = objetoComAnimacao.GetComponent<Animator>();

        if (larguraAreaAlerta > 0) { 
            BoxCollider2D areaAlerta = GetComponent<BoxCollider2D>();
            areaAlerta.size = new Vector2(larguraAreaAlerta, areaAlerta.size.y);
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void startMovimento() {
        scriptMovimento.startMovimento = true;
        anim.SetBool("ativo", true);
    }

    private void stopMovimento() {
        scriptMovimento.startMovimento = false;
        anim.SetBool("ativo", false);
    }


    void OnTriggerStay2D(Collider2D outro) {
        Global.typeOfPlayer tipo = Global.getTypeOfPlayerByTag(outro.tag);
        if(!tipo.Equals(Global.typeOfPlayer.Player_None)) {
            startMovimento();
            
        }
    }

     void OnTriggerExit2D(Collider2D outro) {
        Global.typeOfPlayer tipo = Global.getTypeOfPlayerByTag(outro.tag);
        if (!tipo.Equals(Global.typeOfPlayer.Player_None)) {
            StartCoroutine("PararEm2Segudos");
        }
    }

    private IEnumerator PararEm2Segudos() {
        yield return new WaitForSeconds(2f);
        stopMovimento();
    }
}
