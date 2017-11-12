using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("Setup Prefabs")]
    public GameObject ballPrefab;
    public GameObject paddlePivot;
    public GameObject paddle;
    public CircleCollider2D paddleBuffer;
    private GameObject currentBall;
    public float ballScaleSpeed = 0.15f;
    private float radius;
    public Text scoreText;

    [Header("Game Variables")]
    public int targetScore;
    public int remainingScore;

    // Publics
    public void Score() {
        PaddleController.instance.canMove = false;
        remainingScore--;
        AudioManager.instance.Pop();

        // If the player hit the last remaining ball
        if (remainingScore <= 0) {
            targetScore++;
            remainingScore = targetScore;
            Debug.Log("Level " + (targetScore-1) + " complete!");
            PaddleController.instance.ResetPaddle();
            MoveBall();
        }
        // If the player still has balls to hit
        else {
            MoveBall();
            PaddleController.instance.SwitchPaddle();
            PaddleController.instance.canMove = true;
        }
        //Debug.Log("Score! " + remainingScore + "/" + targetScore);
        scoreText.text = remainingScore.ToString();
    }

    public void Lose() {
        PaddleController.instance.ResetPaddle();
        remainingScore = targetScore;
        scoreText.text = remainingScore.ToString();
        MoveBall();
        Debug.Log("Lose!");
    }

    // Privates
    private void MoveBall() {
        paddleBuffer.GetComponent<CircleCollider2D>().enabled = true;
        currentBall.GetComponent<SpriteRenderer>().enabled = false;
        currentBall.transform.localScale = new Vector3(1, 1, 1);
        currentBall.transform.position = RandomCirclePos();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(paddle.transform.position, paddleBuffer.radius);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.tag == "Ball") {
                //Debug.LogWarning("Moved ball inside buffer, vector: " + collider.gameObject.transform.position);
                MoveBall();
                return;
            }
        }
        currentBall.GetComponent<SpriteRenderer>().enabled = true;
        paddleBuffer.GetComponent<CircleCollider2D>().enabled = false;
    }

    /// <summary>
    /// Generates a random position along a circular path.
    /// </summary>
    /// <returns>A Vector3 location along a circular path.</returns>
    private Vector3 RandomCirclePos() {
        float angle = Random.value * 360;
        Vector3 pos;
        pos.x = paddlePivot.transform.position.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = paddlePivot.transform.position.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        pos.z = paddlePivot.transform.position.z;
        return pos;
    }

    // Monos
    private void Awake() {
        instance = this;
    }

    private void Start() {
        remainingScore = targetScore;
        radius = paddlePivot.GetComponent<CircleCollider2D>().radius;
        scoreText.text = remainingScore.ToString();
        currentBall = Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        MoveBall();
    }
}
