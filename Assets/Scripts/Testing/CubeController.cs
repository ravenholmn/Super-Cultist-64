using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
        private Coroutine _scaleDown;

        public void OnPressingE()
        {
                if (_scaleDown != default)
                {
                        StopCoroutine(_scaleDown);
                }

                _scaleDown = StartCoroutine(ScaleDown());
        }

        IEnumerator ScaleDown()
        {
                var wait = new WaitForEndOfFrame();
                float timer = 0f;
                float duration = 1f;

                Vector3 startScale = transform.localScale;
                Vector3 to = Vector3.zero;

                if (startScale.y < 0.5f)
                {
                        to = Vector3.one;
                }

                while (timer < duration)
                {
                        timer += Time.deltaTime;

                        transform.localScale = Vector3.Lerp(startScale, to, timer / duration);

                        yield return wait;
                }
        }
}