using UnityEngine;

public class RotorSpin : MonoBehaviour
{
    public Vector3 direction = Vector3.up; // default to spinning around the Y-axis

    public float spinSpeed = 720f; // degrees per second

    void Update()
    {
        transform.Rotate(direction * spinSpeed * Time.deltaTime);
    }
}