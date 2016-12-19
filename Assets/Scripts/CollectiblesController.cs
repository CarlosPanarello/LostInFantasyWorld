using UnityEngine;
using System.Collections;

public class CollectiablesController : MonoBehaviour {

    public delegate void ItemCollect(int valueOfItem, Global.typeOfPlayer player , Global.typeOfItem item);
    public static event ItemCollect OnItemCollect;

    public Global.typeOfItem item;

    public int valueOfItem;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {

        if (OnItemCollect != null) {
            Global.typeOfPlayer player = Global.getTypeOfPlayerByTag(other.tag);
            //Qualquer tag nao encontrada sera definida como Player_None
            //Assim qualquer outro tipo de jogador ira sofre dano
            switch (Global.getTypeOfPlayerByTag(other.tag)) {
                case Global.typeOfPlayer.Player_None:
                    break;
                default:
                    if (OnItemCollect != null) { 
                        OnItemCollect(valueOfItem, player, item);
                    }
                    break;
            }
            //TODO verificar se como fica o delagate das classes pressas a ele
            //Destroy (gameObject);
            gameObject.SetActive(false);
        }
    }
}
