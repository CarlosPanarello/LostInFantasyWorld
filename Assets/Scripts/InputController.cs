using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public delegate void GameActions(IGameAction action);
    public static event GameActions OnGameAction;

    public delegate void PlayersActions(IPlayerAction action);
    public static event PlayersActions OnPlayerAction;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        //Selecão do jogador
        if (Input.GetButtonDown("Fire3")) {
            changePlayer();
        }

        //Acao Especial (Nadar,Atirar, Bomba etc)
        if (Input.GetButtonDown("Fire1")) {
            especial();
        }

        // movimentar para direita
        if (Input.GetAxisRaw("Horizontal") > 0f) {
            toRight();
        } else // movimentar para esquerda
            if (Input.GetAxisRaw("Horizontal") < 0f) {
                toLeft();
            } else {
                toIdle();

        }
        //Pulo
        if (Input.GetButtonDown("Jump")) {
            jump();
        }

        if (Input.GetButton("Pause")) {
            pause();
        }
    }

    public void OnRealeaseRightOrLeftButton() {
        toIdle();
    }

    public void toRight() {
        OnPlayerAction(new MoverParaDireita());
    }

    public void toLeft() {
        OnPlayerAction(new MoverParaEsquerda());
    }

    public void toIdle() {
        OnPlayerAction(new ActionIdle());
    }

    public void jump() {
        OnPlayerAction(new ActionJump());
    }

    public void especial() {
        OnPlayerAction(new ActionEspecial());
        OnPlayerAction(new ActionSwim());
    }

    public void changePlayer() {
        OnPlayerAction(new ActionChangePlayer());
    }

    public void pause() {
        OnGameAction(new PauseGame());
    }

    public void resume() {
        OnGameAction(new ResumeGame());
    }

    public void start() {
        OnGameAction(new StartGame());
    }

    public void quit() {
        OnGameAction(new QuitGame());
    }

    public void config() {
        OnGameAction(new ConfigGame());
    }
}
