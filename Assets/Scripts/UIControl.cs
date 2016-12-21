using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UIControl : MonoBehaviour {

    public static UIControl instance;

    public Sprite s_numero_0;
    public Sprite s_numero_1;
    public Sprite s_numero_2;
    public Sprite s_numero_3;
    public Sprite s_numero_4;
    public Sprite s_numero_5;
    public Sprite s_numero_6;
    public Sprite s_numero_7;
    public Sprite s_numero_8;
    public Sprite s_numero_9;

    public Sprite s_coracao_Cheio;
    public Sprite s_coracao_3_4;
    public Sprite s_coracao_1_2;
    public Sprite s_coracao_1_4;
    public Sprite s_coracao_Vazio;

    public Sprite s_gema_azul;
    public Sprite s_gema_verde;
    public Sprite s_gema_laranja;
    public Sprite s_gema_amarela;

    public Sprite s_key_azul_enable;
    public Sprite s_key_azul_disable;
    public Sprite s_key_verde_enable;
    public Sprite s_key_verde_disable;
    public Sprite s_key_vermelho_enable;
    public Sprite s_key_vermelho_disable;
    public Sprite s_key_amarelo_enable;
    public Sprite s_key_amarelo_disable;

    public Sprite s_pause;
    public Sprite s_start;

    public Image playerAtivo;

    public Image coracao_01;
    public Image coracao_02;
    public Image coracao_03;
    public Image coracao_04;
    public Image coracao_05;
    public Image coracao_06;
    public Image coracao_07;
    public Image coracao_08;
    public Image coracao_09;
    public Image coracao_10;

    public Image hundred;
    public Image ten;
    public Image unity;

    public Image gema_azul;
    public Image gema_verde;
    public Image gema_vermelho;
    public Image gema_amarelo;

    public Image chave_azul;
    public Image chave_verde;
    public Image chave_vermelho;
    public Image chave_amarelo;

    public int quantidadeMoeda;
    public int quantidadeChaves;
    public int quantidadeGemas;

    private List<Image> listaCoracoes;
    private int hearths;
    private int currentHealth;

    private int currentCoin;

    /*    
        public Button botaoIrDireita;
        public Button botaoIrEsquerda;
        public Button botaoAcaoPular;
        public Button botaoAcaoEspecial;
        public Button botaoConfig;
        public Button botaoSair;
        */

    public Button botaoMudarJogador;
    public Button botaoPausaStart;

    public GameObject conjuntoBotaoJogador;
    public GameObject conjuntoBotaoGame;
    
    void Awake() {
        MakeInstance();
        HurtPlayer.OnHurtPlayer += updateHearthDamage;
        CollectiablesController.OnItemCollect += catchItem;
        LevelManager.OnLevelAction += changedPlayer;
        LevelManager.OnRespaw += respawPlayer;

    }

    private void respawPlayer(Global.typeOfPlayer type) {
        hearths = ScoreController.instance.getCurrentHealthFromPlayer(type);
        currentHealth = ScoreController.instance.getHearthsFromPlayer(type);

        setQuantidadeCoracao(hearths);

        updateHealthShow();
    }

    private void changedPlayer(IPlayerAction action,Global.typeOfPlayer type) {
        action.changedPlayer(botaoMudarJogador,playerAtivo);
        hearths = ScoreController.instance.getCurrentHealthFromPlayer(type);
        currentHealth = ScoreController.instance.getHearthsFromPlayer(type);
        updateHealthShow();
    }

    void MakeInstance() {
        if(instance == null) {
            inicializarItens();
            instance = this;
        }
    }

    //##########################################################################################################

    public void setQuantidadeInicialMoedas(int qtdMoedas) {
        currentCoin = qtdMoedas;
        updateCurrentCoins();
    }

    public void setQuantidadeCoracao(int fullHearts) {
        hearths = fullHearts;
        //cada coração possui 2 divisoes
        currentHealth = fullHearts * 2;
        for (int cont = 1; cont <= fullHearts; cont++) {
            switch (cont) {
                case 1:
                    coracao_01.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_01);
                    break;
                case 2:
                    coracao_02.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_02);
                    break;
                case 3:
                    coracao_03.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_03);
                    break;
                case 4:
                    coracao_04.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_04);
                    break;
                case 5:
                    coracao_05.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_05);
                    break;
                case 6:
                    coracao_06.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_06);
                    break;
                case 7:
                    coracao_07.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_07);
                    break;
                case 8:
                    coracao_08.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_08);
                    break;
                case 9:
                    coracao_09.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_09);
                    break;
                case 10:
                    coracao_10.gameObject.SetActive(true);
                    listaCoracoes.Add(coracao_10);
                    break;
                default:
                    break;
            }
        }
    }

    public void setQuantidadeChaves(int qtd) {
        switch (qtd) {
            case 0:
                chave_azul.gameObject.SetActive(false);
                chave_verde.gameObject.SetActive(false);
                chave_vermelho.gameObject.SetActive(false);
                chave_amarelo.gameObject.SetActive(false);
                break;
            case 1:
                chave_azul.sprite = s_key_azul_disable;
                chave_verde.gameObject.SetActive(false);
                chave_vermelho.gameObject.SetActive(false);
                chave_amarelo.gameObject.SetActive(false);
                break;
            case 2:
                chave_azul.sprite = s_key_azul_disable;
                chave_verde.sprite = s_key_verde_disable;
                chave_vermelho.gameObject.SetActive(false);
                chave_amarelo.gameObject.SetActive(false);
                break;
            case 3:
                chave_azul.sprite = s_key_azul_disable;
                chave_verde.sprite = s_key_verde_disable;
                chave_vermelho.sprite = s_key_vermelho_disable;
                chave_amarelo.gameObject.SetActive(false);
                break;
            default:
                chave_azul.sprite = s_key_azul_disable;
                chave_verde.sprite = s_key_verde_disable;
                chave_vermelho.sprite = s_key_vermelho_disable;
                chave_amarelo.sprite = s_key_amarelo_disable;
                return;
        }
    }

    public void ativarBotoesControleGame(bool ativar) {
        conjuntoBotaoGame.SetActive(ativar);
    }

    public void ativarBotoesControleJogador(bool ativar) {
        conjuntoBotaoJogador.SetActive(ativar);
    }

    void Start() {

    }

    //Metodos Privados
    private void catchItem(int add, Global.typeOfPlayer player,CollectiablesController item) {
        switch (item.typeOfItem) {
            case Global.typeOfItem.Gem_Blue:
                gema_azul.gameObject.SetActive(true);
                gema_azul.sprite = s_gema_azul;
                break;
            case Global.typeOfItem.Gem_Green:
                gema_verde.gameObject.SetActive(true);
                gema_verde.sprite = s_gema_azul;
                break;
            case Global.typeOfItem.Gem_Red:
                gema_vermelho.gameObject.SetActive(true);
                gema_vermelho.sprite = s_gema_azul;
                break;
            case Global.typeOfItem.Gem_Yellow:
                gema_amarelo.gameObject.SetActive(true);
                gema_amarelo.sprite = s_gema_azul;
                break;
            case Global.typeOfItem.Key_Blue:
                chave_azul.sprite = s_key_azul_enable;
                break;
            case Global.typeOfItem.Key_Green:
                chave_verde.sprite = s_key_verde_enable;
                break;
            case Global.typeOfItem.Key_Red:
                chave_vermelho.sprite = s_key_vermelho_enable;
                break;
            case Global.typeOfItem.Key_Yellow:
                chave_amarelo.sprite = s_key_amarelo_enable;
                break;
            case Global.typeOfItem.Health_empty:
                setQuantidadeCoracao(hearths + add);
                break;
            case Global.typeOfItem.Health_half:
                currentHealth += add;
                updateHealthShow();
                break;
            case Global.typeOfItem.Health_full:
                currentHealth = hearths * 2;
                updateHealthShow();
                break;
            case Global.typeOfItem.Gold_Coin:
                currentCoin += add;
                updateCurrentCoins();
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    private void inicializarItens () {
        listaCoracoes = new List<Image>();
       
        quantidadeMoeda = 0;

        gema_azul.gameObject.SetActive(false);
        gema_verde.gameObject.SetActive(false);
        gema_vermelho.gameObject.SetActive(false);
        gema_amarelo.gameObject.SetActive(false);

        coracao_01.gameObject.SetActive(false);
        coracao_02.gameObject.SetActive(false);
        coracao_03.gameObject.SetActive(false);
        coracao_04.gameObject.SetActive(false);
        coracao_05.gameObject.SetActive(false);
        coracao_06.gameObject.SetActive(false);
        coracao_07.gameObject.SetActive(false);
        coracao_08.gameObject.SetActive(false);
        coracao_09.gameObject.SetActive(false);
        coracao_10.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void updateHearthDamage(int dano, Global.typeOfPlayer type) {
        currentHealth -= dano;
        updateHealthShow();
    }

    private void updateCurrentCoins() {
        int valueHundred = currentCoin / 100;
        int valueTen = (currentCoin % 100) / 10;
        int valueUnity = currentCoin % 10;

        setNumber(hundred, valueHundred);
        setNumber(ten, valueTen);
        setNumber(unity, valueUnity);
    }

    private void setNumber(Image imgHolder, int value) {
        switch (value) {
            case 1:
                imgHolder.sprite = s_numero_1;
                return;
            case 2:
                imgHolder.sprite = s_numero_2;
                return;
            case 3:
                imgHolder.sprite = s_numero_3;
                return;
            case 4:
                imgHolder.sprite = s_numero_4;
                return;
            case 5:
                imgHolder.sprite = s_numero_5;
                return;
            case 6:
                imgHolder.sprite = s_numero_6;
                return;
            case 7:
                imgHolder.sprite = s_numero_7;
                return;
            case 8:
                imgHolder.sprite = s_numero_8;
                return;
            case 9:
                imgHolder.sprite = s_numero_9;
                return;
            default:
                imgHolder.sprite = s_numero_0;
                return;
        }
    }

    private void updateHealthShow() {
        int remaining = 0 ;
        int fullHearths = 0;
        int emptyHearths = 0;

        if (hearths > 10) {
            hearths = 10;
        }
        if (hearths < 1) {
            hearths = 1;
        }

        if (currentHealth < 0) {
            currentHealth = 0;
        }
        if(currentHealth > hearths*2) {
            currentHealth = hearths * 2;
        }

        if (currentHealth > 0) {
            fullHearths = currentHealth / 2;
            remaining = currentHealth % 2;
        }

        emptyHearths = (hearths - fullHearths) - remaining;

        if (fullHearths > 0) {
            for (int count = 0; count < fullHearths; count++) {
                listaCoracoes[count].sprite = s_coracao_Cheio;
            }
        }

        if(emptyHearths > 0) {
            for (int count = hearths; count > (hearths - emptyHearths); count--) {
                listaCoracoes[count - 1].sprite = s_coracao_Vazio;
            }
        }

        if(remaining == 1) {
            listaCoracoes[fullHearths].sprite = s_coracao_1_2;
        }
    }
}
