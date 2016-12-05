using UnityEngine;
using System.Collections;


public class InvalidResponseException : System.Exception
{
    public InvalidResponseException() : base(){

    }
    public InvalidResponseException(string msg) : base(msg){

    }
}
