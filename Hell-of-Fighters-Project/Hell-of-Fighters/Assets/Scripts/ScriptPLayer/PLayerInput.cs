using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptPLayer
{
    public class PlayerInput : MonoBehaviour
    {
        public bool isGrounded;
        public bool isAttacking1;
        public bool isAttacking2;
        public bool isAttacking3;
        public bool isAirAttacking;
        public bool isRolling;
        public bool isHealing;
        public bool isAttackingSpe;
        public bool isDefending;

        public Transform groudCheckLeft;
        public Transform groudCheckRigth;
        
        void Update()
        {
            isGrounded=Physics2D.OverlapArea(groudCheckLeft.position,groudCheckRigth.position);
        }
    
        public Vector2 InputVelocity
        {
            get;
            private set;
        }
        public void OnMovement(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            float horizontalInput = context.ReadValue<float>();
            InputVelocity = new Vector2(horizontalInput, 0);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            if (isGrounded && !isAttacking2 && !isAttacking3)
            {
                InputVelocity += new Vector2(0, 1.2f);
            }
        }

        public void OnAttack1(InputAction.CallbackContext context)
        {
            if (context.performed && !isAttacking1)
            {
                isAttacking1 = true;
            }
        }

        public void OnAttack2(InputAction.CallbackContext context)
        {
            if (context.performed && !isAttacking2)
            {
                isAttacking2 = true;
            }
        }
        public void OnAttack3(InputAction.CallbackContext context)
        {
            if (context.performed && !isAttacking3)
            {
                isAttacking3 = true;
            }
        }
        public void OnAirAttack(InputAction.CallbackContext context)
        {
            if (context.performed && !isAirAttacking)
            {
                isAirAttacking = true;
            }
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                isRolling = true;
            }
        }

        public void OnHeal(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                isHealing = true;
            }
        }

        public void OnAttackSpe(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                isAttackingSpe = true;
            }
        }

        public void OnDefend(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                isDefending = true;
            }
        }
        public void ResetJump()
        {
            InputVelocity = new Vector2(InputVelocity.x, 0);
        }

        public void ResetAirAttack()
        {
            isAirAttacking = false;
        }
    }
}
