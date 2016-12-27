using UnityEngine;
using System.Collections;

public class InteractiveItensController : MonoBehaviour {

    public delegate void ItemActiveByPlayer(bool actived,
        Global.typeOfPlayer player,
        Global.typeOfPlayer playerThatCanActived,
        Global.InteractiveItem item,
        Vector3 positionOfItem);

    public static event ItemActiveByPlayer OnItemActivedByPlayer;

    public bool allPlayersCanCatch;
    public bool isItemActive;
    public bool CanBeDesactived;

    public Global.typeOfPlayer typeOfPlayerCanActived;
    public Global.InteractiveItem typeOfInteractiveItem;

    private Animator animacao;
    // Use this for initialization
    void Start() {
		animacao = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
		animacao.SetBool("ativo", isItemActive);
    }

    private bool checkWhoCanIterateWithItem(Global.typeOfPlayer player) {
        if (allPlayersCanCatch) {
            return true;
        } else {
            return typeOfPlayerCanActived.Equals(player);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (OnItemActivedByPlayer != null) {
            Global.typeOfPlayer player = Global.getTypeOfPlayerByTag(other.tag);
            if (!Global.typeOfPlayer.Player_None.Equals(player)
                    && checkWhoCanIterateWithItem(player)) {

                if (CanBeDesactived) {
                    isItemActive = !isItemActive;
					animacao.SetBool ("ativo", isItemActive);
					//Trocar esses dois Enum por um bool
					OnItemActivedByPlayer (isItemActive, Global.getTypeOfPlayerByTag (other.tag), typeOfPlayerCanActived, typeOfInteractiveItem, transform.position);
                } else {
					if (isItemActive == false) {
						isItemActive = true;
						animacao.SetBool ("ativo", true);
						//Trocar esses dois Enum por um bool
						OnItemActivedByPlayer (true, Global.getTypeOfPlayerByTag (other.tag), typeOfPlayerCanActived, typeOfInteractiveItem, transform.position);
					}
                }
            }
        }
    }
}
