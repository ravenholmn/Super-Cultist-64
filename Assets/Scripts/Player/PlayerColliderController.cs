using System;
using UnityEngine;

public class PlayerColliderController : PlayerInputHandler
{
        private InputListener inputListener;

        private void Update()
        {
                InteractionInput();
                CheckIfCollidingWithInputObject();
        }

        private void CheckIfCollidingWithInputObject()
        {
                if (!InteractionInputTrigger) return;
                if (inputListener == default) return;

                InteractionInputTrigger = false;
                inputListener.TakeInput();
        }

        private void OnTriggerEnter(Collider other)
        {
                InputListener inputListener = other.transform.gameObject.GetComponent<InputListener>();

                if (inputListener)
                {
                        this.inputListener = inputListener;
                }
        }

        private void OnTriggerExit(Collider other)
        {
                InputListener inputListener = other.transform.gameObject.GetComponent<InputListener>();

                if (inputListener)
                {
                        this.inputListener = default;
                }
        }
}