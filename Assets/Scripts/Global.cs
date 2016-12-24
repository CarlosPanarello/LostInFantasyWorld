using System.ComponentModel;
using System.Linq;


public class Global {

    public enum situationOfPlayer {
        Disable,
        Actived,
        Desactived
    }

    public enum nameOfLevel {
        MainMenu,
        Map,
        ConfigMenu,
        Level_1
    }

    public enum InteractiveItem {
        CheckPoint,
        Switch,
        Button,
        Tourch,
        Lamp
    }

    public enum typeOfPlayer {
        Player_None,
        Player_Green,
        Player_Yellow,
        Player_Pink,
        Player_Bege,
        Player_Blue
    };

    public enum typeOfCanHurt {
        None,
        Enemy,
        Static_Danger,
        Moving_Danger,
        Lava,
        Water
    }
    
    public enum typeOfItem {
        Key_Yellow,
        Key_Green,
        Key_Red,
        Key_Blue,
        Gem_Yellow,
        Gem_Green,
        Gem_Red,
        Gem_Blue,
        Health_full,
        Health_half,
        Health_empty,
        Gold_Coin,
        Item_None
    }

    public enum typeOfMovementEnemy {
        Perseguir,
        Patrulha,
        Ir_Direita,
        Ir_Esquerda
    };

    public static typeOfPlayer getTypeOfPlayerByTag(string valor) {
        typeOfPlayer t = System.Enum.GetValues(typeof(typeOfPlayer))
                .Cast<typeOfPlayer>()
                .FirstOrDefault(v => v.ToString() == valor);
        return t;
    }

    public static typeOfCanHurt getTypeOfThingsThatCanHurtPlayer(string valor) {
        typeOfCanHurt t = System.Enum.GetValues(typeof(typeOfCanHurt))
                .Cast<typeOfCanHurt>()
                .FirstOrDefault(v => v.ToString() == valor);
        return t;
    }
}