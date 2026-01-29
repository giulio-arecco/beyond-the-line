// --- VISIBLE GLOBAL STATS (only used for testing) ---
VAR Health = 70      // Range: 0-100
VAR Fatigue = 30     // Range: 0-100

// --- HIDDEN GLOBAL STATS (only used for testing) ---
VAR Cohesion = 0     // Range: 0-100

// --- NARRATIVE FLAGS ---
VAR COMPLETED_FARMSTEAD = false
VAR COMPLETED_WOOD = false
VAR COMPLETED_VILLAGE = false
VAR COMPLETED_ROAD = false
VAR COMPLETED_MOUNTAINPASS = false
VAR COMPLETED_ENDING = false
VAR CAN_SET_CAMP = false

VAR SET_CAMP_COUNT = 0
VAR ELIAS_OPTIONAL_DIALOGUE_DONE = false
VAR LIRA_OPTIONAL_DIALOGUE_DONE = false
VAR KNOWN_MOUNTAINPASS = false
VAR LISTENED_TO_RADIO = false // Listened to the radio in The Burnt Village
VAR KNOWN_MILITARY_ROAD_STATUS = false // Info from radio for the Military Road
VAR KNOWN_MOUNTAINPASS_STATUS = false // Info from radio for the Mountain Pass
VAR READ_NOTEBOOK = false

// --- EXTERNAL FUNCTIONS (UNITY API) ---
// These functions must be mapped to a C# implementation in Unity. Use these functions to interface with the global stats, the inventory and the companions.

EXTERNAL HasItem(itemId)
EXTERNAL HasCompanion(companionId)
EXTERNAL AddItemToInventory(itemId)
EXTERNAL RemoveItemFromInventory(itemId, count)
EXTERNAL AddCompanionToParty(companionId)
EXTERNAL RemoveCompanionFromParty(companionId)
EXTERNAL GetGlobalStat(statName)
EXTERNAL IncreaseGlobalStat(statName, statValue)
EXTERNAL DecreaseGlobalStat(statName, statValue)
EXTERNAL PlayMusic(trackId, transitionType)
EXTERNAL StopMusic()

// UTILITY FUNCTIONS
=== function came_from(-> x) 
    ~ return TURNS_SINCE(x) == 0

// --- FALLBACK FUNCTIONS FOR TESTING USING INKY ---
=== function HasItem(itemId)
    ~ return true // Default per test
    
=== function HasCompanion(companionId)
    ~ return true // Default per test
    
=== function AddItemToInventory(itemId)
    >> Added {itemId} to inventory. 
    ~ return
    
=== function RemoveItemFromInventory(itemId, count)
    >> Removed {count} {itemId} from inventory. 
    ~ return
    
=== function AddCompanionToParty(companionId)
    >> Added {companionId} to companions.
    ~ return

=== function RemoveCompanionFromParty(companionId)
    >> Removed {companionId} from companions. 
    ~ return
    
=== function GetGlobalStat(statName)
    {
        - statName == "Health": ~ return Health
        - statName == "Fatigue": ~ return Fatigue
        - statName == "Cohesion": ~ return Cohesion
        - else:
            >> Undefined statName "{statName}" passed to GetGlobalStat.
            ~ return 0
    }
    
=== function IncreaseGlobalStat(statName, statValue)
    {
        - statName == "Health": 
            ~ Health += statValue
            >> Increased Health. Current value: {Health}
        - statName == "Fatigue": 
            ~ Fatigue += statValue
            >> Increased Fatigue. Current value: {Fatigue}
        - statName == "Cohesion": 
            ~ Cohesion += statValue
            >> Increased Cohesion. Current value: {Cohesion}
        - else:
            >> Undefined statName "{statName}" passed to IncreaseGlobalStat.
            ~ return 0
    }
    
=== function DecreaseGlobalStat(statName, statValue)
    {
        - statName == "Health": 
            ~ Health -= statValue
            >> Decreased Health. Current value: {Health}
        - statName == "Fatigue": 
            ~ Fatigue -= statValue
            >> Decreased Fatigue. Current value: {Fatigue}
        - statName == "Cohesion": 
            ~ Cohesion -= statValue
            >> Decreased Cohesion. Current value: {Cohesion}
        - else:
            >> Undefined statName "{statName}" passed to DecreaseGlobalStat.
            ~ return 0
    }