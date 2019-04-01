using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSBInput {
    private Vector2 Input2D;
    private float Input1D;


    public LSBInput(){}
    public LSBInput(float input) {
        Input1D = input;
    }

    public LSBInput(Vector2 input)
    {
        Input2D = input;
    }
    
    public float getInput1D()
    {
        return Input1D;
    }
    public Vector2 getInput2D()
    {
        return Input2D;
    }
}
