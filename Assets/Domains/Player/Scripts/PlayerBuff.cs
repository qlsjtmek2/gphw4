using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    public Speed speed;

    public void SpeedBuff()
    {
        speed.Value += 1.0f;
    }
}
