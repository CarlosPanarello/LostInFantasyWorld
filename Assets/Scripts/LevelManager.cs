using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public float tempoEsperaRespawn;

	public PlayerControl jogador_Green;
	public PlayerControl jogador_Bege;
	public PlayerControl jogador_Blue;
	public PlayerControl jogador_Pink;
	public PlayerControl jogador_Yellow;

    public LayerMask whatIsGround;
    public LayerMask whatIsWater;
    public LayerMask whatIsLava;

	public GameObject explosao;

    private ResetOnRespawn[] objetosParaResetar;

    //Novos Metodos
    public bool canRespaw;
    private List<PlayerControl> listOfPlayers;
    private PlayerControl playerActive;

    public delegate void LevelActions(IPlayerAction action);
    public static event LevelActions OnLevelAction;

    public delegate void LevelPlayerRespaw(int coins);
    public static event LevelPlayerRespaw OnRespaw;

    void Awake() {
        InputController.OnPlayerAction += actionController;
        ScoreController.OnHealthGoesToZero += playerDies;
    }

    private void playerDies(Global.typeOfPlayer player) {
        foreach (PlayerControl scriptPlayer in listOfPlayers) {
            if (scriptPlayer.tipo.Equals(player)) {

                if (canRespaw) {

                    OnRespaw(0);
                }

            }
        }
    }

    private void actionController(IPlayerAction action) {
        if (action.changePlayer(listOfPlayers)) {
            if (OnLevelAction != null) { 
                OnLevelAction(new ActionChangePlayer());
            }
        }
    }

    void Start() {
        inicializarListaJogaresPossiveis();
    }

    private void inicializarListaJogaresPossiveis() {
        listOfPlayers = new List<PlayerControl>(5);

        if(jogador_Green != null) {
            if (playerActive == null) {
                jogador_Green.ativarJogador();
                playerActive = jogador_Green;
            }else {
                jogador_Green.desativarJogador();
            }

            listOfPlayers.Add(jogador_Green);
        }
        if (jogador_Bege != null) {
            if (playerActive == null) {
                jogador_Bege.ativarJogador();
                playerActive = jogador_Bege;
            } else {
                jogador_Bege.desativarJogador();
            }

            listOfPlayers.Add(jogador_Bege);
        }
        if (jogador_Blue != null) {
            if (playerActive == null) {
                jogador_Blue.ativarJogador();
                playerActive = jogador_Blue;
            } else {
                jogador_Blue.desativarJogador();
            }

            listOfPlayers.Add(jogador_Blue);
        }
        if (jogador_Pink != null) {
            if (playerActive == null) {
                jogador_Pink.ativarJogador();
                playerActive = jogador_Pink;
            } else {
                jogador_Pink.desativarJogador();
            }

            listOfPlayers.Add(jogador_Pink);
        }
        if (jogador_Yellow != null) {
            if (playerActive == null) {
                jogador_Yellow.ativarJogador();
                playerActive = jogador_Yellow;
            } else {
                jogador_Yellow.desativarJogador();
            }

            listOfPlayers.Add(jogador_Yellow);
        }
    }
	
	// Update is called once per frame
	void Update () {
        /*
        foreach (PlayerControl player in listOfPlayers) {
            if (player.contadorVida <= 0 && !player.renascendo) {
                player.renascendo = true;
                Respawn(player);
            }
        }
        */
	}

	public void Respawn(PlayerControl jogadorRespaw){
		//TIpo Thread
		StartCoroutine ("RespawnCoRoutine", jogadorRespaw);
	}

	public IEnumerator RespawnCoRoutine(PlayerControl jogadorRespaw) {

        jogadorRespaw.gameObject.SetActive (false);

        // Não funciona o objeto ja esta foi criado.
        //explosao.GetComponent<ParticleSystem>().startColor = jogador_ativo.obterCorParaExplosaoMorte();

        Instantiate (explosao, jogadorRespaw.transform.position, jogadorRespaw.transform.rotation);
        
        yield return new WaitForSeconds (tempoEsperaRespawn);

        //jogadorRespaw.Respawing(vidaMaxima);

        jogadorRespaw.gameObject.SetActive (true);

        jogadorRespaw.renascendo = false;

        //UpdateCoracao ();

        // Resetando as moedas e iniciando objetos;
        foreach (ResetOnRespawn objeto in objetosParaResetar) {
            objeto.gameObject.SetActive(true);
            objeto.ResetObject();
        }

	}
}
