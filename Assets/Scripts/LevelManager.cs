using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

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

    //Novos Metodos
    private int qtdMortos = 0;
    public bool canRespaw;
    public List<PlayerControl> listOfPlayers;
    public List<PlayerControl> listOfPlayersDesativados;

    public delegate void LevelActions(IPlayerAction action,Global.typeOfPlayer tipoPlayer);
    public static event LevelActions OnLevelAction;

    public delegate void LevelPlayerRespaw(Global.typeOfPlayer player);
    public static event LevelPlayerRespaw OnRespaw;

    public delegate void LevelPlayerKill(Global.typeOfPlayer player);
    public static event LevelPlayerKill OnKillPlayer;

    void MakeInstance() {
        if (instance == null) {
            instance = this;
        }
    }

    void Awake() {
        MakeInstance();
        InputController.OnPlayerAction += actionController;
        ScoreController.OnHealthGoesToZero += playerDies;
    }

    private void playerDies(Global.typeOfPlayer player) {
        Debug.Log("Executando o OnHealthGoesToZero no LevelManager");
        if (OnRespaw != null) { 
            foreach (PlayerControl scriptPlayer in listOfPlayers) {
                if (scriptPlayer.tipo.Equals(player)) {

                    if (canRespaw) {
                        OnRespaw(player);
                    } else {
                        InputController.instance.changePlayer(true);
                        OnKillPlayer(player);
                        qtdMortos++;
						Debug.Log("Jogador Morreu--> " + player.ToString() + " Posicao->"  + qtdMortos );
                        if (qtdMortos == listOfPlayers.Count) {
                            Debug.Log("GAME OVER");
                        }
                    }
                }
            }
        }
    }

    private void actionController(IPlayerAction action) {
        if (action.changePlayer(listOfPlayers)) {
            Global.typeOfPlayer type = Global.typeOfPlayer.Player_None;

            foreach (PlayerControl player in listOfPlayers) {
                if (player.isAtivo) {
                    type = player.tipo;
                    break;
                }
            }

            if (OnLevelAction != null) { 
				OnLevelAction(action,type);
            }
        }
    }

    void Start() {
        inicializarListaJogaresPossiveis();
    }

    private void inicializarListaJogaresPossiveis() {
        listOfPlayers = new List<PlayerControl>(5);
        listOfPlayersDesativados = new List<PlayerControl>(1);
        PlayerControl playerActive = getActivePlayer ();

        if(jogador_Green != null) {
            if (playerActive == null) {
                jogador_Green.ativarJogador();
                playerActive = jogador_Green;
            }else {
                jogador_Green.desativarJogador();
            }
			jogador_Green.isGrounded = true;
            listOfPlayers.Add(jogador_Green);
        }
        if (jogador_Bege != null) {
            if (playerActive == null) {
                jogador_Bege.ativarJogador();
                playerActive = jogador_Bege;
            } else {
                jogador_Bege.desativarJogador();
            }
			jogador_Bege.isGrounded = true;
            listOfPlayers.Add(jogador_Bege);
        }
        if (jogador_Blue != null) {
            if (playerActive == null) {
                jogador_Blue.ativarJogador();
                playerActive = jogador_Blue;
            } else {
                jogador_Blue.desativarJogador();
            }
			jogador_Blue.isGrounded = true;

            listOfPlayers.Add(jogador_Blue);
        }
        if (jogador_Pink != null) {
            if (playerActive == null) {
                jogador_Pink.ativarJogador();
                playerActive = jogador_Pink;
            } else {
                jogador_Pink.desativarJogador();
            }
			jogador_Pink.isGrounded = true;

            listOfPlayers.Add(jogador_Pink);
        }
        if (jogador_Yellow != null) {
            if (playerActive == null) {
                jogador_Yellow.ativarJogador();
                playerActive = jogador_Yellow;
            } else {
                jogador_Yellow.desativarJogador();
            }
			jogador_Yellow.isGrounded = true;

            listOfPlayers.Add(jogador_Yellow);
        }

    }
	
	// Update is called once per frame
	void Update () {
	}

    public Vector3 posicaoJogadorAtivo() {
		return getActivePlayer().transform.position;
    }

	public Global.typeOfPlayer getTypeActivePlayer(){
		return getActivePlayer().tipo;
	}

	public PlayerControl getActivePlayer(){
		foreach (PlayerControl player in listOfPlayers) {
			if (player.isAtivo) {
				return player;
			}
		}

		return null;
	}
}
