using UnityEngine;
using System.Collections;
using System;

public class SoundController : MonoBehaviour {

    public AudioSource hurtGreen;
    public AudioSource hurtYellow;
    public AudioSource hurtPink;
    public AudioSource hurtBege;
    public AudioSource hurtBlue;
    public AudioSource catchGem;
    public AudioSource catchKey;
    public AudioSource catchGoldCoin;

    public AudioSource playerDied;
    public AudioSource playerJumpGreen;
    public AudioSource playerJumpYellow;
    public AudioSource playerJumpPink;
    public AudioSource playerJumpBege;
    public AudioSource playerJumpBlue;
    public AudioSource playerSwim;
    public AudioSource interactiveItemOn;
    public AudioSource interactiveItemOff;

    public AudioSource musicLevel;
    public AudioSource musicGameOver;

    void Awake() {
        HurtPlayer.OnHurtPlayer += soundPlayerHurt;
        CollectiablesController.OnItemCollect += soundItemCatch;
        InputController.OnPlayerAction += actionController;
        InteractiveItensController.OnItemActivedByPlayer += soundItemActived;
    }

    private void soundItemActived(bool actived, Global.typeOfPlayer player, Global.typeOfPlayer playerThatCanActived, Global.InteractiveItem item,Vector3 position) {
        if (player.Equals(playerThatCanActived)) {
            if (actived) {
                interactiveItemOn.Play();
            } else {
                interactiveItemOff.Play();
            }
        }
    }

    private void actionController(IPlayerAction action) {
		if (action.GetType () == typeof(ActionJump)) {
			PlayerControl player = LevelManager.instance.getActivePlayer ();

			switch (player.tipo) {
			case Global.typeOfPlayer.Player_Bege:
				action.playSound (playerJumpBege,player.isGrounded);
				break;
			case Global.typeOfPlayer.Player_Blue:
				action.playSound (playerJumpBlue,player.isGrounded);
				break;
			case Global.typeOfPlayer.Player_Green:
				action.playSound (playerJumpGreen,player.isGrounded);
				break;
			case Global.typeOfPlayer.Player_Pink:
				action.playSound (playerJumpPink,player.isGrounded);
				break;
			case Global.typeOfPlayer.Player_Yellow:
				action.playSound (playerJumpYellow,player.isGrounded);
				break;
			default:
				break;
			}
		}
    }

    // Use this for initialization
    void Start () {
		musicLevel.loop = true;
		//musicLevel.Play();

    }

    private void soundItemCatch(int valueOfItem,
        Global.typeOfPlayer player,
        CollectiablesController item) {
        switch (item.typeOfItem) {
            case Global.typeOfItem.Gem_Blue:
            case Global.typeOfItem.Gem_Green:
            case Global.typeOfItem.Gem_Red:
            case Global.typeOfItem.Gem_Yellow:
                catchGem.Play();
                break;
            case Global.typeOfItem.Key_Blue:
            case Global.typeOfItem.Key_Green:
            case Global.typeOfItem.Key_Red:
            case Global.typeOfItem.Key_Yellow:
                catchKey.Play();
                break;
            case Global.typeOfItem.Gold_Coin:
                catchGoldCoin.Play();
                break;
            default:
                break;
        }
    }
    private void soundPlayerHurt(int damage, Global.typeOfPlayer player) {
        switch (player) {
            case Global.typeOfPlayer.Player_Bege:
                hurtBege.Play();
                break;
            case Global.typeOfPlayer.Player_Blue:
                hurtBlue.Play();
                break;
            case Global.typeOfPlayer.Player_Green:
                hurtGreen.Play();
                break;
            case Global.typeOfPlayer.Player_Pink:
                hurtPink.Play();
                break;
            case Global.typeOfPlayer.Player_Yellow:
                hurtYellow.Play();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void muteAll(bool mute) {
        muteSoundEffect(mute);
        muteSoundMusic(mute);
    }

    public void muteSoundEffect(bool mute) {
        if (mute) {
            setVolumeSoundEffect(0);
        }
    }

    public void muteSoundMusic(bool mute) {
        if (mute) {
            setVolumeMusic(0);
        }
    }

    public void setVolumeSoundEffect(int volume) {

    }

    public void setVolumeMusic(int volume) {

    }
}

