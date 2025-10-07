INCLUDE globals.ink

-> farm_arrival

=== farm_arrival ===
Il sole tramonta dietro le colline quando raggiungi una fattoria.<nl><>
Le porte sono sfondate, il vento porta odore di cenere.

Dentro al fienile, un uomo magro sta sistemando uno zaino.\<nl><>
Ti squadra con sospetto, la mano vicina a un forcone.

"Non cercare guai, soldato. Qui non è rimasto molto."

* [Gli chiedi se può accompagnarti verso nord.] -> take_companion
* [Frughi tra le rovine alla ricerca di qualcosa da mangiare.] -> find_ration
* [Prosegui senza dire altro.] -> leave_alone

=== take_companion ===
"Sto andando verso nord," dici, osservando le travi incenerite. "Potrei aver bisogno di qualcuno che conosca la zona."

L’uomo ti fissa a lungo, poi getta un’occhiata al cielo che si oscura.<nl><> 
"Non ho motivo di rimanere," mormora. "Conosco i colori della tua uniforme, verrò con te."

Annuisci. Sai bene che la rovina della fattoria è opera degli stessi uomini che ti stanno dando la caccia.

L'uomo finisce di preparare lo zaino. Prima di chiuderlo, estrae dal fondo un rotolo di garze.
"Prendi queste," dice, porgendoti le bende. "Se le cose si metteranno male, saranno più utili a te che a me".
~AddCompanionToParty("OldFarmer")
-> end_story

=== find_ration ===
Ignorando lo sguardo dell’uomo, rovisti tra le casse rovesciate.

Trovi una vecchia razione militare, ammaccata ma ancora sigillata.<nl><> 
Il silenzio pesa, ma nessuno ti ferma.  
~AddItemToInventory("Ration")
-> end_story

=== leave_alone ===
Ti allontani lungo la strada, mentre il vento scuote le imposte sgangherate.  
Un’altra notte ti attende, e nessuno a cui chiedere riparo.  
-> end_story

=== end_story ===
~VISITED_FARMSTEAD = true
-> END