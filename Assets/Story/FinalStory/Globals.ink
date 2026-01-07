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