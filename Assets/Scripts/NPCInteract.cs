using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public Animator animator;
    public void Interact()
    {
        animator.SetBool("isWaving", true);
        Debug.Log("Interact!");
    }
}
