using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10, 10, 10);
    [SerializeField] float period = 2f;
    //[Range(0,1)] [SerializeField] << This creates a range slider that can be manipulated within the inspector. 
    float movementFactor; // 0 for not moved, 1 for fully moved. 
    private Vector3 startingPosition; 

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; } // You can not compare floats as they may be off my tiny fractions of the number. We need to determine our Threshold of acceptable values in comparison. Think peoples height. 
        float cycles = Time.time / period; // grows continually from 0
        //print(cycles); 
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau); // gives a value from -1 to 1
        //print(rawSinWave);
        movementFactor = rawSinWave / 2f + 0.5f; // We need a value of 0-1
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset; 
    }
}
