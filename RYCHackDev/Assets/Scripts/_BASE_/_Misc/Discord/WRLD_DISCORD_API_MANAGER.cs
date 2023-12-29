using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

namespace Deceilio.Psychain
{
    public class WRLD_DISCORD_API_MANAGER : MonoBehaviour
    {
        public long applicationID; // Application ID for the game
        [Space]
        public string details = "Exploring the haunting world of Psychain."; // Description of the game
        public string state = "Current position: "; // Current position state of the player, in-game
        [Space]
        public string largeImage = "game_logo"; // String value used for the logo name
        public string largeText = "Chained Psychics"; // String value used for the game name

        private Transform playerTransform; // Reference to the rigidbody component
        private long time; // Reference to the time value

        private static bool instanceExists; // Bool to check if the instance exist or not
        public Discord.Discord discord; // Reference to the discord api

        void Awake()
        {
            // BELOW CODE: Transition the game object between scenes, destroy any duplicates
            if(!instanceExists)
            {
                instanceExists = true;
                DontDestroyOnLoad(gameObject);
            }
            else if(FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

            playerTransform = GameObject.FindObjectOfType<PlayerManager>().GetComponent<Transform>();
            time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

            UpdateStatus();
        }

        void Update()
        {
            // BELOW CODE: Destroy the gameObject if discord is not running
            try 
            {
                discord.RunCallbacks();
            } 
            catch
            {
                Destroy(gameObject);
            } 
        }

        void LateUpdate()
        {
            UpdateStatus();
        }

        void UpdateStatus()
        {
            // BELOW CODE: Update status every frame
            try
            {
                var activityManager = discord.GetActivityManager();
                var activity = new Discord.Activity
                {
                    Details = details,
                    State = state + playerTransform.position, 
                    Assets = 
                    {
                        LargeImage = largeImage,
                        LargeText = largeText
                    },
                    Timestamps = 
                    {
                        Start = time
                    }
                };

                activityManager.UpdateActivity(activity, (res) => 
                {
                    if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to discord");
                });
            }
            catch
            {
                // BELOW CODE: If updating the status fails, destroy the game object
                Destroy(gameObject);
            }
        }
    }
}