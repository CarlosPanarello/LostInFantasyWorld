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

    public bool renascendo;

    private bool actionSwin = false;
    private Animator meuAnim;
    private Rigidbody2D meuRigibody;
    public bool executandoDano;
    private bool jogadorParando;
    
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
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LevelManager.instance.whatIsGround);
                isLavaGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LevelManager.instance.whatIsLava);

                if (isLavaGrounded) {
                    if (canWalkInLava) {
                        isGrounded = true;
                    } else {
                        isGrounded = false;
                    }
                }

				if (action.GetType () == typeof(ActionSwim)) {
					action.playerAction (meuRigibody, transform, swimSpeed, true, isSwinning);										
				} else {
					if (action.GetType () == typeof(ActionJump)) {
						action.playerAction (meuRigibody, transform, jumpSpeed, true, isGrounded);
					} else {
						action.playerAction (meuRigibody, transform, moveSpeed, true, isGrounded);
					}
				}

                invencivel = false;
            } else {
                knockbackAction();
            }
        } 
    }

    // Use this for initialization
    void Start() {
        isAtivo = false;
        meuRigibody = GetComponent<Rigidbody2D>();
        meuAnim = GetComponent<Animator>();
        respawnPosition = transform.position;
        
        executandoDano = false;
        renascendo = false;
        jogadorParando = true;
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
		if (isAtivo) {
			isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, LevelManager.instance.whatIsGround);
			isSwinning = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, LevelManager.instance.whatIsWater);
			isLavaGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, LevelManager.instance.whatIsLava);

			if (isSwinning) {
				if (canSwim) {
					meuRigibody.gravityScale = 0.25f;
				} else {
					//Ferir(1);
				}
			} else {
				meuRigibody.gravityScale = 2.5f;
			}

			isLavaGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, LevelManager.instance.whatIsLava);
			if (isLavaGrounded) {
				if (canWalkInLava) {
					isGrounded = true;
				} else {
					//Ferir(1);
				}
			}

			if (contadorTempoInvencibilidade <= 0) {
				invencivel = false;
			} else {
				contadorTempoInvencibilidade -= Time.deltaTime;
			}


			if (meuRigibody.velocity.y < 0) {
				stompBox.SetActive (true);
			} else {
				if (stompBox != null) {
					stompBox.SetActive (false);
				}
			}

			meuAnim.SetBool ("Hurting", executandoDano);
			meuAnim.SetFloat ("Speed", Mathf.Abs (meuRigibody.velocity.x));
			meuAnim.SetBool ("Grounded", isGrounded);
			meuAnim.SetBool ("Swimming", isSwinning);
		}
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
        if ((isSwinning || isGrounded || isLavaGrounded) && !jogadorParando && !inPlataformaMovel) {
			meuRigibody.velocity = Vector3.zero;
			StartCoroutine("PararJogador");
        }
    }

    private IEnumerator PararJogador() {
		jogadorParando = true;
        yield return new WaitForSeconds(0.05f);
        meuRigibody.isKinematic = true;
    }

    public void ativarJogador() {
        isAtivo = true;
        jogadorParando = false;
        meuRigibody.isKinematic = false;
    }

    public void desativarJogador() {
        isAtivo = false;
		congelarJogador();
    }

    public void Respawn() {
        //TIpo Thread
        StartCoroutine("RespawnCoRoutine");
    }

    public IEnumerator RespawnCoRoutine() {

        gameObject.SetActive(false);

        // Não funciona o objeto ja esta foi criado.
        //explosao.GetComponent<ParticleSystem>().startColor = jogador_ativo.obterCorParaExplosaoMorte();

        Instantiate(LevelManager.instance.explosao, transform.position, transform.rotation);

        yield return new WaitForSeconds(LevelManager.instance.tempoEsperaRespawn);

        //jogadorRespaw.Respawing(vidaMaxima);

        gameObject.SetActive(true);

        renascendo = false;

        //UpdateCoracao ();

        // Resetando as moedas e iniciando objetos;
        /*
        foreach (ResetOnRespawn objeto in objetosParaResetar) {
            objeto.gameObject.SetActive(true);
            objeto.ResetObject();
        }
        */
    }

	public bool canSwitchPlayer(){
		return (isSwinning || isGrounded || isLavaGrounded) && isAtivo;
	}

    void OnTriggerEnter2D(Collider2D outro) {
        if (isAtivo) {
            if (outro.tag == "KillPlane") {
                //gameObject.SetActive (false);
                //transform.position = respawnPosition;
                Respawn();
            }
        }

		if (outro.tag == "Water") {
			meuRigibody.gravityScale = 0.25f;
		}
    }

	void OnTriggerExit2D(Collider2D outro){
		if (outro.tag == "Water") {
			meuRigibody.gravityScale = 2.5f;
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