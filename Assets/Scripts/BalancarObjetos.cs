using UnityEngine;
using System.Collections;

public class BalancarObjetos : MonoBehaviour {

    public int grauMovimentacao;
    public float velocidade;
    public GameObject objeto;

    float grauDireita;
    float grauEsquerda;

    private float step;
    Vector3 rotationEuler;

    // Use this for initialization
    void Start () {
        float valor = grauMovimentacao / 2;
        grauDireita = valor;
        grauEsquerda = 360 - valor;
        direta = true;
		//TODO isso aqui ta estranho 
        step = velocidade * Time.deltaTime;
       // transform.rotation = new Quaternion(2)

    }

    private bool direta;
    

    // Update is called once per frame
    void Update () {
        //rotationEuler += Vector3.zero * step; //increment 30 degrees every second

        //rotationEuler += Vector3.forward * 30 * Time.deltaTime; //increment 30 degrees every second

        if (transform.rotation.eulerAngles.z >= 0 && transform.rotation.eulerAngles.z <= grauDireita) {
            if (direta) {
                rotationEuler += Vector3.forward * step; //increment 30 degrees every second
            } else {
                rotationEuler -= Vector3.forward * step; //increment 30 degrees every second
            }
        } else if (transform.rotation.eulerAngles.z < 360 && transform.rotation.eulerAngles.z > grauEsquerda) {
            if (direta) {
                rotationEuler += Vector3.forward * step; //increment 30 degrees every second
            } else {
                rotationEuler -= Vector3.forward * step; //increment 30 degrees every second
            }
        } else {
            if (direta) {
                direta = false;
                rotationEuler -= Vector3.forward * step; //increment 30 degrees every second
            } else {
                direta = true;
                rotationEuler += Vector3.forward * step; //increment 30 degrees every second
            }
            transform.rotation = Quaternion.Euler(rotationEuler);
        }
        transform.rotation = Quaternion.Euler(rotationEuler);
    }
}
