using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using InventorySystem.Items;

namespace CoinFlipLogger
{
    public class CoinFlipLoggerPlugin
    {
        [PluginConfig]
        public PluginConfig Config;

        private ItemType[] PossibleItems;
        private ItemType[] RareItems;
        private ItemType[] LegendItems; // Added this array
        private int currentIndex;
        private int flipCount; // To simulate randomness with a simple counter

        // Parameters for the linear congruential generator (LCG)
        private const uint LCGMultiplier = 1664525;
        private const uint LCGIncrement = 1013904223;
        private uint lcgState = 0; // Seed for the LCG

        // Probability constants
        private const double LoseCoinChance = 0.10; // 10% chance to lose the coin
        private const double RareItemChance = 0.05; // 5% chance for a rare item
        private const double LegendItemChance = 0.01; // 1% chance for a legendary item

        [PluginPriority(LoadPriority.High)]
        [PluginEntryPoint("CoinFlipLogger", "1.1.0", "Logs when a player flips a coin and occasionally rewards them with items, with some risk of losing the coin.", "YourName")]
        public void OnPluginStart()
        {
            // Initialize the arrays and the deterministic sequence
            InitializePossibleItems();
            InitializeRareItems();
            InitializeLegendItems(); // Ensure this is called to initialize the array
            currentIndex = 0;
            flipCount = 0; // Initialize the counter
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

        private void InitializeRareItems()
        {
            // Initialize the array with rare items
            RareItems = new ItemType[]
            {
                ItemType.GunCom45,                 // COM-45
                ItemType.ParticleDisruptor,        // Particle Disruptor
                ItemType.Jailbird,                 // Jailbird
            };
        }

        private void InitializeLegendItems()
        {
            // Initialize the array with legendary items
            LegendItems = new ItemType[]
            {
                ItemType.MicroHID // Example legendary item
            };
        }

        private int GetRandomIndex(int max)
        {
            // Linear congruential generator (LCG) for pseudo-random number generation
            lcgState = LCGMultiplier * lcgState + LCGIncrement;
            return (int)(lcgState % max);
        }

        private ItemType GetRandomItem(ItemType[] items)
        {
            // Get a random index for the item
            int randomIndex = GetRandomIndex(items.Length);
            return items[randomIndex];
        }

        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void OnPlayerCoinFlip(PlayerCoinFlipEvent ev)
        {
            // Log the player's coin flip
            string logMessage = ev.Player.Nickname + " (" + ev.Player.UserId + ") flipped a coin.";
            Log.Info(logMessage);

            // Simulate 10% chance to lose the coin
            if (GetRandomIndex(100) < 10) // 10% chance to lose the coin
            {
                ev.Player.RemoveItem(ev.Player.CurrentItem);
                Log.Info(ev.Player.Nickname + " (" + ev.Player.UserId + ") lost the coin.");
                return;
            }

            // Determine if a rare item should be given
            if (GetRandomIndex(100) < 5) // 5% chance for a rare item
            {
                var rareItemType = GetRandomItem(RareItems);
                ev.Player.AddItem(rareItemType); // Use ItemType directly
                string itemMessage = ev.Player.Nickname + " (" + ev.Player.UserId + ") received a rare item: " + rareItemType + ".";
                Log.Info(itemMessage);
            }
            else if (GetRandomIndex(100) < 1) // 1% chance for a legendary item
            {
                var legendItemType = GetRandomItem(LegendItems);
                ev.Player.AddItem(legendItemType); // Use ItemType directly
                string itemMessage = ev.Player.Nickname + " (" + ev.Player.UserId + ") received a legendary item: " + legendItemType + ".";
                Log.Info(itemMessage);
            }
            else if (GetRandomIndex(100) < 25) // 25% chance for a normal item
            {
                // Normal item reward
                var itemType = GetRandomItem(PossibleItems);
                ev.Player.AddItem(itemType); // Use ItemType directly
                string itemMessage = ev.Player.Nickname + " (" + ev.Player.UserId + ") received a " + itemType + ".";
                Log.Info(itemMessage);
            }
        }
    }
}
