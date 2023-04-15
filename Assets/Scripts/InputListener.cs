using UnityEngine;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
        [SerializeField] private UnityEvent eventToInvoke;

        public void TakeInput()
        {
                if (eventToInvoke != default)
                {
                        eventToInvoke.Invoke();
                }
        }
}