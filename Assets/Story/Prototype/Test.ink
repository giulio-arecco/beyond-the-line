INCLUDE globals.ink

-> whack_a_mole

=== whack_a_mole ===
	{I heft the hammer.|{~Missed!|Nothing!|No good. Where is he?|Ah-ha! Got him! -> win}}
	The {&mole|{&nasty|blasted|foul} {&creature|rodent}} is {in here somewhere|hiding somewhere|still at large|laughing at me|still unwhacked|doomed}. <>
	{!I'll show him!|But this time he won't escape!}
	* 	[{&Hit|Smash|Try} top-left] 	-> whack_a_mole
	*  [{&Whallop|Splat|Whack} top-right] -> whack_a_mole
	*  [{&Blast|Hammer} middle] -> whack_a_mole
	*  [{&Clobber|Bosh} bottom-left] 	-> whack_a_mole
	*  [{&Nail|Thump} bottom-right] 	-> whack_a_mole
	*   ->
    	    Then you collapse from hunger. The mole has defeated you!
            -> END
            
=== win ===
~ moleHitCount += 1
~ playerHealth -= 25

~ AddItemToInventory("Crowbar")
~ AddItemToInventory("Ammo")
~ AddItemToInventory("Crowbar")
~ AddCompanionToParty("OldFarmer")
~ SetCompanionStat("OldFarmer", "Hunger", 50)

{moleHitCount} {playerHealth} <>

{HasItem("Ammo"):
    You have Ammo, <>   
- else:
    You don't have Ammo, <>
} <>

{HasItem("Crowbar"):
    you have Crowbar <>
- else:
    you don't have Crowbar <>
}

and OldFarmer <>
{GetCompanionStat("OldFarmer", "Hunger") > 0:
    is <>
- else:
    is not <>
}
hungry.

Removing ammo from inventory.
~RemoveItemFromInventory("Ammo", 1)

Removing TestCompanion from party.
~RemoveCompanionFromParty("OldFarmer")

-> END