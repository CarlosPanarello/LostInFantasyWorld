using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class SpiderControl : MonoBehaviour {

    public float moveSpeed;
    public Global.typeOfMovementEnemy tipoMovimento;

    public Transform fim;
	public Transform inicio;
    
    public LevelManager level;

    private bool ativo;
	private bool hit;
	private bool isVisible;
    private Rigidbody2D meuRigibody;
    private Animator animacao;

    private Vector3 pontoFinal;
    private Vector3 pontoInicio;
	private Vector3 alvoAtualFixo;

    // Use this for initialization
    void Start() {
        meuRigibody = GetComponent<Rigidbody2D>();
        animacao = GetComponent<Animator>();
        
		ativo = false;
		isVisible = false; 
		hit = false;

		pontoInicio = new Vector3(inicio.position.x, transform.position.y, transform.position.z);
        pontoFinal = new Vector3(fim.position.x, transform.position.y, transform.position.z);
        alvoAtualFixo = pontoFinal;
    }

    private void movimentarModoPatrulha() {
        if (ativo) {
            if (transform.position.x <= pontoFinal.x && meuRigibody.velocity.x < 0) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                alvoAtualFixo = pontoInicio;
            }

            if (transform.position.x >= pontoInicio.x && meuRigibody.velocity.x > 0) {
                transform.localScale = new Vector3(1f, 1f, 1f);
                alvoAtualFixo = pontoFinal;
            }

            if (alvoAtualFixo.x < transform.position.x) {
                meuRigibody.velocity = new Vector3(-moveSpeed, meuRigibody.velocity.y, 0f);
                //Vector3.MoveTowards (transform.position, alvoAtual, moveSpeed * Time.deltaTime);	
            } else {
                meuRigibody.velocity = new Vector3(moveSpeed, meuRigibody.velocity.y, 0f);
            }
        }
    }

    private void movimentarModePersiguicao() {
		Vector3 alvoAtualMovel = level.posicaoJogadorAtivo ();

        // o Player esta na mesma posicao do inimigo no eixo x
        float dif = Mathf.Abs(alvoAtualMovel.x - transform.position.x);
        ativo = !(dif > 0 && dif < 1) ;

        if (ativo) {
            // o Player esta a direita
            if ((alvoAtualMovel.x - transform.position.x) > 0) {
                // vai realizar a transformacao se ele nao estiver na posicao correta
                if (transform.localScale.x > 0) {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                meuRigibody.velocity = new Vector3(moveSpeed, meuRigibody.velocity.y, 0f);
            }

            // o Player esta a esquerda
            if ((alvoAtualMovel.x - transform.position.x) < 0) {
                // vai realizar a transformacao se ele nao estiver na posicao correta
                if (transform.localScale.x < 0) {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                meuRigibody.velocity = new Vector3(-moveSpeed, meuRigibody.velocity.y, 0f);
            }
        } else {
            //Forca a parada
            meuRigibody.velocity = new Vector3(0, meuRigibody.velocity.y, 0f);
        }

    }

    private void movimentarParaDireita() {
        if (ativo) {
            meuRigibody.velocity = new Vector3(moveSpeed, meuRigibody.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void movimentarParaEsquerda() {
        if (ativo) {
            meuRigibody.velocity = new Vector3(-moveSpeed, meuRigibody.velocity.y, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // Update is called once per frame
    void Update() {
        switch (tipoMovimento) {
            case Global.typeOfMovementEnemy.IrDireita:
                movimentarParaDireita();
                break;
            case Global.typeOfMovementEnemy.IrEsquerda:
                movimentarParaEsquerda();
                break;
            case Global.typeOfMovementEnemy.Perseguir:
                movimentarModePersiguicao();
                break;
            case Global.typeOfMovementEnemy.Patrulha:
                movimentarModoPatrulha();
                break;
            default:
                break;
        }

        animacao.SetFloat("speed", Mathf.Abs(meuRigibody.velocity.x));
        animacao.SetBool("hit", hit);
    }

    public void OnBecameInvisible() {
		isVisible = false;
    }

    public void OnBecameVisible() {
		if (!isVisible) {
			ativo = true;
			hit = false;
			isVisible = true;
		}
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "KillPlane" || other.tag == "Water" || other.tag == "Spikes") {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    void OnEnable() {
        ativo = false;
    }

}
