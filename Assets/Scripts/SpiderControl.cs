using UnityEngine;
using System.Collections;

public class SpiderControl : MonoBehaviour {

    public float moveSpeed;
    public Global.tipoMovimentacaoInimigo tipoMovimento;

    public Transform fim;
	public Transform inicio;
    
    public LevelManager level;

    private bool ativo;
    private Rigidbody2D meuRigibody;
    private Animator animacao;

    private Vector3 pontoFinal;
    private Vector3 pontoInicio;
    private Vector3 alvoAtual;

    // Use this for initialization
    void Start() {
        meuRigibody = GetComponent<Rigidbody2D>();
        animacao = GetComponent<Animator>();
        
        ativo = false;

		pontoInicio = new Vector3(inicio.position.x, transform.position.y, transform.position.z);
        pontoFinal = new Vector3(fim.position.x, transform.position.y, transform.position.z);
        alvoAtual = pontoFinal;
    }

    private void movimentarModoPatrulha() {

        meuRigibody.velocity =
        Vector3.MoveTowards(transform.position, alvoAtual, moveSpeed * Time.deltaTime);

        if (transform.position.x == pontoFinal.x) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            alvoAtual = pontoInicio;
        }

        if (transform.position.x == pontoInicio.x) {
            transform.localScale = new Vector3(1f, 1f, 1f);
            alvoAtual = pontoFinal;
        }
    }

    private void movimentarModePersiguicao() {
        float posicaoX = level.posicaoJogadorAtivo().x;

        if (posicaoX>0) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        Vector3 alvoPerseguir = new Vector3(posicaoX, transform.position.y, transform.position.z);
        meuRigibody.velocity = Vector3.MoveTowards(transform.position, alvoPerseguir, moveSpeed * Time.deltaTime);
    }

    private void movimentarParaDireita() {
        meuRigibody.velocity = new Vector3(moveSpeed * Time.deltaTime, meuRigibody.velocity.y, 0f);
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    private void movimentarParaEsquerda() {
        meuRigibody.velocity = new Vector3(-(moveSpeed * Time.deltaTime), meuRigibody.velocity.y, 0f);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update() {
        
        if (ativo) {
            switch (tipoMovimento) {
                case Global.tipoMovimentacaoInimigo.IrDireita:
                    movimentarParaDireita();
                    break;
                case Global.tipoMovimentacaoInimigo.IrEsquerda:
                    movimentarParaEsquerda();
                    break;
                case Global.tipoMovimentacaoInimigo.Perseguir:
                    movimentarModePersiguicao();
                    break;
                case Global.tipoMovimentacaoInimigo.Patrulha:
                    movimentarModoPatrulha();
                    break;
                default:
                    break;
            }
        }
    }

    public void OnBecameInvisible() {

    }

    public void OnBecameVisible() {
        ativo = true;
        animacao.SetBool("ativo", true);
        animacao.SetBool("hit", false);
    }
}
