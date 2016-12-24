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
        if (OnPlayerAction != null) {
            OnPlayerAction(new MoverParaDireita());
        }
    }

    public void toLeft() {
        if (OnPlayerAction != null) {
            OnPlayerAction(new MoverParaEsquerda());
        }
    }

    public void toIdle() {
        if (OnPlayerAction != null) {
            OnPlayerAction(new ActionIdle());
        }
    }

    public void jump() {
        if (OnPlayerAction != null) {
            OnPlayerAction(new ActionJump());
        }
    }

    public void especial() {
        if (OnPlayerAction != null) {
            OnPlayerAction(new ActionEspecial());
            OnPlayerAction(new ActionSwim());
        }
    }

    public void changePlayer() {
        if (OnPlayerAction != null) {
            OnPlayerAction(new ActionChangePlayer());
        }
    }

    public void pause() {
        if (OnGameAction != null) {
            OnGameAction(new PauseGame());
        }
    }

    public void resume() {
        if (OnGameAction != null) {
            OnGameAction(new ResumeGame());
        }
    }

    public void start() {
        if (OnGameAction != null) {
            OnGameAction(new StartGame());
        }
    }

    public void quit() {
        if (OnGameAction != null) {
            OnGameAction(new QuitGame());
        }
    }

    public void config() {
        if (OnGameAction != null) {
            OnGameAction(new ConfigGame());
        }
    }
}
