using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
       if(instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int HowManyBalls = 10;
    public GameObject[] ballPrefabs;
    public GameObject startPoint;
    public GameObject[] ballCheckPoints;
    public bool gameOver = false;
    public int numberOfBalls = 10;
    public int ballCounter = 0;
    private BallGridNonMono ballGrid;

    public List<GameObject> activeBalls = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(BallInstantiateCouroutine());
        ballGrid = new BallGridNonMono();
        ballGrid.onCellCreation += InstantiateBallWithCell;
        ballGrid.onGridCompletion += StartMovementCouroutine;
        ballGrid.onBallAdded += InstantiateBallAtIndex;
        ballGrid.onBallRemoved += RemoveBallsOnMatch;
        ballGrid.CreateBallGrid(HowManyBalls);
    }

    // Update is called once per frame
    void Update()
    {
        ballGrid.CheckBallSame();
    }

    IEnumerator BallInstantiateCouroutine()
    {
        for (int i = 0; i < numberOfBalls; i++)
        {
            yield return new WaitForSeconds(0.5f);
            InstantiateBall();
            ballCounter++;
        }
        StartCoroutine(MovementRoutine());
        StopCoroutine(BallInstantiateCouroutine());
    }

    void InstantiateBallWithCell(int randomIndex)
    {
        GameObject activeBall = Instantiate(ballPrefabs[randomIndex], startPoint.transform.position, ballPrefabs[randomIndex].transform.rotation);
        Ball tempBallScript = activeBall.GetComponent<Ball>();
        tempBallScript.SetGameManager(this);
        tempBallScript.isInQue = true;
        tempBallScript.SetGridIndex(activeBalls.Count);
        if(activeBalls.Count > 0 ) MoveOnePosition();
        activeBall.transform.position = ballCheckPoints[0].transform.position;
        activeBalls.Add(activeBall);
    }

    void InstantiateBallAtIndex(int index, int value)
    {
        StopAllCoroutines();
        GameObject activeBall = Instantiate(ballPrefabs[value], startPoint.transform.position, ballPrefabs[value].transform.rotation);
        Ball tempBallScript = activeBall.GetComponent<Ball>();
        tempBallScript.SetGameManager(this);
        tempBallScript.isInQue = true;
        //tempBallScript.SetGridIndex(index);
        activeBalls.Insert(index, activeBall);
        SyncGridWithBalls();
        StartMovementCouroutine();
    }

    void SyncGridWithBalls()
    {
        int tempIndex = activeBalls[0].GetComponent<Ball>().positionIndex;
        for (int i = 0; i < activeBalls.Count; i++)
        {
            tempIndex--;
            activeBalls[i].GetComponent<Ball>().SetGridIndex(i);
            activeBalls[i].GetComponent<Ball>().positionIndex = tempIndex;
        }
        RearrangeAllBalls();
    }

    void RearrangeAllBalls()
    {
        int tempPosition = activeBalls[0].GetComponent<Ball>().GetPosition();
        activeBalls[0].transform.position = ballCheckPoints[tempPosition].transform.position;
        for (int i = 1; i < activeBalls.Count; i++)
        {
            tempPosition--;
            activeBalls[i].transform.position = ballCheckPoints[tempPosition].transform.position;
        }
    }

    void InstantiateBall()
    {
        int randomIndex = Random.Range(0, ballPrefabs.Length);
        GameObject activeBall = Instantiate(ballPrefabs[randomIndex], startPoint.transform.position, ballPrefabs[randomIndex].transform.rotation);
        Ball tempBallScript = activeBall.GetComponent<Ball>();
        tempBallScript.SetGameManager(this);
        tempBallScript.isInQue = true;
        MoveOnePosition();
        activeBall.transform.position = ballCheckPoints[0].transform.position;
        activeBalls.Add(activeBall);
    }

    void MoveOnePosition()
    {
        if (activeBalls.Count < 1) return;
        foreach (GameObject ball in activeBalls)
        {
            Ball tempBallScript = ball.GetComponent<Ball>();
            ball.transform.position = ballCheckPoints[tempBallScript.GetNewPosition()].transform.position;   
        }
    }

    void RemoveBallsOnMatch(int startIndex)
    {
        for (int i = startIndex; i < startIndex+2; i++)
        {
            RemoveFromActiveBalls(activeBalls[i]);
        }
        StopAllCoroutines();
        SyncGridWithBalls();
        StartMovementCouroutine();
    }

    void StartMovementCouroutine()
    {
        StartCoroutine(MovementRoutine());
    }

    IEnumerator MovementRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            MoveOnePosition();
        }
    }

    public void RemoveFromActiveBalls(GameObject ballToRemove)
    {
        GameObject tempBall = null;
        foreach (GameObject ball in activeBalls)
        {
            if(ballToRemove == ball)
            {
                tempBall = ballToRemove;
            }
        }
        if(tempBall != null)
        {
            activeBalls.Remove(tempBall);
        }
    }

    public void BallHit(int index, int value)
    {
        ballGrid.AddAtIndex(index, value);
    }
}
