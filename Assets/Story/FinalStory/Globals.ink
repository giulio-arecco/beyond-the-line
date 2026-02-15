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
VAR COMPLETED_BORDER = false
VAR END_OF_STORY = false
VAR CAN_SET_CAMP = false

VAR SET_CAMP_COUNT = 0
VAR ELIAS_OPTIONAL_DIALOGUE_DONE = false
VAR LIRA_OPTIONAL_DIALOGUE_DONE = false
VAR KNOWN_MOUNTAINPASS = false
VAR LISTENED_TO_RADIO = false // Listened to the radio in The Burnt Village
VAR KNOWN_MILITARY_ROAD_STATUS = false // Info from radio for the Military Road
VAR KNOWN_MOUNTAINPASS_STATUS = false // Info from radio for the Mountain Pass
VAR READ_NOTEBOOK = false

// --- SUPPORT VARS ---
VAR FATIGUE_CAP = 95

// ========= CPS ANALYTICS TRACKERS ========= 

// --- INFORMATION MANAGEMENT & EXPLORATION ---
// Radio Puzzle: Resolution Method
VAR CPS_Radio_Tried_Blind = false
VAR CPS_Radio_Tried_No_Manual = false
VAR CPS_Radio_Solved_Systematic = false  // True if solved using Frequency + Manual
VAR CPS_Radio_Solved_NoManual = false
VAR CPS_Radio_Broken = false             // True if the radio was broken during attempts

// Lira Interrogation: Deduction Quality
VAR CPS_Lira_Respect = 0
VAR CPS_Lira_Patience = -1
VAR CPS_Lira_Clues_Found_Count = 0
VAR CPS_Lira_Accusation_Strength = 0.0
VAR CPS_Lira_Unmasked_Success = false
VAR CPS_Lira_Traded = false 

// Accumulators (Numeric values for stats/averages)
VAR CPS_Info_Points_Gathered = 0         // Counter: how many optional info pieces were found

// --- STRATEGIC PLANNING & RISK ASSESSMENT ---
// Route Choice
VAR CPS_Route_Discovered_Mountain = false
VAR CPS_Route_Chosen_Road = false
VAR CPS_Route_Chosen_Mountain = false

// Route Preparedness
VAR CPS_Route_Was_Prepared = false       // True if player had the Key Item for the chosen route

// Farmstead choices and consequences
VAR CPS_Farmstead_Pantry_Explored_Fast = false
VAR CPS_Farmstead_Pantry_Explored_Slow = false
VAR CPS_Farmstead_Patrol_Trusted_Elias = false

// Village choices and consequences
VAR CPS_Village_Entered_Shelter = false
VAR CPS_Village_Entered_Barn = false
VAR CPS_Village_Shelter_Used_Crowbar = false
VAR CPS_Village_Shelter_Used_Hands = false
VAR CPS_Village_Scavenger_Traded = false 
VAR CPS_Village_Scavenger_Threat_Success = false
VAR CPS_Village_Scavenger_Threat_Fail = false
VAR CPS_Village_Scavenger_Failed = false 

// Military Road choices and consequences
VAR CPS_MilitaryRoad_Route_Sewer = false
VAR CPS_MilitaryRoad_Route_Warehouses = false
VAR CPS_MilitaryRoad_Warehouse_Opened_Crowbar = false
VAR CPS_MilitaryRoad_Warehouse_Opened_Hands = false
VAR CPS_MilitaryRoad_Warehouse_NotOpened = false
VAR CPS_MilitaryRoad_Sewer_Corpses_Climbed = false
VAR CPS_MilitaryRoad_TruckActive_Chose_Smoke = false
VAR CPS_MilitaryRoad_TruckActive_Chose_Stealth = false
VAR CPS_MilitaryRoad_TruckActive_Chose_Wait = false
VAR CPS_MilitaryRoad_TruckPassive_Chose_Dive = false
VAR CPS_MilitaryRoad_TruckPassive_Chose_Noise = false
VAR CPS_MilitaryRoad_TruckPassive_Chose_Wait = false
VAR CPS_MilitaryRoad_Escape_Chose_Smoke = false
VAR CPS_MilitaryRoad_Escape_Chose_Pistol = false
VAR CPS_MilitaryRoad_Escape_Chose_Run = false
VAR CPS_MilitaryRoad_Alerted = false

