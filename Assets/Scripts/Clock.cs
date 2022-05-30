using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    Transform hoursArm, minArm, secArm;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DateTime.Now);
    }

    // Update is called once per frame
    void Update()
    {
        // Updating clock:
        //DateTime now = DateTime.Now;
        //hoursArm.localRotation = Quaternion.Euler(0, 30 * now.Hour, 0);
        //minArm.localRotation = Quaternion.Euler(0, 6 * now.Minute, 0);
        //secArm.localRotation = Quaternion.Euler(0, 6 * now.Second, 0);

        // Analog clock:
        TimeSpan now = DateTime.Now.TimeOfDay;
        hoursArm.localRotation = Quaternion.Euler(0, 30 * (float)now.TotalHours, 0);
        minArm.localRotation = Quaternion.Euler(0, 6 * (float)now.TotalMinutes, 0);
        secArm.localRotation = Quaternion.Euler(0, 6 * (float)now.TotalSeconds, 0);
    }
}
