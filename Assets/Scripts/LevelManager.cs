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

    public CameraControl cameraPrincipal;
        
	public GameObject explosao;

	public int contadorMoeda;
	public Text moedaText;
	public AudioSource somMoeda;

	public Image coracao_holder_1;
	public Image coracao_holder_2;
	public Image coracao_holder_3;

	public Sprite coracao_cheio;
	public Sprite coracao_metade;
	public Sprite coracao_vazio;

	public int vidaMaxima;
	
    
	public AudioSource levelMusic;
	public AudioSource gameOverMusic;

    public int volumeSFX;
    public int volumeMusic;

    private PlayerControl jogador_ativo;
    private List<PlayerControl> listaJogadores;

    private ResetOnRespawn[] objetosParaResetar;

    // Use this for initialization
    void Start() {
        inicializarListaJogaresPossiveis();
        cameraPrincipal = FindObjectOfType<CameraControl>();
        cameraPrincipal.posicionarCameraAlvo(jogador_ativo.gameObject);
        ajusteSom(volumeSFX, volumeMusic);
        // moedaText.text = "Coins: " + contadorMoeda.ToString();

        objetosParaResetar = FindObjectsOfType<ResetOnRespawn>();
    }

    private void ajusteSom(int sfx,int music) {
        if (music <= 0) {
            levelMusic.volume = 0f;
        } else {
            levelMusic.volume = music / 100;
        }

        if (sfx <= 0) {
            levelMusic.volume = 0f;
        } else {
            levelMusic.volume = sfx / 100;
        }
    }

    private void inicializarListaJogaresPossiveis() {
        listaJogadores = new List<PlayerControl>(5);

        if(jogador_Green != null) {
            if (jogador_ativo==null) {
                jogador_Green.ativarJogador();
                jogador_ativo = jogador_Green;
            }else {
                jogador_Green.desativarJogador();
            }
            jogador_Green.contadorVida = vidaMaxima;
            listaJogadores.Add(jogador_Green);
        }
        if (jogador_Bege != null) {
            if (jogador_ativo == null) {
                jogador_Bege.ativarJogador();
                jogador_ativo = jogador_Bege;
            } else {
                jogador_Bege.desativarJogador();
            }
            jogador_Bege.contadorVida = vidaMaxima;
            listaJogadores.Add(jogador_Bege);
        }
        if (jogador_Blue != null) {
            if (jogador_ativo == null) {
                jogador_Blue.ativarJogador();
                jogador_ativo = jogador_Blue;
            } else {
                jogador_Blue.desativarJogador();
            }
            jogador_Blue.contadorVida = vidaMaxima;
            listaJogadores.Add(jogador_Blue);
        }
        if (jogador_Pink != null) {
            if (jogador_ativo == null) {
                jogador_Pink.ativarJogador();
                jogador_ativo = jogador_Pink;
            } else {
                jogador_Pink.desativarJogador();
            }
            jogador_Pink.contadorVida = vidaMaxima;
            listaJogadores.Add(jogador_Pink);
        }
        if (jogador_Yellow != null) {
            if (jogador_ativo == null) {
                jogador_Yellow.ativarJogador();
                jogador_ativo = jogador_Yellow;
            } else {
                jogador_Yellow.desativarJogador();
            }
            jogador_Yellow.contadorVida = vidaMaxima;
            listaJogadores.Add(jogador_Yellow);
        }
    }
	
	// Update is called once per frame
	void Update () {

        foreach (PlayerControl player in listaJogadores) {
            if (player.contadorVida <= 0 && !player.renascendo) {
                player.renascendo = true;
                Respawn(player);
            }
        }

		//Selecão do jogador
		if (Input.GetButtonDown ("Fire3")) {
            jogador_ativo = obterProximoJogador();
            cameraPrincipal.posicionarCameraAlvo(jogador_ativo.gameObject);
        }

	}

    private PlayerControl obterProximoJogador() {
        int indiceJogadorAtivo = 0;
        PlayerControl retorno;

        foreach (PlayerControl jogador in listaJogadores) {
            if (jogador.isAtivo) {
                jogador.desativarJogador();
                break;
            }
            indiceJogadorAtivo++;
        }

        if (indiceJogadorAtivo >= listaJogadores.Count-1) {
            indiceJogadorAtivo = 0;
        } else {
            indiceJogadorAtivo++;
        }

        retorno = listaJogadores[indiceJogadorAtivo];
        retorno.ativarJogador();
        return retorno;
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

        jogadorRespaw.Respawing(vidaMaxima);

        jogadorRespaw.gameObject.SetActive (true);

        jogadorRespaw.renascendo = false;

        //UpdateCoracao ();

        // Resetando as moedas e iniciando objetos;
        contadorMoeda = 0;
        foreach (ResetOnRespawn objeto in objetosParaResetar) {
            objeto.gameObject.SetActive(true);
            objeto.ResetObject();
        }

	}

	public void AddMoeda(int moedasAdd){
		contadorMoeda += moedasAdd;
		moedaText.text = "Coins: " + contadorMoeda.ToString();
		somMoeda.Play ();
	}

    public Vector3 posicaoJogadorAtivo() {
        return jogador_ativo.transform.position;
    }

	public void FerirJogador(int dano,Global.tipoPlayer tipo){
        switch (tipo) {
            case Global.tipoPlayer.Player_Green:
                jogador_Green.HurtPlayer(dano);
                break;
            case Global.tipoPlayer.Player_Bege:
                jogador_Bege.HurtPlayer(dano);
                break;
            case Global.tipoPlayer.Player_Blue:
                jogador_Blue.HurtPlayer(dano);
                break;
            case Global.tipoPlayer.Player_Pink:
                jogador_Pink.HurtPlayer(dano);
                break;
            case Global.tipoPlayer.Player_Yellow:
                jogador_Yellow.HurtPlayer(dano);
                break;
            default:
                //jogador_ativo.HurtPlayer(dano);
                break;
        }
        
		//UpdateCoracao ();
	}

	public void AdicionarVida(int qtd){
		jogador_ativo.vida = jogador_ativo.vida + qtd;
	}
	
	
	public void UpdateCoracao(){
		switch (jogador_ativo.contadorVida) {
		case 6:
			coracao_holder_1.sprite = coracao_cheio;
			coracao_holder_2.sprite = coracao_cheio;
			coracao_holder_3.sprite = coracao_cheio;
			return;
		case 5:
			coracao_holder_1.sprite = coracao_cheio;
			coracao_holder_2.sprite = coracao_cheio;
			coracao_holder_3.sprite = coracao_metade;
			return;
		case 4:
			coracao_holder_1.sprite = coracao_cheio;
			coracao_holder_2.sprite = coracao_cheio;
			coracao_holder_3.sprite = coracao_vazio;
			return;
		case 3:
			coracao_holder_1.sprite = coracao_cheio;
			coracao_holder_2.sprite = coracao_metade;
			coracao_holder_3.sprite = coracao_vazio;
			return;
		case 2:
			coracao_holder_1.sprite = coracao_cheio;
			coracao_holder_2.sprite = coracao_vazio;
			coracao_holder_3.sprite = coracao_vazio;
			return;
		case 1:
			coracao_holder_1.sprite = coracao_metade;
			coracao_holder_2.sprite = coracao_vazio;
			coracao_holder_3.sprite = coracao_vazio;
			return;
		case 0:
			coracao_holder_1.sprite = coracao_vazio;
			coracao_holder_2.sprite = coracao_vazio;
			coracao_holder_3.sprite = coracao_vazio;
			return;
		default:
			coracao_holder_1.sprite = coracao_vazio;
			coracao_holder_2.sprite = coracao_vazio;
			coracao_holder_3.sprite = coracao_vazio;
			return;
		}
	}
}
