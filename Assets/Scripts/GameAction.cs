using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : IGameAction {
    public void gameAction(Button botaoAcao, Sprite novaImagemBotao, Global.nameOfLevel level) {
        botaoAcao.image.sprite = novaImagemBotao;
        Time.timeScale = 0f;
    }

    public void playSound(AudioSource sound) {
    }
}

public class QuitGame : IGameAction {
    public void gameAction(Button botaoAcao, Sprite novaImagemBotao, Global.nameOfLevel level) {
        Time.timeScale = 1f;
        //string s = Enum.GetName(typeof(Global.nameOfLevel), level);
        // Application.LoadLevel("MainMenu");
    }

    public void playSound(AudioSource sound) {
    }
}

public class ResumeGame : IGameAction {
    
    public void gameAction(Button botaoAcao, Sprite novaImagemBotao, Global.nameOfLevel level) {
        Time.timeScale = 1f;
        botaoAcao.image.sprite = novaImagemBotao;
    }

    public void playSound(AudioSource sound) {
    }
}

public class StartGame : IGameAction {

    public void gameAction(Button botaoAcao, Sprite novaImagemBotao, Global.nameOfLevel level) {
    }

    public void playSound(AudioSource sound) {
    }
}

public class ConfigGame : IGameAction {

    public void gameAction(Button botaoAcao, Sprite novaImagemBotao, Global.nameOfLevel level) {
    }

    public void playSound(AudioSource sound) {
    }
}