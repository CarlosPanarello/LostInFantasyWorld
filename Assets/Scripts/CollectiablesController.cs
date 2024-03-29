﻿using UnityEngine;
using System.Collections;
using System;

public class CollectiablesController : MonoBehaviour {

    public delegate void ItemCollect(int valueOfItem, 
        Global.typeOfPlayer player ,
        CollectiablesController item);
    public static event ItemCollect OnItemCollect;

    public Global.typeOfItem typeOfItem;
    public Global.typeOfPlayer typeOfPlayerThatCanCatchItem;
    
    public int valueOfItem;

    public bool allPlayersCanCatch;
    public bool canRespaw;
    
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startLocalScale;
    private Rigidbody2D body;
    

    void Awake() {
        //TODO colocar um alerta para qdo for respaw
        LevelManager.OnRespaw += respawCollectibles;
    }

    private bool checkWhoCanCatchItem(Global.typeOfPlayer player) {
        if (allPlayersCanCatch) {
            return true;
        } else {
            return typeOfPlayerThatCanCatchItem.Equals(player);
        }
    }

    private void respawCollectibles( Global.typeOfPlayer player) {
        if (!this.gameObject.activeSelf && canRespaw 
            && typeOfPlayerThatCanCatchItem.Equals(player) ) {
            this.gameObject.SetActive(true);
            this.ResetObject();
        }
    }

    // Use this for initialization
    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startLocalScale = transform.localScale;

        if (GetComponent<Rigidbody2D>() != null) {
            body = GetComponent<Rigidbody2D>();
        }
    }

    public void ResetObject() {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startLocalScale;

        if (body != null) {
            body.velocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {

        if (OnItemCollect != null) {
            Global.typeOfPlayer player = Global.getTypeOfPlayerByTag(other.tag);
            //Qualquer tag nao encontrada sera definida como Player_None
            //Tem q ser um Player e tem que estar apto a pegar o item
            if (!Global.typeOfPlayer.Player_None.Equals(player) 
                    && checkWhoCanCatchItem(player)) {

                typeOfPlayerThatCanCatchItem = player;
                OnItemCollect(valueOfItem, player, this);
                //TODO verificar se como fica o delagate das classes pressas a ele
                //Destroy (gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}