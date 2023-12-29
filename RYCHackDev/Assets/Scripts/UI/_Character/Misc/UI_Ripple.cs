using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_Ripple : MonoBehaviour
    {
        public bool unscaledTime = false; // Checks if the upscale time should be enable or not
        public float speed; // Speed of the ripple
        public float maxSize; // Max size of the ripple
        public Color startColor; // Start color of the ripple
        public Color transitionColor; // Transition color of the ripple
        Image colorImg; // Image of the ripple

        void Start()
        {
            transform.localScale = new Vector3(0f, 0f, 0f);
            colorImg = GetComponent<Image>();
            colorImg.color = new Color(startColor.r, startColor.g, startColor.b, startColor.a);
        }

        void OnDisable()
        {
            Destroy(gameObject);
        }

        void Update()
        {
            if (unscaledTime == false)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), Time.deltaTime * speed);
                colorImg.color = Color.Lerp(colorImg.color, new Color(transitionColor.r, transitionColor.g, transitionColor.b, transitionColor.a), Time.deltaTime * speed);

                if (transform.localScale.x >= maxSize * 0.998)
                {
                    if (transform.parent.childCount == 1)
                        transform.parent.gameObject.SetActive(false);

                    Destroy(gameObject);
                }
            }

            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), Time.unscaledDeltaTime * speed);
                colorImg.color = Color.Lerp(colorImg.color, new Color(transitionColor.r, transitionColor.g, transitionColor.b, transitionColor.a), Time.unscaledDeltaTime * speed);

                if (transform.localScale.x >= maxSize * 0.998)
                {
                    if (transform.parent.childCount == 1)
                        transform.parent.gameObject.SetActive(false);

                    Destroy(gameObject);
                }
            }
        }
    }
}