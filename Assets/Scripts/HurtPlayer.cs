using UnityEngine;
using System.Collections;

public class HurtPlayer : MonoBehaviour {

    public delegate void Hurt(int damage, Global.typeOfPlayer player);
    public static event Hurt OnHurtPlayer;

    public int damage;
    public float timeToTakeDamage;

    private bool executandoDano;

    // Use this for initialization
    void Start () {
        executandoDano = false;
        if (timeToTakeDamage == 0f) {
            timeToTakeDamage = 0.5f;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void executarFerimento(Collider2D outro) {
        Global.typeOfPlayer tipo = Global.getTypeOfPlayerByTag(outro.tag);

        if (OnHurtPlayer != null) {
            //Qualquer tag nao encontrada sera definida como Player_None
            //Assim qualquer outro tipo de jogador ira sofre dano
            switch (tipo) {
                case Global.typeOfPlayer.Player_None:
                    break;
                default:
                    //Será que isso eh perfomatico?
                    if (canHurt(outro.GetComponent<PlayerControl>())) {
                        OnHurtPlayer(damage, tipo);
                    }
                    break;
            }
        }
    }

    private bool canHurt(PlayerControl player) {
        Global.typeOfCanHurt thisThing = Global.getTypeOfThingsThatCanHurtPlayer(this.tag);

        if (player.invencivel) {
            return false;
        }

        switch (thisThing) {
            case Global.typeOfCanHurt.None:
                return false;
            case Global.typeOfCanHurt.Enemy:
                return true;
            case Global.typeOfCanHurt.Lava:
                return !player.canWalkInLava;
            case Global.typeOfCanHurt.Static_Danger:
                return true;
            case Global.typeOfCanHurt.Moving_Danger:
                return true;
            case Global.typeOfCanHurt.Water:
                return !player.canSwim;
            default:
                return true;
        }
    }


    void OnTriggerEnter2D(Collider2D outro) {
        executarFerimento(outro);
    }

    void OnTriggerStay2D(Collider2D outro) {
        if (!executandoDano) {
            StartCoroutine("executandoFerimento", outro);
        }
    }

    private IEnumerator executandoFerimento(Collider2D outro) {
        executandoDano = true;
        executarFerimento(outro);
        yield return new WaitForSeconds(timeToTakeDamage);
        executandoDano = false;
    }

}
