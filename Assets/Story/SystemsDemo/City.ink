INCLUDE globals.ink

-> city_passage

=== city_passage ===
La strada polverosa conduce alla periferia di una città caduta nel silenzio.<nl><> 
Le case mostrano i segni della guerra, finestre sfondate e insegne spezzate.  

{ HasCompanion("OldFarmer"):
    L’uomo che ti ha accompagnato dalla fattoria osserva attentamente i vicoli. "Meglio fare attenzione, sembra che qualcuno sia passato di qui recentemente."
- else:
    Cammini da solo tra le rovine. Il silenzio è interrotto solo dal rumore dei tuoi passi sulle macerie.
}

* [Ispezioni un edificio abbandonato.] -> search_building
* [Prosegui lungo la strada principale.] -> main_road

=== search_building ===
Ti addentri tra le macerie e trovi un vecchio scaffale di legno.  
Tra polvere e ragnatele scorgi un piccolo sacco di proiettili.  

* [Prendi i proiettili.] -> get_ammo
* [Ignori e torni sulla strada.] -> main_road

=== get_ammo ===
 Rimetti il sacco nello zaino e torni sulla strada principale. 
 ~AddItemToInventory("Ammo")
-> main_road

=== main_road ===
La città si allunga davanti a te, desolata ma percorribile.  
{ HasCompanion("OldFarmer"):
    L’uomo cammina al tuo fianco, con la schiena inarcata a causa dalla fatica.
- else:
    Sei solo, e ogni ombra sembra più minacciosa.
}
-> end_story

=== end_story ===
~VISITED_CITY = true
-> END