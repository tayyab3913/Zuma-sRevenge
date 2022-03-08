using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGridNonMono
{
    public List<int> ballGrid;

    public delegate void OnCellCreation (int value);
    public event OnCellCreation onCellCreation;

    public delegate void OnGridCompletion();
    public event OnGridCompletion onGridCompletion;

    public delegate void OnBallAdded(int index, int value);
    public event OnBallAdded onBallAdded;

    public delegate void OnBallRemoved(int startIndex);
    public event OnBallRemoved onBallRemoved;


    public BallGridNonMono()
    {
        
    }

    public void CreateBallGrid(int numberOfBalls)
    {
        ballGrid = new List<int>();
        for (int i = 0; i < numberOfBalls; i++)
        {
            int tempIndex = Random.Range(0, 3);
            ballGrid.Add(tempIndex);

            onCellCreation?.Invoke(tempIndex);
        }
        onGridCompletion?.Invoke();
    }

    public void AddAtIndex(int index, int value)
    {
        ballGrid.Insert(index, value);
        onBallAdded?.Invoke(index, value);
    }

    public void CheckBallSame()
    {
        //Debug.Log("CheckSameBall");
        if (ballGrid.Count < 3) return;
        int startIndex = 0;
        int stopIndex = 0;

        for (int i = 0; i < ballGrid.Count-2; i++)
        {
            if (ballGrid[i] == ballGrid[i+1] && ballGrid[i + 1] == ballGrid[i+2])
            {
                Debug.Log("SameBallsFound");
                startIndex = i;
                stopIndex = i + 2;
                RemoveThreeBalls(startIndex);
            }
        }

        //for (int i = 2; i < ballGrid.Count; i++)
        //{
        //    if(value1 == value2 && value2 == value3)
        //    {
        //        Debug.Log("SameBallsFound");
        //        startIndex = value1Index;
        //        stopIndex = value3Index;
        //        RemoveBallsInRange(startIndex, stopIndex);
        //    }
        //    value1++;
        //    value2++;
        //    value3++;
        //    value1Index++;
        //    value2Index++;
        //    value3Index++;
        //}

        //if (startIndex - stopIndex > 2 || startIndex - stopIndex < -2)
        //{
        //    RemoveBallsInRange(startIndex, stopIndex);
        //}

        //for (int i = 1; i < ballGrid.Count; i++)
        //{
        //    if(valueToCheck == ballGrid[i])
        //    {
        //        stopIndex = i;
        //    }
        //    else
        //    {
        //        if(startIndex - stopIndex > 2 || startIndex - stopIndex < -2)
        //        {
        //            RemoveBallsInRange(startIndex, stopIndex);
        //        }
        //        startIndex = i;
        //        stopIndex = i;
        //    }
        //    valueToCheck = ballGrid[i];
        //}


    }

    //void RemoveThreeBalls(int value1, int value2, int value3)
    //{
    //    foreach (var item in collection)
    //    {

    //    }
    //}

    //void RemoveBallsInRange(int startIndex, int stopIndex)
    //{
    //    for (int i = startIndex; i <= stopIndex; i++)
    //    {
    //        ballGrid.Remove(i);
    //    }
    //    onBallRemoved?.Invoke(startIndex, stopIndex);
    //}

    void RemoveThreeBalls(int startIndex)
    {
        for (int i = startIndex; i <= startIndex+2; i++)
        {
            ballGrid.Remove(i);
        }
        onBallRemoved?.Invoke(startIndex);
    }
}
