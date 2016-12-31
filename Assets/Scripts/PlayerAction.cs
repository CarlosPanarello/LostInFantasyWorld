using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoverParaDireita : IPlayerAction {
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float moveSpeed, bool isDoing, bool canDo) {
        objectToMove.velocity = new Vector3(moveSpeed, objectToMove.velocity.y, 0f);
        transf.localScale = new Vector3(1f, 1f, 1f);
        CameraController.instance.posicionarCameraAlvo(transf);
    }
    public void playSound(AudioSource sound,bool canPlay) {}
	public void changedPlayer(List<PlayerControl> listOfPlayers,Button buttonNextPlayer, Image imageCurrentPlayer) {}
	public bool changePlayer(List<PlayerControl> listOfPlayers) {return false;}
}

public class MoverParaEsquerda : IPlayerAction {
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float moveSpeed, bool isDoing, bool canDo) {
        objectToMove.velocity = new Vector3(-moveSpeed, objectToMove.velocity.y, 0f);
        transf.localScale = new Vector3(-1f, 1f, 1f);
        CameraController.instance.posicionarCameraAlvo(transf);
    }
	public void playSound(AudioSource sound,bool canPlay) {}
	public void changedPlayer(List<PlayerControl> listOfPlayers,Button buttonNextPlayer, Image imageCurrentPlayer) {}
	public bool changePlayer(List<PlayerControl> listOfPlayers) {return false;}
}

public class ActionJump : IPlayerAction {
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float actionSpeed, bool isDoing, bool canDo) {
        if (canDo) {
            objectToMove.velocity = new Vector3(objectToMove.velocity.x, actionSpeed, 0f);
            CameraController.instance.posicionarCameraAlvo(transf);
        }
    }

	public void playSound(AudioSource sound,bool canPlay) {
		if (!sound.isPlaying && canPlay) {
			sound.Play();
		}
    }

	public void changedPlayer(List<PlayerControl> listOfPlayers,Button buttonNextPlayer, Image imageCurrentPlayer) {}
	public bool changePlayer(List<PlayerControl> listOfPlayers) {return false;}
}

public class ActionIdle : IPlayerAction {
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float actionSpeed, bool isDoing, bool canDo) {
        objectToMove.velocity = new Vector3(0f, objectToMove.velocity.y, 0f);
        CameraController.instance.posicionarCameraAlvo(transf);
    }
	public void playSound(AudioSource sound,bool canPlay) {}
	public void changedPlayer(List<PlayerControl> listOfPlayers,Button buttonNextPlayer, Image imageCurrentPlayer) {}
	public bool changePlayer(List<PlayerControl> listOfPlayers) {return false;}
}

public class ActionEspecial : IPlayerAction {
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float moveSpeed, bool isDoing, bool canDo) {
    }

	public void playSound(AudioSource sound,bool canPlay) {}
	public void changedPlayer(List<PlayerControl> listOfPlayers,Button buttonNextPlayer, Image imageCurrentPlayer) {}
	public bool changePlayer(List<PlayerControl> listOfPlayers) {return false;}
}

public class ActionSwim : IPlayerAction {
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float swimSpeed, bool isDoing, bool canDo) {
       // objectToMove.gravityScale = 0.25f;
		if (canDo) {
			objectToMove.velocity = new Vector3(objectToMove.velocity.x, swimSpeed, 0f);
			CameraController.instance.posicionarCameraAlvo(transf);
		}
    }
    public void playSound(AudioSource sound,bool canPlay) {
        sound.Play();
    }
	public void changedPlayer(List<PlayerControl> listOfPlayers,Button buttonNextPlayer, Image imageCurrentPlayer) {}
	public bool changePlayer(List<PlayerControl> listOfPlayers) {return false;}
}

public class ActionChangePlayer : IPlayerAction {

    private bool isForKill = false;

    public  ActionChangePlayer(bool isForKill) {
        this.isForKill = isForKill;
    }

    public ActionChangePlayer() {}

	public void changedPlayer(List<PlayerControl> listOfPlayers,Button buttonNextPlayer, Image imageCurrentPlayer) {
		int indiceJogadorAtivo = 0;
		PlayerControl nextPlayer = null;
		PlayerControl actualPlayer = null;

		foreach (PlayerControl jogador in listOfPlayers) {
			if (actualPlayer == null && jogador.canSwitchPlayer(isForKill)) {
				actualPlayer = jogador;
			}

			if (indiceJogadorAtivo >= listOfPlayers.Count - 1) {
				indiceJogadorAtivo = 0;
			} else {
				indiceJogadorAtivo++;
			}

			if(actualPlayer!= null) {
				break;
			}
		}

		for(int cont = 0; cont < listOfPlayers.Count; cont++) {

			if (actualPlayer != null && nextPlayer == null 
				&& listOfPlayers[indiceJogadorAtivo].canBeNextPlayer()) {
				nextPlayer = listOfPlayers[indiceJogadorAtivo];
			}

			if (indiceJogadorAtivo >= listOfPlayers.Count - 1) {
				indiceJogadorAtivo = 0;
			} else {
				indiceJogadorAtivo++;
			}

			if (nextPlayer != null) {
				break;
			}
		}

		if(actualPlayer == null) {
			return;
		} else {
			imageCurrentPlayer.sprite = actualPlayer.spriteBotonFull;
		}

		if(nextPlayer == null) {
			buttonNextPlayer.image.sprite = actualPlayer.spriteBotonCenter;
		} else {
			buttonNextPlayer.image.sprite = nextPlayer.spriteBotonCenter;
		}

	}

    public bool changePlayer(List<PlayerControl> listOfPlayers ) {
        int indiceJogadorAtivo = 0;
        PlayerControl nextPlayer = null;
        PlayerControl actualPlayer = null;

        foreach (PlayerControl jogador in listOfPlayers) {
            if (actualPlayer == null && jogador.canSwitchPlayer(isForKill)) {
                actualPlayer = jogador;
            }

            if (indiceJogadorAtivo >= listOfPlayers.Count - 1) {
                indiceJogadorAtivo = 0;
            } else {
                indiceJogadorAtivo++;
            }

            if(actualPlayer!= null) {
                break;
            }
        }

        for(int cont = 0; cont < listOfPlayers.Count; cont++) {

            if (actualPlayer != null && nextPlayer == null 
                && listOfPlayers[indiceJogadorAtivo].canBeNextPlayer()) {
                nextPlayer = listOfPlayers[indiceJogadorAtivo];
            }

            if (indiceJogadorAtivo >= listOfPlayers.Count - 1) {
                indiceJogadorAtivo = 0;
            } else {
                indiceJogadorAtivo++;
            }

            if (nextPlayer != null) {
                break;
            }
        }

        if (actualPlayer != null && nextPlayer != null) {
            actualPlayer.desativarJogador();
            nextPlayer.ativarJogador();
            return true;
        } else {
            return false;
        }
    }

    public void playerAction(Rigidbody2D objectToMove, Transform transf, float actionSpeed, bool isDoing,bool canDo) {}
	public void playSound(AudioSource sound,bool canPlay) {}
}