using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Obstacle", menuName = "Obstacle Info")]
public class ObstacleInfo : ScriptableObject
{

    public string obstableName;
    public int healthPoints;
    public Type obstacleType;

}
