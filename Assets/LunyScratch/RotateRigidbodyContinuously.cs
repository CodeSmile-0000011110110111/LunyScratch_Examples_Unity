using System;
using UnityEngine;

public class RotateRigidbodyContinuously : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public Boolean _getInputInFixedUpdate;

    Rigidbody _rigidbody;
    private Boolean _shouldRotate;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!_getInputInFixedUpdate)
            _shouldRotate = Input.GetKey(KeyCode.Space);

    }
    void FixedUpdate()
    {
        if (_getInputInFixedUpdate)
            _shouldRotate = Input.GetKey(KeyCode.Space);

        if (_shouldRotate)
        {
            // var rot = Quaternion.Euler(0, rotationSpeed * Time.fixedDeltaTime, 0);
            // _rigidbody.MoveRotation(_rigidbody.rotation * rot);
            _rigidbody.angularVelocity = new Vector3(0f, rotationSpeed * Time.fixedDeltaTime, 0f);
            //_rigidbody.AddTorque(0f, rotationSpeed * Time.fixedDeltaTime, 0f, ForceMode.VelocityChange);
        }
        else
        {
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
