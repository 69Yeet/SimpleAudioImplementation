using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private int audioStackCount;
    private Queue<GameObject> audioPlayers = new Queue<GameObject>();
}