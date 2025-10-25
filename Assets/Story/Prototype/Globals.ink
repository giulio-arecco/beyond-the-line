VAR moleHitCount = 0
VAR playerHealth = 100

EXTERNAL HasItem(itemId)
EXTERNAL HasCompanion(companionId)
EXTERNAL AddItemToInventory(itemId)
EXTERNAL AddCompanionToParty(companionId)
EXTERNAL GetCompanionStat(companionId, statName)
EXTERNAL SetCompanionStat(companionId, statName, statValue)
EXTERNAL IncreaseGlobalStat(statName, statValue)
EXTERNAL DescreaseGlobalStat(statName, statValue)