// Mountain Pass choices and consequences
VAR CPS_MountainPass_Outpost_Opened_Crowbar = false
VAR CPS_MountainPass_Outpost_Opened_Together = false
VAR CPS_MountainPass_Outpost_Opened_Hands = false
VAR CPS_MountainPass_Bridge_Cable_Fixed = false
VAR CPS_MountainPass_Bridge_Crossing_Crawl = false
VAR CPS_MountainPass_Bridge_Crossing_Cable = false
VAR CPS_MountainPass_Bridge_Crossing_Run = false
VAR CPS_MountainPass_Tunnel_Split_Teams = false
VAR CPS_MountainPass_Tunnel_Share_Mask = false
VAR CPS_MountainPass_Tunnel_Group_Run = false
VAR CPS_MountainPass_Tunnel_Group_Run_Went_Back = false
VAR CPS_MountainPass_Tunnel_Solo_Mask = false
VAR CPS_MountainPass_Tunnel_Solo_Mask_Wheel_Hands = false
VAR CPS_MountainPass_Tunnel_Solo_Mask_Wheel_Crowbar = false
VAR CPS_MountainPass_Tunnel_Solo_Run = false
VAR CPS_MountainPass_Hermit_Threatened = false

// Border choices and consequences
VAR CPS_Border_Help_Boy = false
VAR CPS_Border_Ignore_Boy = false

// Final Ending 
VAR CPS_Reached_Ending_1 = false         // Group Ending
VAR CPS_Reached_Ending_2 = false         // Solo Ending

// Camp management
VAR CPS_Camp_Set_Count = 0
VAR CPS_Camp_First_Location = ""
VAR CPS_Camp_Second_Location = ""

// --- RESOURCE MANAGEMENT & OPTIMIZATION ---
// Resources used in camp
VAR CPS_Camp_Used_Rations = 0
VAR CPS_Camp_Used_Bandages = 0
VAR CPS_Camp_Used_Medikits = 0
 
// Optional resources found
VAR CPS_Found_Crowbar = false
VAR CPS_Found_FrequencyNote = false
VAR CPS_Found_DecryptionManual = false
VAR CPS_Found_Medikit = false
VAR CPS_Found_GasMask = false
VAR CPS_Found_SmokeGrenade = false

// Accumulators
VAR CPS_Min_Health_Value = 100           // Absolute minimum Health reached 
VAR CPS_Max_Fatigue_Value = 0            // Absolute maximum Fatigue reached

// Fatigue stat tracking
VAR CPS_Fatigue_Farmstead_Entry = -1
VAR CPS_Fatigue_Wood_Entry = -1
VAR CPS_Fatigue_Village_Entry = -1
VAR CPS_Fatigue_MilitaryRoad_Entry = -1
VAR CPS_Fatigue_MountainPass_Entry = -1
VAR CPS_Fatigue_Border_Entry = -1

// Collapse chances
VAR CPS_Collapse_Village_Shelter = false
VAR CPS_Collapse_MilitaryRoad_SewerClimb = false
VAR CPS_Collapse_MilitaryRoad_Escape = false
VAR CPS_Collapse_MountainPass_TunnelPhysical = false
VAR CPS_Collapse_MountainPass_TunnelHypoxia = false

// --- SOCIAL DYNAMICS & OUTCOMES ---
// Elias
VAR CPS_Elias_Recruited = false          // Reached the end alive in the party
VAR CPS_Elias_Abandoned_Aggressive = false
VAR CPS_Elias_Abandoned_Avoidant = false
VAR CPS_Elias_Confrontation_Asked = false
VAR CPS_Elias_Confrontation_Silence = false
VAR CPS_Elias_Confrontation_Thanked = false
VAR CPS_Elias_Died_Sacrifice = false     // Died fighting to cover player's escape
VAR CPS_Elias_Died_Separation = false    // Died/Lost after getting separated during escape
VAR CPS_Elias_Bridge_Tried_Saving = false
VAR CPS_Elias_Bridge_Fall = false
VAR CPS_Elias_Camp_Dialogue_Done = false
VAR CPS_Elias_Notebook_Translated = false

// Lira
VAR CPS_Lira_Recruited = false           // True if Lira joined the party
VAR CPS_Lira_Camp_Dialogue_Done = false

// Ethical Tracking
// VAR CPS_Humanity_Score_Final = 0         // Numeric score tracking empathy vs ruthlessness throughout the game

// ========= EXTERNAL FUNCTIONS (UNITY API) =========
// These functions must be mapped to a C# implementation in Unity. Use these functions to interface with the global stats, the inventory and the companions.

EXTERNAL HasItem(itemId)
EXTERNAL HasCompanion(companionId)
EXTERNAL AddItemToInventory(itemId)
EXTERNAL RemoveItemFromInventory(itemId, count)
EXTERNAL AddCompanionToParty(companionId)
EXTERNAL RemoveCompanionFromParty(companionId)
EXTERNAL GetGlobalStat(statName)
EXTERNAL IncreaseGlobalStat_Internal(statName, statValue)
EXTERNAL DecreaseGlobalStat_Internal(statName, statValue)
EXTERNAL PlayMusic_Internal(trackId, transitionType, transitionDuration, volume)
EXTERNAL StopMusic_Internal(transitionDuration)
EXTERNAL Log(message)
EXTERNAL LogWarning(message)
EXTERNAL LogError(message)

