using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreController : MonoBehaviour {

    public delegate void noHealth(Global.typeOfPlayer player);
    public static event noHealth OnHealthGoesToZero;

    public static ScoreController instance;

    private Dictionary<Global.typeOfPlayer, int> maxHealth ;
    private Dictionary<Global.typeOfPlayer, int> currentHealth ;

    private int initialCoins;
    private int currentCoins;
    public int maxHealthOfGreen;
    public int maxHealthOfBege;
    public int maxHealthOfBlue;
    public int maxHealthOfPink;
    public int maxHealthOfYellow;

    private Dictionary<Global.typeOfItem, bool> itens;


    void Awake() {
        MakeInstance();
        //Atribuindo uma função qdo o jogador for ferido
        HurtPlayer.OnHurtPlayer += updateHealth;
        //Atribuindo uma função qdo um item for coletado
        CollectiablesController.OnItemCollect += upadateItemCatch;
        LevelManager.OnRespaw += respawPlayer;
    }

    private void respawPlayer(int coins) {
        initializeHealth();
        initialCoins = coins;
    }

    void MakeInstance() {
        if (instance == null) {
            instance = this;
        }
    }

    // Use this for initialization
    void Start() {
        maxHealth = new Dictionary<Global.typeOfPlayer, int>();
        currentHealth = new Dictionary<Global.typeOfPlayer, int>();
        itens = new Dictionary<Global.typeOfItem, bool>();

        initializeHealth();

        foreach (Global.typeOfItem item in Enum.GetValues(typeof(Global.typeOfItem))) {
            if (!Global.typeOfItem.Item_None.Equals(item)) {
                itens.Add(item, false);
            }
        }
    }

    private void initializeHealth() {

        foreach (Global.typeOfPlayer player in Enum.GetValues(typeof(Global.typeOfPlayer))) {
            switch (player) {
                case Global.typeOfPlayer.Player_Bege:
                    maxHealth.Add(player, maxHealthOfBege);
                    currentHealth.Add(player, maxHealthOfBege);
                    break;
                case Global.typeOfPlayer.Player_Blue:
                    maxHealth.Add(player, maxHealthOfBlue);
                    currentHealth.Add(player, maxHealthOfBlue);
                    break;
                case Global.typeOfPlayer.Player_Pink:
                    maxHealth.Add(player, maxHealthOfPink);
                    currentHealth.Add(player, maxHealthOfPink);
                    break;
                case Global.typeOfPlayer.Player_Green:
                    maxHealth.Add(player, maxHealthOfGreen);
                    currentHealth.Add(player, maxHealthOfGreen);
                    break;
                case Global.typeOfPlayer.Player_Yellow:
                    maxHealth.Add(player, maxHealthOfYellow);
                    currentHealth.Add(player, maxHealthOfYellow);
                    break;
                default:
                    break;
            }
        }
    }

    private void upadateItemCatch(int valueOfItem, Global.typeOfPlayer player, Global.typeOfItem item) {
        switch (item) {
            case Global.typeOfItem.Gem_Blue:
            case Global.typeOfItem.Gem_Green:
            case Global.typeOfItem.Gem_Red:
            case Global.typeOfItem.Gem_Yellow:
            case Global.typeOfItem.Key_Blue:
            case Global.typeOfItem.Key_Green:
            case Global.typeOfItem.Key_Red:
            case Global.typeOfItem.Key_Yellow:
                itens[item] = true;
                break;
            case Global.typeOfItem.Health_empty:
                maxHealth[player] += valueOfItem;
                break;
            case Global.typeOfItem.Health_full:
                currentHealth[player] = maxHealth[player];
                break;
            case Global.typeOfItem.Health_half:
                currentHealth[player] += valueOfItem;
                break;
            case Global.typeOfItem.Gold_Coin:
                currentCoins += valueOfItem;
                return;
            default:
                return;
        }
    }

    private void updateHealth(int damage, Global.typeOfPlayer player) {
        switch (player) {
            case Global.typeOfPlayer.Player_None:
                return;
            default:
                currentHealth[player] -= damage;
                if (currentHealth[player] <= 0) {
                    if (OnHealthGoesToZero != null) {
                        OnHealthGoesToZero(player);
                    }
                }
                return;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
