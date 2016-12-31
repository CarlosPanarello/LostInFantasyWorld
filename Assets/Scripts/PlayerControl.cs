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
    private SpriteRenderer[] sprites;
    public bool executandoDano;
    private bool jogadorParando;
    
    public float knockbackForce;
    public float knockbackLength;
    public float knockbackCounter;

    public int contadorInvencibilidade;
    public bool invencivel;

    public float tempoInvencibilidade;
    private float contadorTempoInvencibilidade;

    public Sprite spriteBotonFull;
    public Sprite spriteBotonCenter;
    public Vector3 respawnPosition;
	public Vector3 posicaoAnterior;

    private GameObject plataformaMovelObject;

    void Awake() {
        //Atribuindo uma função qdo o jogador for ferido
        HurtPlayer.OnHurtPlayer += machucarJogador;
        InputController.OnPlayerAction += actionController;
        InteractiveItensController.OnItemActivedByPlayer += itemActived;
        LevelManager.OnKillPlayer += whoToKill;
        LevelManager.OnRespaw += whoToRespawn;
		CollectiablesController.OnItemCollect += itemCatch;
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
                //knockbackAction();
            }
        } 
    }

    // Use this for initialization
    void Start() {
        isAtivo = false;
        meuRigibody = GetComponent<Rigidbody2D>();
        meuAnim = GetComponent<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        respawnPosition = transform.position;
        
        executandoDano = false;
        renascendo = false;
        jogadorParando = true;
        meuRigibody.isKinematic = true;
        inPlataformaMovel = false;
        contadorInvencibilidade = 0;
    }

    public void machucarJogador(int dano, Global.typeOfPlayer type) {
        Debug.Log("Executando o OnHurt no PlayerControl");
        // startKnockbackAction();
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
		} else {
			if(inPlataformaMovel){
				movimentar ();
			}
		}

    }

    private IEnumerator flashSprites(SpriteRenderer[] sprites, int seconds, bool disable = false) {
        // number of times to loop
        for (int loop = 0; loop < seconds*10; loop++) {
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++) {
                if (disable) {
                    // for disabling
                    sprites[i].enabled = false;
                } else {
                    // for changing the alpha
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 0.5f);
                }
            }

            // delay specified amount
            yield return new WaitForSeconds(0.05f);

            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++) {
                if (disable) {
                    // for disabling
                    sprites[i].enabled = true;
                } else {
                    // for changing the alpha
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 1);
                }
            }

            // delay specified amount
            yield return new WaitForSeconds(0.05f);
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
        if ((isSwinning || isGrounded || isLavaGrounded) 
            && !jogadorParando && gameObject.activeSelf) {

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
        StartCoroutine("RespawnCoRoutine");
    }

    public IEnumerator RespawnCoRoutine() {
        killPlayer();
        yield return new WaitForSeconds(LevelManager.instance.tempoEsperaRespawn);
        gameObject.SetActive(true);
        renascendo = false;
    }

    private void killPlayer() {
        isAtivo = false;
        gameObject.SetActive(false);
        Instantiate(LevelManager.instance.explosao, transform.position, transform.rotation);
    }

    private void whoToRespawn(Global.typeOfPlayer player) {
        if (tipo.Equals(player)) {
            Respawn();
        }
    }

    private void whoToKill(Global.typeOfPlayer player) {
        if (tipo.Equals(player)) {
            killPlayer();
        }
    }

    public bool canSwitchPlayer(bool forKill) {
        if (forKill) {
            return isAtivo;
        } else { 
            if (this.gameObject.activeSelf) {
                return (isSwinning || isGrounded || isLavaGrounded) && isAtivo;
            } else {
                return false;
            }
        }
	}

    public bool canBeNextPlayer() {
        if (this.gameObject.activeSelf) {
            return (isSwinning || isGrounded || isLavaGrounded || meuRigibody.isKinematic)  && !isAtivo;
        } else {
            return false;
        }
    }
    private void itemCatch(int valueOfItem, Global.typeOfPlayer player, CollectiablesController item) {
        if (Global.typeOfItem.Star.Equals(item.typeOfItem) && tipo.Equals(player)) {
            Debug.Log("Pegou Estrela-->" + valueOfItem);
            contadorInvencibilidade = valueOfItem;
            Invoke("ContadorInvencibilidade", valueOfItem);
            invencivel = true;
            
            StartCoroutine(flashSprites(sprites, valueOfItem));
        }
    }
    public void ContadorInvencibilidade() {
        Debug.Log("Normal" );
        invencivel = false;
    }
        /*
        private void itemCatch(int valueOfItem, Global.typeOfPlayer player, CollectiablesController item) {
            if (Global.typeOfItem.Star.Equals(item.typeOfItem)) {
                Debug.Log("Pegou Estrela-->" + valueOfItem);
                contadorInvencibilidade = valueOfItem;
                InvokeRepeating("ContadorInvencibilidade", 1f, 1f);
            }
        }

        public void ContadorInvencibilidade() {
            Debug.Log("Tempo Invencibilidade--> " + contadorInvencibilidade);
            contadorInvencibilidade--; 
        }
        */
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
            plataformaMovelObject = outro.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D outro) {
        if (outro.gameObject.tag == "PlataformaMovel" && isAtivo) {
            transform.parent = null;
            inPlataformaMovel = false;
        }
    }

	private void movimentar() {
        float posicaoX = transform.position.x;
        float posicaoPlataformaX = plataformaMovelObject.transform.position.x;
        float offsetX = Math.Abs(posicaoX) - Math.Abs(posicaoPlataformaX);

        float posicaoY = transform.position.y;
        float posicaoPlataformaY = plataformaMovelObject.transform.position.y + 0.815f;
        float offsetY = Math.Abs(posicaoY) - Math.Abs(posicaoPlataformaY);

        if (posicaoX < 0) {
            offsetX = offsetX * -1f;
        }

        if (posicaoY < 0) {
            offsetY = offsetY * -1f;
        }

        this.transform.position = new Vector3(posicaoPlataformaX + offsetX,
            posicaoPlataformaY + offsetY, 0f);
	}

    public bool isDead() {
        return gameObject.activeSelf;
    }
}