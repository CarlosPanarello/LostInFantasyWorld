using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class PlayerControl : MonoBehaviour {

    public float moveSpeed;
    public float jumpSpeed;
    public float swimSpeed;
    public Global.tipoPlayer tipo;
        
    public int contadorVida;

    public bool isAtivo;

    public Transform groundCheck;
    public float groundCheckRadius;

    public bool isGrounded;
    public bool isSwinning;
    public bool isLavaGrounded;
    public bool inPlataformaMovel;

    public bool canSwim;
    public bool canWalkInLava;

    public Vector3 respawnPosition;

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

    // Use this for initialization
    void Start () {
		//moveSpeed = 1f;
		isAtivo = false;
		meuRigibody = GetComponent<Rigidbody2D>();
		meuAnim = GetComponent<Animator> ();
		respawnPosition = transform.position;
		level = FindObjectOfType<LevelManager> ();
        executandoDano = false;
        renascendo = false;
        jogadorParado = true;
        meuRigibody.isKinematic = true;
        inPlataformaMovel = false;
     }

	void FixedUpdate(){
    	if (actionSwin && isAtivo) {
			actionSwin = false;
			meuRigibody.velocity = new Vector2 (0, swimSpeed);
		}
	}

    public void Respawing(int vidaMaxima) {
        executandoDano = false;
        contadorVida = vidaMaxima;
        transform.position = respawnPosition;
    }

    public void HurtPlayer(int dano) {
        if (!executandoDano) {
            StartCoroutine("ResetarAnimHurt", dano);
        }
    }

    private IEnumerator ResetarAnimHurt(int dano) {
        executandoDano = true;
        contadorVida -= dano;
        ferirSom.Play();
        yield return new WaitForSeconds(0.5f);
        executandoDano = false;
    }

    // Update is called once per frame
    void Update () {
		
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, level.whatIsGround);
		isSwinning = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, level.whatIsWater);
        isLavaGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, level.whatIsLava);
          
        if (isSwinning) {
            if (canSwim) {
                meuRigibody.gravityScale = 0.25f;
            } else {
                HurtPlayer(1);
            }
		} else {
			meuRigibody.gravityScale = 2.5f;
		}

        if (isLavaGrounded) {
            if (canWalkInLava) {
                isGrounded = true;
            } else {
                HurtPlayer(1);
            }
        }

        if (isAtivo) {
            if (Input.GetButtonDown("Fire1") && isSwinning && canSwim) {
                actionSwin = true;
            }

            if (Input.GetAxisRaw("Horizontal") > 0f) {
                meuRigibody.velocity = new Vector3(moveSpeed, meuRigibody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
            } else if (Input.GetAxisRaw("Horizontal") < 0f) {
                meuRigibody.velocity = new Vector3(-moveSpeed, meuRigibody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else {
                meuRigibody.velocity = new Vector3(0f, meuRigibody.velocity.y, 0f);
            }

            if (Input.GetButtonDown("Jump") && isGrounded) {
                meuRigibody.velocity = new Vector3(meuRigibody.velocity.x, jumpSpeed, 0f);
                jumpSound.Play();
            }
        } else {
            // Se nao tiver ativo ele pode estar parado no ar 
            // entao devo deixar ele cair ate encontrar alguma coisa.
            congelarJogador();
        }

        if(meuRigibody.velocity.y < 0 ) {
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
        //meuRigibody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void desativarJogador() {
        isAtivo = false;
        congelarJogador();
        //meuRigibody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

	void onTriggerExit2D(Collider2D outro){

	}

	void OnTriggerEnter2D(Collider2D outro){
		if (isAtivo) {
			if (outro.tag == "KillPlane") {
				//gameObject.SetActive (false);
				//transform.position = respawnPosition;
				level.Respawn (this);
			}

			if (outro.tag == "CheckPoint") {
                CheckPointControl chk = outro.gameObject.GetComponent<CheckPointControl>();
                if (chk.tipoPlayer.Equals(tipo)) {
                    respawnPosition = outro.transform.position;
                }
			}
		}
	}

	void OnCollisionEnter2D(Collision2D outro){
		if (outro.gameObject.tag == "PlataformaMovel") {
			transform.parent = outro.transform;
            inPlataformaMovel = true;
        }
    }

	void OnCollisionExit2D(Collision2D outro){
		if (outro.gameObject.tag == "PlataformaMovel") {
			transform.parent = null;
            inPlataformaMovel = false;
        }
	}

    public Color obterCorParaExplosaoMorte() {
        switch (tipo) {
            case Global.tipoPlayer.Player_Green: 
                return new Color(139f, 207f, 186f, 255f);
            case Global.tipoPlayer.Player_Bege:
                return new Color(224f, 209f, 175f, 255f);
            case Global.tipoPlayer.Player_Blue:
                return new Color(141f, 181f, 231f, 255f);
            case Global.tipoPlayer.Player_Pink:
                return new Color(241f, 156f, 183f, 255f);
            case Global.tipoPlayer.Player_Yellow:
                return new Color(255f, 204f, 0f, 255f);
            default:
                return new Color(227f, 48f, 48f, 255f);
        }
    }
}
