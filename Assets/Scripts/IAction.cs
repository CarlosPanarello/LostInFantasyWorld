using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public interface IPlayerAction {
    void playerAction(Rigidbody2D objectToMove,
        Transform transf, float actionSpeed,bool isDoing, bool canDo);
    void playSound(AudioSource sound);

    bool changePlayer(List<PlayerControl> listOfPlayers);
    void changedPlayer(Button buttonNextPlayer, Image imageCurrentPlayer);
}

public interface IGameAction {
    void gameAction(Button botaoAcao,Sprite novaImagemBotao,Global.nameOfLevel level);
    void playSound(AudioSource sound);
}
