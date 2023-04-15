using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{

        #region Movement
    
        protected float HorizontalInput;
        protected float VerticalInput;
        protected Vector3 Direction;
        protected bool HoldingShift;
        protected bool Jump;
        protected bool HoldingJump;
        protected bool ReleasedJump;
        protected bool Crouch;
        protected bool HoldingCrouch;
        protected bool ReleasedCrouch;

        #endregion
    
        #region MouseLook
    
        protected float XRotation;
        protected float YRotation;
        private float _mouseX;
        private float _mouseY;

        #endregion

        #region Interaction

        protected bool InteractionInputTrigger;

        #endregion

        #region Inventory

        protected bool InventoryInputTrigger;
        protected bool ScrollLeft, ScrollRight;

        #endregion

        protected virtual void LookInput()
        {
                if (InputBlocker()) return;
                _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * PlayerController.Instance.PlayerConfig.mouseSensitivityX;
                _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * PlayerController.Instance.PlayerConfig.mouseSensitivityY;

                YRotation += _mouseX;
                XRotation -= _mouseY;
        }

        protected virtual void MoveInput()
        {
                if (InputBlocker()) return;
                HorizontalInput = Input.GetAxisRaw("Horizontal");
                VerticalInput = Input.GetAxisRaw("Vertical");
                
                HoldingShift = Input.GetKey(KeyCode.LeftShift);
                
                Jump = Input.GetKeyDown(KeyCode.Space);
                HoldingJump = Input.GetKey(KeyCode.Space);
                ReleasedJump = Input.GetKeyUp(KeyCode.Space);
                
                Crouch = Input.GetKeyDown(KeyCode.LeftControl);
                HoldingCrouch = Input.GetKey(KeyCode.LeftControl);
                ReleasedCrouch = Input.GetKeyUp(KeyCode.LeftControl);
        }

        protected virtual void InteractionInput()
        {
                if (InputBlocker()) return;
                InteractionInputTrigger = Input.GetKeyDown(KeyCode.E);
        }

        protected virtual bool InputBlocker()
        {
                return Dialogue.instance != null && Dialogue.instance.gameObject.activeSelf;
        }
}