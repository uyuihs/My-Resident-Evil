using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour {
    public Animator animator;
    private int verticalHash;
    private int horizontalHash;
    protected virtual void Awake(){
        animator = GetComponent<Animator>();
        verticalHash = Animator.StringToHash("Vertical");
        horizontalHash = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimations(float horizontal, float vertical){
        animator.SetFloat(verticalHash, vertical);
        animator.SetFloat(horizontalHash, horizontal);
    }
}
