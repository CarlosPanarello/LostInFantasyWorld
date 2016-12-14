using System.ComponentModel;
using System.Linq;


public class Global {

    public enum tipoPlayer {
        Player_None,
        Player_Green,
        Player_Yellow,
        Player_Pink,
        Player_Bege,
        Player_Blue
    };

    public enum tipoMovimentacaoInimigo {
        Perseguir,
        Patrulha,
        IrDireita,
        IrEsquerda
    };

    public static tipoPlayer obterPorDescricao(string valor) {
        tipoPlayer t = System.Enum.GetValues(typeof(tipoPlayer))
                .Cast<tipoPlayer>()
                .FirstOrDefault(v => v.ToString() == valor);
        return t;
    }
}