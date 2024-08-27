using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;

namespace CoinFlipLogger
{
    public class CoinFlipLoggerPlugin
    {
        [PluginConfig]
        public PluginConfig Config;

        private ItemType[] PossibleItems;

        // Parameters for the linear congruential generator (LCG)
        private const uint LCGMultiplier = 1664525;
        private const uint LCGIncrement = 1013904223;
        private uint lcgState = 0; // Seed for the LCG

        [PluginPriority(LoadPriority.High)]
        [PluginEntryPoint(
            "CoinGambler", 
            "1.0.0", 
            "Makes you able to gamble with coins.", 
            "Joseph_fallen")]

        public void OnPluginStart()
        {
            // Initialize the array and the deterministic sequence
            InitializePossibleItems();
            lcgState = (uint)DateTime.Now.Ticks; // Initialize LCG seed

            // Register event handlers
            EventManager.RegisterEvents(this);
            Log.Info("CoinFlipLogger plugin has been enabled.");
        }

        private void InitializePossibleItems()
        {
            // Initialize the array with non-SCP item types from the SCP:SL wiki
            PossibleItems = new ItemType[]
            {
                ItemType.Coin,                     // Coin
                ItemType.GunCOM18,                 // COM-18
                ItemType.Medkit,                   // Medkit
                ItemType.ArmorHeavy,               // Heavy Armor
                ItemType.ArmorLight,               // Light Armor
                ItemType.Radio,                    // Radio
                ItemType.Adrenaline,
                ItemType.AntiSCP207,
                ItemType.SCP500,
                ItemType.Ammo9x19,
                ItemType.Ammo9x19,
                ItemType.Ammo9x19,
                ItemType.KeycardO5,                // O5 Keycard
                ItemType.KeycardJanitor,           // Janitor Keycard
                ItemType.KeycardScientist,         // Scientist Keycard
                ItemType.KeycardResearchCoordinator, // Research Coordinator Keycard
                ItemType.KeycardZoneManager,       // Zone Manager Keycard
                ItemType.KeycardGuard,             // Guard Keycard
                ItemType.KeycardMTFPrivate,        // MTF Private Keycard
                ItemType.KeycardContainmentEngineer, // Containment Engineer Keycard
                ItemType.KeycardMTFOperative,      // MTF Operative Keycard
                ItemType.KeycardMTFCaptain,        // MTF Captain Keycard
                ItemType.KeycardFacilityManager,   // Facility Manager Keycard
                ItemType.KeycardChaosInsurgency,   // Chaos Insurgency Keycard
                ItemType.GrenadeHE,                // High-Explosive Grenade
                ItemType.GrenadeFlash,             // Flash Grenade
                // Add any additional non-SCP items if needed
            };
        }

        private int GetRandomIndex(int max)
        {
            // Linear congruential generator (LCG) for pseudo-random number generation
            lcgState = LCGMultiplier * lcgState + LCGIncrement;
            return (int)(lcgState % max);
        }

        private bool RollChance(float chance)
        {
            // Roll a random chance based on the provided percentage
            return (GetRandomIndex(100) < chance * 100);
        }

        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void OnPlayerCoinFlip(PlayerCoinFlipEvent ev)
        {
            // Log the player's coin flip
            string logMessage = ev.Player.Nickname + " (" + ev.Player.UserId + ") flipped a coin.";
            Log.Info(logMessage);

            // Check if the item should be given with a 25% chance
            if (RollChance(0.25f)) // 25% chance
            {
                // Get a random index for the item
                int randomIndex = GetRandomIndex(PossibleItems.Length);
                var itemToGive = PossibleItems[randomIndex];

                // Give the item to the player
                ev.Player.AddItem(itemToGive);
                string itemMessage = ev.Player.Nickname + " (" + ev.Player.UserId + ") received a " + itemToGive + ".";
                Log.Info(itemMessage);
            }
        }
    }
}