// ========= FUNCTION WRAPPERS =========
=== function IncreaseGlobalStat(statName, statValue)
    ~ IncreaseGlobalStat_Internal(statName, statValue)
    
    ~ temp currentValue = GetGlobalStat(statName)
    ~ UpdateReachedStatBounds(statName, currentValue)
    
=== function DecreaseGlobalStat(statName, statValue)
    ~ DecreaseGlobalStat_Internal(statName, statValue)
    
    ~ temp currentValue = GetGlobalStat(statName)
    ~ UpdateReachedStatBounds(statName, currentValue)

=== function IncreaseGlobalStatCapped(statName, amountToAdd, maxValue)
    ~ temp currentValue = GetGlobalStat(statName)
    
    { currentValue >= maxValue:
        ~ LogWarning("[Ink Story] Stat Clamped: {statName} - Added: {amountToAdd}, Value: {currentValue}, Clamped to: {maxValue}")
        ~ return
    }
    
    { currentValue + amountToAdd >= maxValue:
        ~ IncreaseGlobalStat(statName, maxValue - currentValue)
        ~ LogWarning("[Ink Story] Stat Clamped: {statName} - Added: {amountToAdd}, Value: {currentValue}, Clamped to: {maxValue}")
    - else:
        ~ IncreaseGlobalStat(statName, amountToAdd)
    }

// Play and Stop Music Functions Wrappers
=== function PlayMusic(trackId, transitionType)
    ~ PlayMusic_Internal(trackId, transitionType, -1.0, -1.0)

=== function PlayMusicCustomTransition(trackId, transitionType, customDuration)
    ~ PlayMusic_Internal(trackId, transitionType, customDuration, -1.0)
    
=== function PlayMusicCustomVolume(trackId, transitionType, customVolume)
    ~ PlayMusic_Internal(trackId, transitionType, -1.0, customVolume)
    
=== function PlayMusicCustom(trackId, transitionType, customDuration, customVolume)
    ~ PlayMusic_Internal(trackId, transitionType, customDuration, customVolume)
    
=== function StopMusic() ===
    ~ StopMusic_Internal(-1.0)

=== function StopMusicCustomDuration(customDuration)
    ~ StopMusic_Internal(customDuration)
    

// ========= UTILITY FUNCTIONS =========
=== function came_from(-> x) 
    ~ return TURNS_SINCE(x) == 0
    
=== function UpdateReachedStatBounds(statName, currentValue)
    { statName == "Health" and currentValue < CPS_Min_Health_Value: 
        ~ CPS_Min_Health_Value = currentValue 
    }
    { statName == "Fatigue" and currentValue > CPS_Max_Fatigue_Value: 
        ~ CPS_Max_Fatigue_Value = currentValue
    }

// ========= FALLBACK FUNCTIONS FOR TESTING USING INKY =========
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
    
// === function IncreaseGlobalStat(statName, statValue)
//     {
//         - statName == "Health": 
//             ~ Health += statValue
//             >> Increased Health. Current value: {Health}
//         - statName == "Fatigue": 
//             ~ Fatigue += statValue
//             >> Increased Fatigue. Current value: {Fatigue}
//         - statName == "Cohesion": 
//             ~ Cohesion += statValue
//             >> Increased Cohesion. Current value: {Cohesion}
//         - else:
//             >> Undefined statName "{statName}" passed to IncreaseGlobalStat.
//             ~ return 0
//     }
    
// === function DecreaseGlobalStat(statName, statValue)
//     {
//         - statName == "Health": 
//             ~ Health -= statValue
//             >> Decreased Health. Current value: {Health}
//         - statName == "Fatigue": 
//             ~ Fatigue -= statValue
//             >> Decreased Fatigue. Current value: {Fatigue}
//         - statName == "Cohesion": 
//             ~ Cohesion -= statValue
//             >> Decreased Cohesion. Current value: {Cohesion}
//         - else:
//             >> Undefined statName "{statName}" passed to DecreaseGlobalStat.
//             ~ return 0
//     }
    
// === function IncreaseGlobalStatCapped(statName, amountToAdd, maxValue)
//     {
//     - statName == "Health": 
//             {Health + amountToAdd >= maxValue: 
//                 >> Health clamped to {Health}.
//             - else:
//                 ~ Health += amountToAdd
//                 >> Increased Health. Current value: {Health}
//             }
//         - statName == "Fatigue": 
//             {Fatigue + amountToAdd >= maxValue: 
//                 >> Fatigue clamped to {Fatigue}.
//             - else:
//                 ~ Fatigue += amountToAdd
//                 >> Increased Fatigue. Current value: {Fatigue}
//             }
//         - statName == "Cohesion": 
//             {Cohesion + amountToAdd >= maxValue: 
//                 >> Cohesion clamped to {Cohesion}.
//             - else:
//                 ~ Cohesion += amountToAdd
//                 >> Increased Cohesion. Current value: {Cohesion}
//             }
//         - else:
//             >> Undefined statName "{statName}" passed to IncreaseGlobalStatCapped.
//             ~ return 0
//     }