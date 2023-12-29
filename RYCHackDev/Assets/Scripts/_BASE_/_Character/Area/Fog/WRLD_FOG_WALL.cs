using UnityEngine;

namespace Deceilio.Psychain 
{
    public class WRLD_FOG_WALL : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
        public void ActivateFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }
    }
}
