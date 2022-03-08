using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 200;
    public float horizontalInput;
    public GameObject[] ballPrefabs;
    public GameObject shootPoint;
    private GameObject activeBall;
    private Ball ballMovementScript;
    // Start is called before the first frame update
    void Start()
    {
        InstantiateBall();
    }

    // Update is called once per frame
    void Update()
    {
        CannonRotation();
        BallShooting();
    }

    void CannonRotation()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * horizontalInput);
    }

    void BallShooting()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
            InstantiateBall();
        }
    }

    void InstantiateBall()
    {
        int randomIndex = Random.Range(0, ballPrefabs.Length);
        activeBall = Instantiate(ballPrefabs[randomIndex], shootPoint.transform.position, shootPoint.transform.rotation);
        activeBall.transform.SetParent(transform);
        ballMovementScript = activeBall.GetComponent<Ball>();
    }

    void ShootBall()
    {
        activeBall.transform.SetParent(null);
        ballMovementScript.isShot = true;
    }
}
