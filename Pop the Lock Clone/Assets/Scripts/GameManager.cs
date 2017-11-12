using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        if (remainingScore <= 0) {
            currentBall.transform.DOScale(0, ballScaleSpeed).OnComplete(ScaleCompletePause);
            targetScore++;
            remainingScore = targetScore;
            Debug.Log("Level complete!");
        }
        else {
            currentBall.transform.DOScale(0, ballScaleSpeed).OnComplete(ScaleComplete);
            PaddleController.instance.SwitchPaddle();
        }
        Debug.Log("Score! " + remainingScore + "/" + targetScore);
        scoreText.text = remainingScore.ToString();
    }

    public void Lose() {
        PaddleController.instance.ResetPaddle();
        LoseShake();
        remainingScore = targetScore;
        scoreText.text = remainingScore.ToString();
        MoveBall();
        Debug.Log("Lose!");
    }

    // Privates
    private void ScaleComplete() {
        MoveBall();
        PaddleController.instance.canMove = true;
    }

    private void ScaleCompletePause() {
        PaddleController.instance.ResetPaddle();
        MoveBall();
    }

    private void LoseShake() {
        Camera.main.transform.DOShakeRotation(0.25f, 3, 50, 20, true).OnComplete(LoseShakeComplete);
    }

    private void LoseShakeComplete() {
    }

    private void MoveBall() {
        paddleBuffer.GetComponent<CircleCollider2D>().enabled = true;
        currentBall.GetComponent<SpriteRenderer>().enabled = false;
        currentBall.transform.localScale = new Vector3(1, 1, 1);
        currentBall.transform.position = RandomCirclePos();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(paddle.transform.position, paddleBuffer.radius);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.tag == "Ball") {
                MoveBall();
                return;
            }
        }
        currentBall.GetComponent<SpriteRenderer>().enabled = true;
        paddleBuffer.GetComponent<CircleCollider2D>().enabled = false;
    }

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

    /*
     * private void Spawn(GameObject prefab) {
        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        obj.transform.position = RandomCircle(transform.position, circle.radius);
        obj.transform.LookAt(transform.position);
        obj.transform.Rotate(new Vector3(1, 0, 0), 90);
    }

    private Vector3 RandomCircle (Vector3 center, float radius) {
        float angle = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
    */

}
