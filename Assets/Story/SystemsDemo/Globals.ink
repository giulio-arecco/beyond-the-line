VAR VISITED_FARMSTEAD = false
VAR VISITED_CITY = false

EXTERNAL HasItem(itemId)
EXTERNAL HasCompanion(companionId)
EXTERNAL AddItemToInventory(itemId)
EXTERNAL AddCompanionToParty(companionId)
EXTERNAL GetCompanionStat(companionId, statName)
EXTERNAL SetCompanionStat(companionId, statName, statValue)