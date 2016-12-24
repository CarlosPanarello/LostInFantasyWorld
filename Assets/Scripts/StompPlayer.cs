using UnityEngine;
using System.Collections;

public class StompPlayer : MonoBehaviour {

    public GameObject explodirInimigo;
    private Rigidbody2D playerRigidBody;

    public float bouceForce;

    // Use this for initialization
    void Start() {
        playerRigidBody = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            Destroy(other.gameObject);
            Instantiate(explodirInimigo, other.transform.position, other.transform.rotation);
            playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, bouceForce, 0f);
        }
    }
}
