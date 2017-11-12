using UnityEngine;

public class PaddleController : MonoBehaviour {

    public static PaddleController instance;

    [Header("Pivot Setup")]
    public GameObject pivotObject;
    public float rotationSpeed;
    public bool direction = false;
    public bool canMove = false;
    public bool onBall = false;
    private Vector3 originalPosition;

    private GameManager gm;

    // Publics
    public void ResetPaddle() {
        canMove = false;
        direction = true;
        onBall = false;
        pivotObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = originalPosition;
        Debug.Log("Paddle reset!");
    }

    public void SwitchPaddle() {
        direction = !direction;
        onBall = false;
    }

    // Monos
    private void Awake() {
        instance = this;
    }

    private void Start() {
        gm = GameManager.instance;
        originalPosition = transform.position;
    }

    private void Update() {

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.F)) {
            if (canMove) {
                if (onBall) {
                    gm.Score();
                }
                else {
                    gm.Lose();
                }
            }
            else {
                canMove = true;
            }
        }

        // Rotate right
        if (canMove) {
            if (direction) {
                pivotObject.transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
            }
            // Rotate left
            else {
                pivotObject.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ball") {
            onBall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (onBall) {
            onBall = false;
            gm.Lose();
        }
    }

}
