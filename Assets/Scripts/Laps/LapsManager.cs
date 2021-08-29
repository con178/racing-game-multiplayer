using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapsManager : MonoBehaviour
{
    public GameObject[] checkpoints;
    [SerializeField] private GameObject finalPoint;
    public List<int> checkPointsCompleted;
    public List<int> lapsCompleted;
    public int lapsToFinish = 3;


    public void ClearList(List<int> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            list[i] = 0;
        }
    }
}
