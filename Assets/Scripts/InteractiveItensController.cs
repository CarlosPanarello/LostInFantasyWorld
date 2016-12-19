using UnityEngine;
using System.Collections;

public class InteractiveItensController : MonoBehaviour {

    public delegate void ItemActiveByPlayer(bool actived,
        Global.typeOfPlayer player,
        Global.typeOfPlayer playerThatCanActived,
        Global.InteractiveItem item,
        Vector3 positionOfItem);

    public static event ItemActiveByPlayer OnItemActivedByPlayer;

    public bool isItemActive;
    public bool CanBeDesactived;

    public Global.typeOfPlayer typeOfPlayerCanActived;
    public Global.InteractiveItem typeOfInteractiveItem;

    private Animator animation;
    // Use this for initialization
    void Start() {
        animation = GetComponent<Animator>();
        animation.SetBool("ativo", isItemActive);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D other) {
        bool actionActive = true;
        if (OnItemActivedByPlayer != null) {
            if (!Global.getTypeOfPlayerByTag(other.tag).Equals(Global.typeOfPlayer.Player_None)) {
                if (CanBeDesactived) {
                    isItemActive = !isItemActive;
                } else {
                    isItemActive = actionActive;
                }
                animation.SetBool("ativo", isItemActive);
                //Trocar esses dois Enum por um bool
                OnItemActivedByPlayer(isItemActive, Global.getTypeOfPlayerByTag(other.tag), typeOfPlayerCanActived, typeOfInteractiveItem,transform.position);
            }
        }
    }
}
