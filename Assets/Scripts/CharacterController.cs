using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody2D body2D;

    [Header("Controller Properties")]
    [SerializeField, Range(0, 50)] private float runSpeed = 15f;
    [SerializeField, Range(0, 50)] private float jumpSpeed = 30f;


}
