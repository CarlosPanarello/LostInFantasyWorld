using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoverParaDireita : IPlayerAction {
    public void changedPlayer(Button buttonNextPlayer, Image imageCurrentPlayer) {

    }

    public bool changePlayer(List<PlayerControl> listOfPlayers) {
        return false;
    }
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float moveSpeed, bool isDoing, bool canDo) {
        objectToMove.velocity = new Vector3(moveSpeed, objectToMove.velocity.y, 0f);
        transf.localScale = new Vector3(1f, 1f, 1f);
        CameraController.instance.posicionarCameraAlvo(transf.position);
    }
    public void playSound(AudioSource sound) {
    }
}

public class MoverParaEsquerda : IPlayerAction {
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float moveSpeed, bool isDoing, bool canDo) {
        objectToMove.velocity = new Vector3(-moveSpeed, objectToMove.velocity.y, 0f);
        transf.localScale = new Vector3(-1f, 1f, 1f);
        CameraController.instance.posicionarCameraAlvo(transf.position);
    }
    public void playSound(AudioSource sound) {
    }
    public void changedPlayer(Button buttonNextPlayer, Image imageCurrentPlayer) {

    }
    public bool changePlayer(List<PlayerControl> listOfPlayers) {
        return false;
    }
}

public class ActionJump : IPlayerAction {
    public void changedPlayer( Button buttonNextPlayer, Image imageCurrentPlayer) {

    }

    public bool changePlayer(List<PlayerControl> listOfPlayers) {
        return false;
    }
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float actionSpeed, bool isDoing, bool canDo) {
        if (canDo) {
            objectToMove.velocity = new Vector3(objectToMove.velocity.x, actionSpeed, 0f);
            CameraController.instance.posicionarCameraAlvo(transf.position);
        }
    }
    public void playSound(AudioSource sound) {
        sound.Play();
    }

}

public class ActionIdle : IPlayerAction {
    public void changedPlayer(Button buttonNextPlayer, Image imageCurrentPlayer) {

    }

    public bool changePlayer(List<PlayerControl> listOfPlayers) {
        return false;
    }
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float actionSpeed, bool isDoing, bool canDo) {
        objectToMove.velocity = new Vector3(0f, objectToMove.velocity.y, 0f);
        CameraController.instance.posicionarCameraAlvo(transf.position);
    }
    public void playSound(AudioSource sound) {
        sound.Play();
    }
}

public class ActionEspecial : IPlayerAction {
    public void changedPlayer( Button buttonNextPlayer, Image imageCurrentPlayer) {

    }

    public bool changePlayer(List<PlayerControl> listOfPlayers) {
        return false;
    }
    public void playerAction(Rigidbody2D objectToMove, Transform transf, float moveSpeed, bool isDoing, bool canDo) {
    }

    public void playSound(AudioSource sound) {
    }
}

public class ActionSwim : IPlayerAction {
    public void changedPlayer(Button buttonNextPlayer, Image imageCurrentPlayer) {

    }

    public bool changePlayer(List<PlayerControl> listOfPlayers) {
        return false;
    }

    public void playerAction(Rigidbody2D objectToMove, Transform transf, float moveSpeed, bool isDoing, bool canDo) {
       // objectToMove.gravityScale = 0.25f;
    }
    public void playSound(AudioSource sound) {
        sound.Play();
    }
}

public class ActionChangePlayer : IPlayerAction {
    List<PlayerControl> listOfPlayersInternal;

    public void changedPlayer(Button buttonNextPlayer, Image imageCurrentPlayer) {
        int indiceJogadorAtivo = 0;
        foreach (PlayerControl jogador in listOfPlayersInternal) {
            if (jogador.isAtivo) {
                imageCurrentPlayer.sprite = jogador.spriteBotonFull;
                break;
            }
            indiceJogadorAtivo++;
        }

        if (indiceJogadorAtivo >= listOfPlayersInternal.Count - 1) {
            indiceJogadorAtivo = 0;
        } else {
            indiceJogadorAtivo++;
        }

        buttonNextPlayer.image.sprite = listOfPlayersInternal[indiceJogadorAtivo].spriteBotonCenter;
    }

    public bool changePlayer(List<PlayerControl> listOfPlayers ) {
        listOfPlayersInternal = listOfPlayers;
        int indiceJogadorAtivo = 0;

        foreach (PlayerControl jogador in listOfPlayers) {
            if (jogador.isAtivo) {
                jogador.desativarJogador();
                break;
            }
            indiceJogadorAtivo++;
        }

        if (indiceJogadorAtivo >= listOfPlayers.Count - 1) {
            indiceJogadorAtivo = 0;
        } else {
            indiceJogadorAtivo++;
        }

        listOfPlayers[indiceJogadorAtivo].ativarJogador();
        return true;
    }

    public void playerAction(Rigidbody2D objectToMove, Transform transf, float actionSpeed, bool isDoing,bool canDo) {
       
    }

    public void playSound(AudioSource sound) {
    }
}



