using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System;

public class PlayerControl : MonoBehaviour {

    public float moveSpeed;
    public float jumpSpeed;
    public float swimSpeed;
    public Global.typeOfPlayer tipo;

    public bool isAtivo;

    public Transform groundCheck;
    public float groundCheckRadius;

    public bool isGrounded;
    public bool isSwinning;
    public bool isLavaGrounded;
    public bool inPlataformaMovel;

    public bool canSwim;
    public bool canWalkInLava;

    public GameObject stompBox;

    public LevelManager level;

    public AudioSource ferirSom;
    public AudioSource jumpSound;

    public bool renascendo;

    private bool actionSwin = false;
    private Animator meuAnim;
    private Rigidbody2D meuRigibody;
    public bool executandoDano;
    private bool jogadorParado;
    
    public float knockbackForce;
    public float knockbackLength;
    public float knockbackCounter;

    public bool invencivel;

    public float tempoInvencibilidade;
    private float contadorTempoInvencibilidade;

    public Sprite spriteBotonFull;
    public Sprite spriteBotonCenter;
    public Vector3 respawnPosition;

    void Awake() {
        //Atribuindo uma função qdo o jogador for ferido
        HurtPlayer.OnHurtPlayer += machucarJogador;
        InputController.OnPlayerAction += actionController;
        InteractiveItensController.OnItemActivedByPlayer += itemActived;
    }

    private void itemActived(bool actived, Global.typeOfPlayer player,
        Global.typeOfPlayer playerThatCanActived,
        Global.InteractiveItem item, Vector3 positionOfItem) {
        if(tipo.Equals(playerThatCanActived) && actived && Global.InteractiveItem.CheckPoint.Equals(item)) {
            respawnPosition = positionOfItem;
        }
    }

    private void actionController(IPlayerAction action) {
        if (isAtivo) {
            if (knockbackCounter <= 0) {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, level.whatIsGround);
                isLavaGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, level.whatIsLava);

                if (isLavaGrounded) {
                    if (canWalkInLava) {
                        isGrounded = true;
                    } else {
                        isGrounded = false;
                    }
                }

                action.playerAction(meuRigibody, transform, moveSpeed, true, isGrounded);
                invencivel = false;
            } else {
                knockbackAction();
            }
        } else {
            congelarJogador();
        }
    }

    // Use this for initialization
    void Start() {
        //moveSpeed = 1f;
        isAtivo = false;
        meuRigibody = GetComponent<Rigidbody2D>();
        meuAnim = GetComponent<Animator>();
        respawnPosition = transform.position;
        level = FindObjectOfType<LevelManager>();
        executandoDano = false;
        renascendo = false;
        jogadorParado = true;
        meuRigibody.isKinematic = true;
        inPlataformaMovel = false;
    }

    public void machucarJogador(int dano, Global.typeOfPlayer type) {
        startKnockbackAction();
    }

    void FixedUpdate() {
        if (actionSwin && isAtivo) {
            actionSwin = false;
            meuRigibody.velocity = new Vector2(0, swimSpeed);
        }
    }

    public void Respawing() {
        executandoDano = false;
        transform.position = respawnPosition;
    }

    // Update is called once per frame
    void Update() {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, level.whatIsGround);
        isSwinning = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, level.whatIsWater);
        isLavaGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, level.whatIsLava);

        if (isSwinning) {
            if (canSwim) {
                meuRigibody.gravityScale = 0.25f;
            } else {
                //Ferir(1);
            }
        } else {
            meuRigibody.gravityScale = 2.5f;
        }

        isLavaGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, level.whatIsLava);
        if (isLavaGrounded) {
            if (canWalkInLava) {
                isGrounded = true;
            } else {
                //Ferir(1);
            }
        }

        if(contadorTempoInvencibilidade <= 0) {
            invencivel = false;
        } else {
            contadorTempoInvencibilidade -= Time.deltaTime;
        }


        if (meuRigibody.velocity.y < 0) {
            stompBox.SetActive(true);
        } else {
            if (stompBox != null) {
                stompBox.SetActive(false);
            }
        }

        meuAnim.SetBool("Hurting", executandoDano);
        meuAnim.SetFloat("Speed", Mathf.Abs(meuRigibody.velocity.x));
        meuAnim.SetBool("Grounded", isGrounded);
        meuAnim.SetBool("Swimming", isSwinning);
    }

    public void startKnockbackAction() {
        knockbackCounter = knockbackLength;
        invencivel = true;
    }

    public void knockbackAction() {
        knockbackCounter -= Time.deltaTime;

        if (transform.localScale.x > 0) {
            meuRigibody.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
        } else {
            meuRigibody.velocity = new Vector3(knockbackForce, knockbackForce, 0f);
        }
    }

    private void congelarJogador() {
        if ((isSwinning || isGrounded || isLavaGrounded) && !jogadorParado && !inPlataformaMovel) {
            StartCoroutine("PararJogador");
        }
    }

    private IEnumerator PararJogador() {
        jogadorParado = true;
        yield return new WaitForSeconds(0.05f);
        meuRigibody.isKinematic = true;
    }


    public void ativarJogador() {
        isAtivo = true;
        jogadorParado = false;
        meuRigibody.isKinematic = false;
    }

    public void desativarJogador() {
        isAtivo = false;
        congelarJogador();
    }

    void OnTriggerEnter2D(Collider2D outro) {
        if (isAtivo) {
            if (outro.tag == "KillPlane") {
                //gameObject.SetActive (false);
                //transform.position = respawnPosition;
                level.Respawn(this);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D outro) {
        if (outro.gameObject.tag == "PlataformaMovel") {
            transform.parent = outro.transform;
            inPlataformaMovel = true;
        }
    }

    void OnCollisionExit2D(Collision2D outro) {
        if (outro.gameObject.tag == "PlataformaMovel") {
            transform.parent = null;
            inPlataformaMovel = false;
        }
    }
}
