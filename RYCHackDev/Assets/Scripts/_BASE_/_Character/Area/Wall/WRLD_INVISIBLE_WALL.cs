using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_INVISIBLE_WALL : MonoBehaviour
    {
        [Header("TEMP")]
        public GameObject testMobAICharacter; // Test Mob A.I Character
        private AICharacterManager aiCharacter; // Test A.I Character Script

        public bool wallHasBeenHit; // Checks if the player hit the wall
        public Material invisibleWallMaterial; // Material for the invisible wall
        public float alpha; // Alpha value to tweak the invisibility
        public float fadeTimer = 2.5f; // To smooth the transition from solid to invisible
        public BoxCollider wallCollider; // Box collider of the wall

        public AudioSource audioSource; // Audio source to get the wall audio clip
        public AudioClip invisibleWallSound; // Audio clip of interacting with the wall
       
        private void Awake()
        {
            aiCharacter = testMobAICharacter.GetComponent<AICharacterManager>();
        }
        private void Update()
        {
            if(wallHasBeenHit)
            {
                FadeToInvisibleWall();
            }

            else if(aiCharacter.aiCharacterStatsManager.tempisDead)
            {
                FadeToVisibleWall();
            }
        }
        public void FadeToInvisibleWall() // To have invisible wall but it will start from visible to invisible with fade
        {
            alpha = invisibleWallMaterial.GetColor("_BaseColor").a;
            alpha = alpha - Time.deltaTime / fadeTimer;
            Color fadedWallColor = new Color(1, 1, 1, alpha);
            invisibleWallMaterial.SetColor("_BaseColor", fadedWallColor);

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                audioSource.PlayOneShot(invisibleWallSound);
            }
            if(alpha <= 0)
            {
                Destroy(this);
            }
        }
        public void FadeToVisibleWall() // To have visible wall but it will start from invisible to visible with fade
        {
            alpha = invisibleWallMaterial.GetColor("_BaseColor").a;
            alpha = Mathf.Min(alpha + Time.deltaTime / fadeTimer, 1.0f); // Ensure the alpha value doesn't exceed 1
            Color fadedWallColor = new Color(1, 1, 1, alpha);
            invisibleWallMaterial.SetColor("_BaseColor", fadedWallColor);
        }

        public void NoFadeVisibleWall() // To have visible wall without fading
        {
            Color originalColor = invisibleWallMaterial.GetColor("_BaseColor");
            Color fullAlphaColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f);
            invisibleWallMaterial.SetColor("_BaseColor", fullAlphaColor);
        }

        public void NoFadeInvisibleWall() // To have invisible wall without fading
        {
            Color originalColor = invisibleWallMaterial.GetColor("_BaseColor");
            Color zeroAlphaColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f); 
            invisibleWallMaterial.SetColor("_BaseColor", zeroAlphaColor);
        }
    }
}