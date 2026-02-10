INCLUDE Globals.ink

// ============================================================
// SECTION III: THE BURNT VILLAGE
// ============================================================

// --- Local Logic Variables ---
VAR shelter_open = false
VAR looted_medikit = false
VAR has_frequency = false
VAR has_manual = false
VAR radio_fixed = false
VAR radio_broken = false
VAR failed_negotiation = false

VAR radio_attempts = 0
VAR threat_power = 0

-> village_entry

=== village_entry ===
~ CPS_Fatigue_Village_Entry = GetGlobalStat("Fatigue")

~ PlayMusic("TheVillage", 2)

Le prime luci dell'alba filtrano tra i rami come lame grigie, dissolvendo le ombre del bosco. <nl><>
Cammini da ore. La stanchezza ti morde i polpacci, ma è la vista dell'orizzonte a farti fermare. Là dove gli alberi finiscono, la valle si apre su una macchia scura, immobile sotto il cielo pallido.

Un villaggio. O quello che ne resta. Da questa distanza sembra un giocattolo rotto abbandonato nel fango. Non c'è fumo, nessuna luce, nessun movimento.

{ HasCompanion("Elias"):
    Elias si ferma accanto a te, il respiro corto che si condensa nell'aria gelida. Socchiude gli occhi, riconoscendo la sagoma delle case. <nl><>
    "Krostov..." mormora. Il nome esce dalle sue labbra come una sentenza. Indica i pendii scoscesi ai lati della valle e scuote la testa: impossibile arrampicarsi lì. Poi traccia una linea immaginaria con il dito che attraversa il centro delle rovine. Ti guarda e fa un gesto inequivocabile con la mano aperta in avanti. Non servono traduzioni: l'unica via passa lì in mezzo.
}

{ HasCompanion("Lira"):
    Lira osserva il villaggio con la freddezza tattica dell'agente sotto copertura. <nl><>
    "Niente sentinelle sui tetti. Niente calore" riporta con voce atona. <nl><>
    "È sulla nostra rotta. Se c'è rimasto qualcosa di utile, una radio o una mappa, è lì che lo troveremo. Meglio rischiare tra le rovine che morire di fame nel bosco".
}

{ not HasCompanion("Lira") && not HasCompanion("Elias"):
    Sei solo. Guardi quella distesa di rovine e calcoli le tue opzioni. Aggirarlo ti costerebbe ore preziose e energie che non hai. Attraversarlo è un rischio, ma anche l'unica possibilità di trovare provviste o informazioni per il confine. La strada passa di lì.
}

Ti rimetti in marcia. L'avvicinamento è lento, accompagnato solo dal rumore dei tuoi passi sulla terra che diventa via via più nera.

Quando arrivi ai primi edifici, l'atmosfera cambia. Non c'è odore di bruciato recente, niente puzza di fuliggine nell'aria. L'incendio si è spento da giorni. L'aria è limpida, gelida, cristallizzata in un silenzio assoluto.

Davanti a te si apre la piazza principale. Tre strutture emergono dal caos, ma una domina sulle altre. <nl><>
A nord, su una piccola altura, la casa di comando in pietra incombe sul villaggio. Un'antenna militare, piegata ma ancora in posizione, pende dal comignolo.

{ HasCompanion("Lira"):
    Lira indica l'antenna. "Quella è la priorità" sussurra. "Se c'è un modo per capire cosa ci aspetta al confine, è lassù. Il resto può attendere".
}
{ HasCompanion("Elias"):
    Elias fissa l'edificio sulla collina. Sa che da lì venivano impartiti molti degli ordini che era abituato a ricevere. Annuisce verso l'altura.
}
{ not HasCompanion("Lira") && not HasCompanion("Elias"):
    Quell'antenna è la tua migliore speranza. Prima di rischiare di farti seppellire dalle macerie degli altri edifici, devi capire se c'è un modo per contattare qualcuno o intercettare informazioni.
}

-> village_hub

=== village_hub ===
Ti trovi al centro della piazza devastata. L'aria è ferma, fredda.

// --- PHASE 1: Must visit Command House first ---
{ command_house_entry_first == 0:
    * [Sali verso la casa di comando.]
        -> command_house_entry_first
    
- else: 
// --- PHASE 2: Exploration unlocked ---
    { command_house_entry_first == 1 and command_house_entry_again == 0 and shelter_entry_first == 0 and barn_entry == 0 and not radio_broken and not radio_fixed:
        La radio alla casa di comando è la chiave, ma ti mancano i pezzi del puzzle: una frequenza da ascoltare e un qualche codice di decrittazione. <nl><>
        L'unica speranza è trovarli tra le rovine rimaste nella piazza.
    }
    {radio_fixed or radio_broken:
        Hai fatto ciò che potevi con la radio. Ora non resta che decidere come procedere.

        * [Raduni le idee e pianifichi la prossima mossa.]
            -> final_planning
    }

    + {command_house_entry_first > 0 and not radio_fixed and not radio_broken} [Torni alla casa di comando.]
        -> command_house_entry_again
    * {shelter_entry_first == 0} [Vai verso l'ingresso del rifugio sotterraneo.]
        -> shelter_entry_first
    + {shelter_entry_first > 0 && not shelter_open} [Torni all'ingresso del rifugio sotterraneo.]
        -> shelter_entry_again
    * {barn_entry == 0} [Controlli il granaio.]
        -> barn_entry
    + {barn_entry > 0 and not has_manual and not failed_negotiation} [Torni al granaio]
        -> barn_entry     
}



=== shelter_entry_first ===
Ti avvicini alla croce medica dipinta su una lamiera. Questo doveva essere il punto di raccolta civili. <nl><>
L'ingresso del seminterrato è una botola di ferro rinforzato, ma è sepolta sotto una tonnellata di travi e mattoni crollati dalla facciata dell'edificio adiacente. <nl><>
Senti un odore dolciastro, nauseante, filtrare dalle fessure.

{ HasCompanion("Elias"):
    Elias si copre naso e bocca con la manica, indietreggiando di un passo. Sa cosa c'è là sotto.
}

Il crollo è massiccio. Spostare quei detriti richiederà uno sforzo immane.

* {HasItem("Crowbar")} [Usi il piede di porco come leva.]
    Incastri il piede di porco sotto la trave portante che blocca la botola. Il metallo stride nel silenzio, si flette, ma la fisica è dalla tua parte. <nl><>
    Con uno scricchiolio secco, la trave si sposta quel tanto che basta per far scivolare via i mattoni. Non hai sprecato energie preziose.
    ~ IncreaseGlobalStat("Fatigue", 5)
    ~ shelter_open = true
    -> shelter_interior

* [Provi a spostare le macerie a mani nude.]
    ~ IncreaseGlobalStat("Fatigue", 25)
    { GetGlobalStat("Fatigue") >= 100:
        -> shelter_collapse_scenario
    - else:
        -> shelter_success_hands
    }

=== shelter_entry_again ===
Torni davanti al cumulo di macerie. Le pietre sembrano guardarti con disprezzo, testimoni silenziosi del tuo precedente fallimento. Senti ancora i muscoli che tremano per il collasso, il sudore freddo sulla schiena. <nl><>
Guardando meglio, vedi che il lavoro è quasi finito. Manca poco per liberare la botola. <nl><>
Devi solo stringere i denti un'ultima volta.

* {HasItem("Crowbar")} [Usi il piede di porco per finire il lavoro.]
    Non hai intenzione di rischiare di nuovo. Usi la leva d'acciaio per spostare gli ultimi detriti.
    ~ IncreaseGlobalStat("Fatigue", 5)
    ~ shelter_open = true
    -> shelter_interior

* [Sposti gli ultimi detriti a mani nude.]
    Ti pieghi sulle ginocchia doloranti. Afferri le pietre rimaste con rabbia sorda. Uno dopo l'altro, i mattoni volano via.
    ~ IncreaseGlobalStat("Fatigue", 10) // Reduced cost for retry/completion
    ~ shelter_open = true
    -> shelter_interior

=== shelter_success_hands ===
Non hai attrezzi. Ti tocca usare la forza bruta. Afferri i mattoni grezzi e freddi, i calcinacci ti tagliano la pelle delle mani. Spingi con le gambe, la schiena che urla di dolore, i polmoni che bruciano per la polvere sollevata. <nl><>
Impieghi quasi un'ora di lavoro sfiancante per liberare l'accesso.

Quando hai finito, sei coperto di sudore che si gela addosso e tremi per lo sforzo.
~ shelter_open = true
-> shelter_interior

=== shelter_collapse_scenario ===
~ CPS_Collapse_Village_Shelter = true
Non hai attrezzi. Ti tocca usare la forza bruta.

Afferri i mattoni grezzi, spingi, tiri. Ma il tuo corpo ha un limite. E oggi l'hai raggiunto. Il respiro si blocca. Un fischio acuto ti riempie le orecchie, coprendo il rumore delle pietre che rotolano. <nl><>
Le gambe cedono all'improvviso. Il mondo si inclina e diventa nero.

// Fatigue Reset Logic: Fatigue hits limit (100) and recovers to 60.
~ temp current_f = GetGlobalStat("Fatigue")
~ temp fatigue_reset = current_f - 60
~ DecreaseGlobalStat("Fatigue", fatigue_reset)

{ HasCompanion("Lira") or HasCompanion("Elias"):
    -> shelter_collapse_companions
- else:
    -> shelter_collapse_solo
}

=== shelter_collapse_companions ===
Ti risvegli con la vista annebbiata. Qualcuno ti sta scuotendo. <nl><>

{ HasCompanion("Lira"):
    Lira ti sovrasta, il volto contorto dalla rabbia. "Idiota" sibila. "Sei svenuto come un principiante. Potevi farci scoprire". <nl><>
    Ha finito lei il lavoro, ma ti guarda con disprezzo.
}

{ HasCompanion("Elias"):
Elias ti aiuta a sederti. Non dice nulla, ma il suo sguardo è grave. {not HasCompanion("Lira"): Ha spostato lui le macerie rimaste.} Vedi la delusione nei suoi occhi: un soldato deve conoscere i propri limiti.
}

Il varco è aperto, ma la fiducia <>
{
    - HasCompanion("Elias") && HasCompanion("Lira"):
        del gruppo
    - HasCompanion("Elias"):
        del tuo compagno
    - HasCompanion("Lira"):
        della tua compagna
} <>
nelle tue capacità è incrinata.

~ DecreaseGlobalStat("Cohesion", 20)
~ shelter_open = true
-> shelter_interior

=== shelter_collapse_solo ===
Ti risvegli riverso nella polvere gelida. Quanto tempo è passato? Il freddo ti è entrato nelle ossa. <nl><>
Ti rialzi a fatica, tremante. Il battito cardiaco ti rimbomba nelle tempie.

* [Cerchi di spostare i detriri rimasti.]
    Ti rimetti al lavoro, muovendoti più lentamente. Sposti i detriti rimasti uno ad uno, dosando le ultime energie.
    ~ IncreaseGlobalStat("Fatigue", 10) // Reduced cost for retry
    ~ shelter_open = true
    -> shelter_interior
* [Sei esausto. Cerchi altrove.] // Option to leave and potentially return later
    Ti allontani barcollando. Non hai la forza, non ora.
    -> village_hub


=== shelter_interior ===
~ CPS_Village_Entered_Shelter = true

Scendi la scala di cemento. Il buio ti inghiotte dopo pochi gradini. L'aria è ferma, pesante. Non hai una torcia. Devi fidarti del tatto.

Le tue dita scorrono sul cemento ruvido e umido della parete, cercando un interruttore. <nl><>
Lo trovi. Una scatola di metallo freddo.

* [Premi l'interruttore.]
    Click.
    
    Per un secondo, non succede nulla. Poi, un ronzio elettrico, simile a un lamento. Le luci di emergenza, ingabbiate nel soffitto, iniziano a sfarfallare. Un bagliore giallo, intermittente, che fa danzare le ombre. <nl><>
    Infine, con uno scoppiettio secco, la luce si stabilizza, rivelando l'orrore.
    
    Il rifugio è pieno di corpi. Civili, per lo più. Donne, anziani. Sono seduti contro i muri, immobili come statue di cera. Non ci sono segni di violenza sui loro corpi: l'ossigeno è finito giorni fa, e si sono addormentati per sempre nel buio.

    { HasCompanion("Elias"):
        Elias si copre la bocca con la mano, soffocando un suono strozzato. I suoi occhi scorrono sulle scritte incise sui muri: non serve un traduttore per intuire che si tratta di nomi, date e preghiere nella sua lingua. 
        
        Si ferma davanti a una scritta fatta col carbone. Parole spigolose, nella lingua del nemico, che non sai decifrare. Le tocca con le dita tremanti. "Nikto" sussurra. Poi ti guarda, gli occhi vuoti, cercando le parole giuste nella tua lingua. <nl><>
        "Nessuno... venuto". Si appoggia al muro, schiacciato dal peso della colpa.
    }

    Tra i civili, noti due figure in uniforme che stonano con il resto. C'è un corpo rannicchiato in un angolo, che stringe una cartella di cuoio. Indossa le cuffie da operatore radio: è un tecnico nemico. <nl><>
    Dall'altra parte, steso vicino all'ingresso come se avesse cercato di uscire all'ultimo, c'è un soldato con l'uniforme della tua fazione. <nl><>
    Di fronte a te, noti anche una cassa medica fissata alla parete.
    
    -> shelter_choices
    
=== shelter_choices ===
* [Esamini il tecnico nemico.]
    -> examine_technician
* [Esamini il soldato alleato.]
    -> examine_deserter
* [Controlli la cassa medica alla parete.]
    -> loot_medikit
* {has_frequency} [Risali in superficie.]
    Lasci quel luogo di morte con un peso sul petto.
    {HasCompanion("Elias"):
        Elias si ferma di fronte alle scale prima di risalire, voltandosi verso i suoi compatrioti. Rimane in silenzio per qualche secondo, fissando la scena. Poi, senza dire nulla, si volta e ti segue.
    }
    -> village_hub

=== examine_technician ===
L'operatore radio è morto stringendo la sua borsa.

Lo perquisisci. Non trovi cibo o armi, ma nella sua mano rigida c'è un foglio stropicciato.È un rapporto operativo, scritto interamente nella lingua del nemico. Le parole sono sequenze incomprensibili di consonanti dure. <nl><>
Tuttavia, il tuo occhio cade su un dettaglio cerchiato frettolosamente a matita rossa. In mezzo al testo straniero, i numeri e le unità di misura sono universali: "44.0 MHz" <nl><>
Non ti serve conoscere la loro grammatica per capire che è una frequenza prioritaria.

~ has_frequency = true
~ AddItemToInventory("FrequencyNote")
~ CPS_Found_FrequencyNote++

~ CPS_Info_Points_Gathered++
-> shelter_choices

=== examine_deserter ===
Il soldato indossa la tua divisa, ma è privo di gradi. È morto abbracciato a una donna civile e a un bambino.

{ HasCompanion("Lira"):
    Lira si avvicina e sbianca. La sua solita freddezza si incrina davanti a quella vista.
    
    "Lo conosco" sussurra, la voce incrinata. "Era il Tenente Kael. È... ha disertato due settimane fa. Pensavamo avesse venduto informazioni". <nl><>
    Guarda la donna e il bambino morti tra le sue braccia con un misto di pietà e rabbia. <nl><>
    "Invece era qui. A cercare di portarli via". 
    
    Lira si china con rispetto e sgancia uno degli stivali del tenente. Dal doppio fondo estrae una mappa piegata meticolosamente. La apre, studiando le curve di livello con occhio esperto. <nl><>
    "Kael era un alpino" mormora, quasi parlasse a se stessa. "Questa non è una mappa standard. Segna il vecchio valico montano, attraverso le miniere dismesse".
    
    Alza lo sguardo su di te, ricalcolando le probabilità. "Non l'avevo preso in considerazione. Tutti gli occhi sono puntati sulla strada principale nella valle. Ma se lui credeva di poter far passare una donna e un bambino di qui... forse è una via che i convogli ignorano. È un rischio, ma è un'alternativa tattica valida".
    
    ~ KNOWN_MOUNTAINPASS = true
    ~ AddItemToInventory("MountainMap")
    
    ~ CPS_Info_Points_Gathered++
- else:
    Lo perquisisci. Trovi solo una foto rovinata e una lettera mai spedita. Nessuna informazione utile, solo un'altra tragedia anonima che la guerra inghiottirà.
}
-> shelter_choices

=== loot_medikit ===
Ti avvicini alla cassetta metallica rettangolare con un simbolo medico bianco e rosso. È coperta di polvere. <nl><>
Si tratta di un dispensario d'emergenza standard.

Sblocchi il gancio arrugginito.
All'interno, tra bende secche e flaconi rotti, trovi un kit medico d'emergenza ancora sigillato.

~ looted_medikit = true
~ AddItemToInventory("Medikit")
~ CPS_Found_Medikit++
-> shelter_choices

=== barn_entry ===
{ barn_entry == 1:
    ~ CPS_Village_Entered_Barn = true
    
    Le porte del granaio sono chiuse, ma senti dei rumori all'interno. Qualcuno sta spostando casse pesanti. Il suono è netto nel silenzio generale.
    { HasCompanion("Lira"):
        Lira ti fa cenno di tacere, indicando col dito la fessura tra le assi. "Qualcuno è vivo lì dentro" sussurra. "E non è un soldato. Troppo rumore".
    }
    
    Bussi o entri con forza? <nl><>
    Non serve decidere. Una voce rauca grida dall'interno: "Se entrate armati, vi sparo a vista. Niente scherzi!"
    
    Spingi la porta lentamente, entrando con le mani ben in vista, i palmi aperti. <nl><>
    "Niente armi" dici a voce alta.
    
    {HasCompanion("Lira"):
        Lira non fa lo stesso e tiene la mano destra pronta ad afferrare la pistola dalla fondina.
    }
    
    L'interno è un magazzino improvvisato, illuminato solo da una lanterna a olio posta sul fondo. Casse di razioni, vestiti e elettronica militare sono accatastate lungo le pareti, ma un largo spazio vuoto ti separa dal proprietario.
    
    Seduto su una cassa nell'ombra, una vecchia pistola puntata saldamente verso il tuo petto, c'è un uomo. Indossa abiti civili pesanti sopra brandelli di uniformi diverse. Un sopravvissuto. Uno sciacallo.
    
    "Fermo lì" abbaia, notando il movimento. "Si guarda con gli occhi, non con le mani". <nl><>
    Ti scruta, gli occhi piccoli e lucidi che valutano le uniformi logore e l'equipaggiamento.
    
    { HasCompanion("Lira") or HasCompanion("Elias"):
        "Siete in gruppo" mormora, calcolando le probabilità. "Disertori? Sbandati? Non mi importa, finché state al vostro posto". <nl><>
        { HasItem("Pistol"): 
            Nota la tua arma. Il suo dito si stringe sul grilletto, ma non spara. È in inferiorità numerica, non gli conviene fare mosse azzardate. <nl><>
        }
        "Clienti. Meglio dei soldati della guarnigione che tornano a reclamare il maltolto".
    - else:
        "Solo uno?" ridacchia, rilassandosi appena. "Un cane sciolto". Abbassa leggermente l'arma, ma il dito resta sul grilletto. <nl><>
        "Un cliente. Meglio dei soldati della guarnigione che tornano a reclamare il maltolto".
    }
    
    Indica con la canna dell'arma le casse a metà strada tra voi e lui. "Lì c'è quello che ho. Cibo, medicine..." <nl><>
    Col piede, spinge un oggetto rilegato in pelle che scivola sul pavimento polveroso, fermandosi a pochi passi da te, ben lontano dalla sua posizione. <nl><>
    "...e roba per intenditori. L'ho recuperato alla casa di comando. Manuale tattico di campo".
    
    Prendi il libretto da terra, sotto il suo sguardo vigile. La copertina rigida è macchiata di grasso. All'interno, non è un semplice libro: è una densa raccolta di tabelle di calibrazione radio, codici di frequenza e, soprattutto, pagine intere di simbologia standardizzata. A ogni parola in lingua nemica corrisponde un'icona tattica universale.
    
    { HasCompanion("Lira"):
        Lira non stacca gli occhi dall'arma dell'uomo, ma getta un'occhiata alle pagine. <nl><>
        "Dotazione standard" commenta secca. "Contiene i protocolli di comunicazione. Posso tradurre qualcosa".
    }
    { HasCompanion("Elias"):
        Elias osserva il manuale con un misto di familiarità e repulsione. Conosce quei simboli. Erano la lingua della sua vita precedente.
    }
    { not HasCompanion("Lira") && not HasCompanion("Elias"):
        Senza conoscere la lingua, è un labirinto di segni. Ma noti che le tabelle associano termini fonetici a pittogrammi chiari. Se riesci a capire come leggerlo, potrebbe essere il tuo dizionario.
    }
    
    Lo sciacallo sorride nell'ombra, mostrando denti gialli. "Inutile per me, ma scommetto che a <>
    {HasCompanion("Lira") or HasCompanion("Elias"): 
        voi <>
    -else: 
        te <>
    }
    possono servire". <nl><>
    
    { HasCompanion("Elias"):
        Elias ringhia qualcosa a bassa voce, i pugni stretti lungo i fianchi. Odia chi profana le case della sua gente ancor più dei nemici che le bombardano.
    }
    { HasCompanion("Lira"):
        Lira lo guarda con disprezzo assoluto. "Parassita" mormora, abbastanza forte da farsi sentire. "Ingrassi sui cadaveri".
    }
    
    L'uomo non si fa intimidire. <nl><>
    "Tutto ha un prezzo. Voglio provviste fresche. O medicine. O forse quell'arma che avete".
- else:
    L'uomo è ancora lì. Sentendoti avvicinare si volta di scatto, imbracciando pistola e accendino. <nl><>
    "Allora, hai trovato qualcosa da offrirmi?"
}

~ temp has_ration = HasItem("Ration")
~ temp has_medikit = HasItem("Medikit")
~ temp has_pistol = HasItem("Pistol")
~ temp can_trade = (has_ration and has_medikit) or (has_ration and has_pistol) or (has_pistol and has_medikit)

{can_trade:
    Potresti offrirgli qualcosa in cambio del manuale, oppure potresti provare a forzare la mano minacciandolo: rischioso, ma se funziona manterresti le provviste. <nl><>
-else:
    Non hai nulla da offrire in cambio del manuale, ma potresti provare a minacciarlo per ottenerlo con la forza. Guardando la sua mano nervosa sul grilletto, sai però che è un gioco pericoloso. <nl><>
}

{ 
- has_pistol and HasCompanion("Lira"):
    Senti il peso rassicurante della Kruger nella fondina. Accanto a te, Lira è immobile, pronta a estrarre in una frazione di secondo. In due, armati e addestrati, rappresentate una minaccia che lui non può ignorare.
    
- has_pistol and not HasCompanion("Lira"):
    Hai un'arma, ma anche lui. È uno stallo. Se estrai, diventerà una gara a chi preme il grilletto per primo.

- not has_pistol and HasCompanion("Lira"):
    Lira è armata, ma tu sei a mani nude. Lui lo ha notato.

- not has_pistol and not HasCompanion("Lira"): 
    Lui ha il dito sul grilletto. Tu hai solo le tue parole e la stanchezza addosso. In un confronto di forza bruta, sei in netto svantaggio.
}

* {can_trade} [Offri risorse per il manuale.]
    Cosa vuoi offrirgli?
    ** {has_ration and has_medikit} [Offri cibo e medikit]
        "Ho del cibo e un kit di primo soccorso" dici, posandoli a terra e spingendoli col piede verso di lui, mantenendo la distanza.
        ~ RemoveItemFromInventory("Ration", 1)
        ~ RemoveItemFromInventory("Medikit", 1)
        
    ** {has_ration and has_pistol} [Offri cibo e pistola]
        "Ho del cibo e un'arma" dici, posandoli a terra e spingendoli col piede verso di lui, mantenendo la distanza.
        ~ RemoveItemFromInventory("Ration", 1)
        ~ RemoveItemFromInventory("Pistol", 1)

    ** {has_pistol and has_medikit} [Offri medikit e pistola]
        "Ho un kit di pronto soccorso e un'arma" dici, posandoli a terra e spingendoli col piede verso di lui, mantenendo la distanza.
        ~ RemoveItemFromInventory("Medikit", 1)
        ~ RemoveItemFromInventory("Pistol", 1)
        
    --
    Lui si alza, controlla la merce rapidamente tenendoti sotto tiro e annuisce. "Accettabile. Il manuale è tuo". <nl><>
    "Piacere di fare affari".
    
    ~ AddItemToInventory("DecryptionManual")
    ~ has_manual = true
    ~ CPS_Village_Scavenger_Traded = true
    ~ CPS_Found_DecryptionManual++
    
    Indietreggi lentamente ed esci dal granaio.
    -> village_hub

* [Lo minacci per avere il manuale.]
    -> barn_threat

+ [Te ne vai.]
    {barn_entry == 1:
        "Non ho niente per te". <nl><>
        Lui sputa a terra. "Allora fuori dai piedi. Il tempo è denaro". <nl><>
        Esci a mani vuote, indietreggiando lentamente verso l'uscita.
    - else:
        Esci dal granaio.
    }
    -> village_hub

=== barn_threat ===
Fai un passo avanti, l'aria minacciosa. <nl><>
"Dammi il manuale. Adesso".

L'uomo stringe la presa sulla sua pistola, i muscoli del collo tesi. Cerca di capire se stai bluffando o se sei pronto a uccidere.

// Asset-Based Threat Logic
~ threat_power = 0
{ HasCompanion("Lira"): 
    ~ threat_power = threat_power + 1 
}
{ HasItem("Pistol"):
    ~ threat_power = threat_power + 1 
}

{ threat_power >= 2:
    Il suo sguardo saetta da un punto all'altro, calcolando le sue possibilità.
    
    Vede la tua Kruger puntata al suo petto e la mano di Lira già ferma sull'impugnatura della sua arma. <nl><>
    "Due pistole contro una" mormora, la fronte imperlata di sudore. "Matematica perdente".
    
    Impallidisce. La sua vecchia pistola arrugginita non reggerebbe il confronto. <nl><>
    "Ehi, ehi... calmi" balbetta, abbassando l'arma lentamente. "Non serve spargere sangue per della carta". <nl><>
    Fa un cenno verso il manuale a terra. "Prendetelo e andatevene".
    
    ~ has_manual = true
    ~ AddItemToInventory("DecryptionManual")
    ~ CPS_Found_DecryptionManual++
    ~ CPS_Village_Scavenger_Threatened = true
    -> village_hub
- else:
    { HasItem("Pistol"):
        Lui non indietreggia. Alza il cane della sua arma con un click che rimbomba nel silenzio. <nl><>
        "Bella la tua Kruger. Ma anche la mia spara". Ti fissa negli occhi, un sorriso tirato sulle labbra. <nl><>
        "Se premi il grilletto, moriamo in due. Vuoi davvero giocarti la vita per un libro?"
    }

    { not HasItem("Pistol") and HasCompanion("Lira"):
        Sposta impercettibilmente la mira dalla tua compagna al centro del tuo petto. <nl><>
        "La tua amica ha lo sguardo cattivo" ammette, "ma tu sei a mani nude". Il suo dito si tende sul grilletto. <nl><>
        "Prima che lei riesca a stendermi, ti faccio un buco nello stomaco. Ne vale la pena?"
    }

    { not HasItem("Pistol") and not HasCompanion("Lira"):
        Lui scoppia in una risata che assomiglia più ad un rantolo sgradevole. <nl><>
        "Sei solo, ferito e disarmato. Credi che le belle parole fermino i proiettili?"
    }
    
    Ti fa un cenno brusco con la canna dell'arma verso l'uscita. <nl><>
    "Ora smamma. Prima che decida di sparare gratis." <nl><>
    La minaccia è reale. Devi ritirarti.
    
    ~ failed_negotiation = true
    ~ CPS_Village_Scavenger_Failed = true
    -> village_hub
}

=== command_house_entry_first ===
Sali la collina. La casa di comando è stata sventrata, ma la struttura regge. L'odore di carbone qui è più forte, secco e pungente.
{ HasCompanion("Lira"):
    Lira osserva l'antenna piegata sul tetto. "Lunga portata" commenta. "Se funziona ancora, è il nostro orecchio sul mondo".
}

L'interno è devastato. Documenti bruciati ovunque, scrivanie rovesciate. <nl><>
Qualcuno ha già saccheggiato il posto, portando via tutto ciò che aveva valore... tranne ciò che è troppo pesante.

Al centro della sala operativa, imbullonata a un tavolo di metallo, c'è una radio militare da campo. È accesa: la spia di alimentazione lampeggia debolmente, e dagli altoparlanti esce un fruscio statico costante.

Ti avvicini alla radio. Sembra funzionante, ma le manopole di sintonizzazione sono bloccate su un canale morto. Inoltre, non hai idea di quale sia la frequenza tattica attuale. <nl><>
Senza queste informazioni, questa macchina è inutile.

In un angolo, sotto una scrivania rovesciata, noti il cadavere di un ufficiale. La fondina è aperta, ma la pistola è scivolata sotto il corpo. <nl><>
La recuperi. È una Kruger P-4 di servizio, con un caricatore mezzo pieno.

~ AddItemToInventory("Pistol")
~ AddItemToInventory("Ammo")

* [Provi a usare la radio.]
    -> radio_interaction
* [Torni alla piazza.]
    -> village_hub

=== command_house_entry_again ===
Entri nuovamente nella sala operativa della casa di comando. Il ronzio statico della radio è l'unico suono che riempie il vuoto lasciato dalla distruzione.

+ [Ti siedi alla radio.]
    -> radio_interaction
+ [Non sei ancora pronto. Torni alla piazza.]
    -> village_hub


=== radio_interaction ===
{not came_from(->radio_blind_attempt):
    Ti siedi davanti all'apparecchio. Le manopole sono fredde al tatto.
    
    { has_manual and not has_frequency:
    Hai trovato il manuale, ma senza frequenza non sai come eseguire la sintonizzazione.
    }
}

* {has_frequency && has_manual} [Hai tutto il necessario. Procedi alla calibrazione.]
    Dispieghi la nota trovata nel rifugio e apri il manuale.
    
    Imposti la frequenza: 44.0 MHz. <nl><>
    Lo statico cambia tono, diventa un fischio ritmico.
    
    Usi le tabelle del manuale per inserire la chiave di sblocco. <nl><>
    Giri le manopole con precisione. Il fischio sparisce.
    
    Una voce emerge dal rumore, parlando una lingua dura, gutturale, scandita da pause militari.
    
    ~ CPS_Radio_Solved_Systematic = true
    -> radio_success

* {has_frequency && not has_manual} [Hai la frequenza, ma manca il manuale. Provi a orecchio.]
    Imposti i 44.0 MHz. Il segnale c'è, ma è sporco. Un muro di statico e fischi distorti. <nl><>
    Senza i codici del manuale, non sai come pulirlo. 
    
    { HasCompanion("Lira"):
        Lira incrocia le braccia. "Senza le tabelle di filtro, è un suicidio tecnico. Non posso aiutarti, non conosco questo modello specifico".
    }
    { HasCompanion("Elias"):
        Elias ti guarda impotente. 
    }
    
    Sei solo contro la macchina. Se sbagli la sequenza di sintonizzazione, rischi di sovraccaricare le valvole e bruciare tutto. <nl><>
    La probabilità di indovinare è bassissima. Ma è l'unica chance che hai.
    
    -> radio_no_manual_attempt

* {not has_frequency and radio_attempts <= 0} [Giri le manopole cercando un segnale.]
    ~ CPS_Radio_Tried_Blind = true
    -> radio_blind_attempt
    
+ {not has_frequency and radio_attempts > 0} [Riprovi a sintonizzare la radio.]
    -> radio_blind_attempt

+ [Lasci perdere per ora.]
    Lasci la casa di comando.
    -> village_hub

=== radio_blind_attempt ===
~ radio_attempts++
    
{ radio_attempts:
- 1:
    Giri la manopola della sintonizzazione lentamente. Il fruscio sale e scende come una marea elettrica. <nl><>
    Per un attimo ti sembra di sentire una voce, ma svanisce subito nel rumore bianco. Senza una frequenza precisa, è come cercare un ago nel buio.
    -> radio_interaction
- 2:
    Riprovi a sintonizzare la radio, forzando il guadagno per captare segnali più deboli. Le valvole iniziano a brillare di una luce arancione troppo intensa. Un ronzio grave fa vibrare il tavolo di metallo. <nl><>
    Senti odore di polvere scaldata e ozono. La macchina sta soffrendo.
    
    ~ CPS_Info_Points_Gathered++
    -> radio_interaction
- 3:
    Non ti arrendi. Ignori il calore e giri la manopola con frustrazione. <nl><>
    Il ronzio diventa un fischio lacerante, poi... uno scoppio metallico.
    
    Una scintilla blu scocca dal retro della radio, seguita da uno sbuffo di fumo nero. L'odore di circuiti bruciati riempie la stanza. <nl><>
    Hai spinto troppo oltre una macchina già morente. <nl><>
    Non c'è più nulla da fare qui. Esci dalla casa di comando.
    
    ~ radio_broken = true
    ~ CPS_Radio_Broken = true
    ~ CPS_Info_Points_Gathered--
    -> village_hub
}

=== radio_no_manual_attempt ===
Fai un respiro profondo. Chiudi gli occhi e ti concentri solo sull'udito.

Attraverso le cuffie, il segnale è un caos: senti un ronzio basso e profondo, costante, come un motore elettrico, sovrapposto a un fischio acuto e penetrante. <nl><>
La voce è sepolta lì sotto.

Hai davanti a te tre selettori principali contrassegnati da simboli grafici astratti. Devi andare a intuito per pulire il segnale.

* [Giri il selettore verso il simbolo di una linea spessa.]
    Speri che indichi la stabilità del segnale. <nl><>
    Il fischio acuto sparisce, ma il ronzio basso diventa un boato che copre tutto. Le voci diventano un mugugno incomprensibile, come se parlassero sott'acqua. <nl><>
    Hai isolato solo il rumore di fondo.
    -> radio_failure_blind

* [Giri il selettore verso il simbolo di una linea sottile.]
    Punti a sintonizzarti sulle frequenze più alte. <nl><>
    Il ronzio basso svanisce istantaneamente. Rimane il fischio acuto e... una voce metallica, gracchiante, che assomiglia al verso di una papera. <nl><>
    Hai isolato la voce, anche se è ancora distorta.
    -> radio_no_manual_step_2

* [Posizioni il selettore sull'indicatore barrato al centro.]
    Provi a tagliare la frequenza mediana. <nl><>
    Il segnale collassa su se stesso. Crei un buco di silenzio proprio dove doveva esserci la voce. <nl><>
    Il ricevitore fischia per il feedback.
    -> radio_failure_blind

=== radio_no_manual_step_2 ===
Hai isolato la trasmissione, ma la voce è ancora incomprensibile. È veloce, acuta e distorta. Sembra una trasmissione criptata o modulata in modo strano. <nl><>
Hai una sola possibilità di renderla comprensibile prima di perdere l'aggancio.

* [Regoli la piccola manopola laterale del tono.]
    Ti ricordi dell'addestramento base: se la voce è acuta, prova a variare il tono. <nl><>
    Giri la manopola lentamente. Il timbro della voce scende. Diventa umana. Le parole prendono forma e chiarezza. <nl><>
    Hai agganciato il segnale.
    
    ~ CPS_Radio_Solved_NoManual = true
    -> radio_success

* [Premi l'interruttore con le due onde incrociate.]
    Pensi che serva a invertire il segnale per pulirlo. <nl><>
    Il suono cambia, diventa ancora più aspro e metallico. Un rumore alieno che ti perfora i timpani. <nl><>
    Hai perso l'aggancio.
    -> radio_failure_blind

* [Aumenti la manopola contrassegnata con il "+" al massimo.]
    Provi semplicemente ad amplificare il segnale distorto sperando di capire qualcosa. <nl><>
    Il volume esplode nelle cuffie. L'ago del segnale sbatte contro il fondo scala. <nl><>
    Un suono breve e secco proviene dal retro della radio.
    -> radio_failure_blind

=== radio_failure_blind ===
Un fumo acre, grigiastro, inizia a uscire dalle feritoie della radio. Puzza di bachelite e rame fuso. La luce ambra sfarfalla, poi muore con un ultimo gemito elettrico. Hai bruciato le valvole finali. <nl><>
Ti togli le cuffie nel silenzio improvviso. Non c'è modo di ripararla. L'informazione è persa per sempre.

~ radio_broken = true
~ IncreaseGlobalStatCapped("Fatigue", 5, FATIGUE_CAP)
~ CPS_Radio_Broken = true
-> village_hub

=== radio_success ===
~ radio_fixed = true
~ LISTENED_TO_RADIO = true

La trasmissione è un bollettino ciclico in lingua nemica. Non capisci le frasi, ma riconosci la cadenza dei codici operativi.

// --- ELIAS SCENARIO: SKETCHING (VISUAL SHORTCUT) ---
{ HasCompanion("Elias"):
    Elias non aspetta. Riconosce quella lingua, quei codici. Sono i comandi a cui obbediva. Ti mette una mano sulla spalla per fermarti, poi afferra un mozzicone di matita grassa da una scrivania rovesciata.
    
    Sente "Sektor Kirt... Status Alfa... Kirt... Status Gamma..." <nl><>
    Scrive direttamente sulla superficie metallica del tavolo radio: disegna un orologio che segna le quattro. Accanto, abbozza rapidamente due omini che si danno le spalle e si allontanano: cambio guardia.
    
    Poi la radio gracchia: "Sektor Senner... K-Verten Frunzkan... Hazr..."
    
    {KNOWN_MOUNTAINPASS:
        Elias si sposta su una porzione pulita del metallo. Disegna due montagne e una linea che le unisce. Poi traccia una croce sopra la linea con un gesto secco. Ponte rotto. Infine, disegna con cura un simbolo che ti gela il sangue: pericolo chimico. Si porta la mano alla gola, mimando il soffocamento.
        
        Il messaggio è chiaro: è una fotografia della situazione al valico montano.
        
        ~ KNOWN_MOUNTAINPASS_STATUS = true
    - else:
        Elias si sposta su una porzione pulita del metallo. Disegna un ponte stilizzato e traccia una croce sopra la linea con un gesto secco. Dopo di che, disegna tre cerchi intersecati su sfondo giallo: il simbolo di pericolo chimico. <nl><>
        Poi, si ferma a fissare i disegni, pensieroso.
        
        Dopo qualche istante bofonchia qualcosa nella sua lingua, probabilmente un'imprecazione, e scuote il capo. Non riesce a capire a cosa si riferisca questa parte della comunicazione.
    }
    

// --- LIRA SCENARIO: TRANSLATION ---
- else: 
    { HasCompanion("Lira"):
        {has_manual:
            Lira ti guarda con rispetto, poi si china sul tavolo. Il suo orecchio è teso verso le cuffie che hai appoggiato sul tavolo per farle sentire.
            
            "Qui... 'Infrastrutture Critiche' e 'Stato Operativo'. E questo codice è per gli orari". <nl><>
            La radio gracchia: "Sektor Kirt... Status Alfa... Kirt... Status Gamma..." <nl><>
            Lira traduce al volo. "Status Gamma alle 4:00. Cambio turno sulla strada. È la nostra finestra".
            
            La voce continua: "Sektor Senner... K-Verten Frunzkan... Hazr..." <nl><>
            "K-9 significa "Ponte". "Negativo"... crollato". <nl><>
            Poi ascolta 'Hazr'. Impallidisce. "Residui chimici attivi".
            
            {KNOWN_MOUNTAINPASS:
                "Si tratta sicuramente del valico montano. Il ponte d'accesso dev'essere crollato e i tunnel saranno saturi di gas, o almeno questa è la mia scommessa" conclude Lira.
                ~ KNOWN_MOUNTAINPASS_STATUS = true
            - else:
                "Non ho idea di cosa stiano parlando..." conclude Lira dopo un attimo di riflessione, con tono sconfitto.
            }
        }
// --- SOLO SCENARIO: DEDUCTION ---
    - else:
        {has_manual:
            Apri il manuale, scorrendone le pagine febbrilmente mentre la voce continua.
            
            "Sektor Kirt... Status Alfa... Kirt... Status Gamma..." <nl><>
            Confronti i suoni con la tabella fonetica. "Status Gamma" corrisponde a un'icona circolare che rappresenta due freccie che si inseguono. <nl><>
            Non ci metti molto a capire che il simbolo si riferisce a un cambio della guardia o a uno spostamento di merci alle ore 4:00.
            
            Non sei estraneo a questa tipologia di comunicazioni: anche il tuo esercito le usa per informare i soldati della scansione oraria delle operazioni, specialmente in zone ad alto traffico o importanti snodi logistici. Probabilmente, il messaggio che hai appena ascoltato elenca gli status orari della strada militare, unico snodo nemico in quest'area sufficientemente rilevante per un bollettino di questo tipo.
            
            Poi la voce cambia registro: "Sektor Senner... K-Verten Frunzkan... Hazr..." <nl><>
            Cerchi i termini.
            
            "Sektor Kirt": Settore 7. <nl><>
            "K-Verten": il simbolo è inequivocabile, un ponte stilizzato. "Frunzkan": Stato Critico / Crollo. <nl><>
            "Hazr": il dito si ferma su un'icona che conosci fin troppo bene. Tre cerchi intersecati su sfondo giallo. Pericolo Chimico.
            
            {KNOWN_MOUNTAINPASS:
                Tiri fuori la mappa recuperata dal disertore. Cerchi i riferimenti dei settori. Il Settore 7 non è sulla strada principale. Segui le curve di livello... eccolo. <nl><>
                È il vecchio valico montano. Le miniere.
                
                Colleghi immediatamente i pezzi: il ponte crollato è l'accesso principale al valico. E probabilmente il gas satura i vecchi tunnel minerari. <nl><>
                Ora hai un quadro completo di ciò che ti aspetta lassù.
                ~ KNOWN_MOUNTAINPASS_STATUS = true
            - else:
                Chiudi il manuale con frustrazione. Sai che da qualche parte c'è un ponte crollato. Sai che c'è del gas. <nl><>
                Hai ottenuto un'informazione senza il contesto in cui usarla.
            }
        - else:
            Ascolti il pattern che si ripete. Una parola torna spesso: "Sektor". "Sektor Kirt... Status Alfa... Kirt... Status Gamma... Kirt... Status Alfa..." <nl><>
            Deduci che "Status Gamma" indichi una evento anomalo all'interno di una sequenza di "Status Alpha".
            
            Non sei estraneo a questa tipologia di comunicazioni: anche il tuo esercito le usa per informare i soldati della scansione oraria delle operazioni, specialmente in zone ad alto traffico o importanti snodi logistici. Probabilmente, il messaggio che hai appena ascoltato elenca gli status orari della strada militare, unico snodo nemico in quest'area sufficientemente rilevante per un bollettino di questo tipo.
            
            La radio continua senza sosta: "Sektor Senner... K-Verten Frunzkan... Hazr..." <nl><>
            Non conosci il significato di "K-Verten Frunzkan", ma "Hazr" è una parola che non dimenticherai mai. <nl><>
            L'hai già sentita urlare dal nemico prima di indossare le maschere antigas: da qualche parte ci sono residui chimici attivi, ma il dove rimane un mistero.
            
            Spegni la radio: non ci sono altre informazioni da raccogliere.
        }
    }
}

~ KNOWN_MILITARY_ROAD_STATUS = true

Spegni l'audio. Il silenzio torna a riempire la stanza.

{ HasCompanion("Lira") && HasCompanion("Elias"):
    Lira guarda Elias, che fissa ancora i cerchi disegnati sul metallo freddo. <nl><>
    "Il ponte è andato. Se cadiamo di sotto, la guerra finisce in fretta". <nl><>
    Elias annuisce lentamente, cancellando il disegno con il pollice.
- else:
    { HasCompanion("Lira"):
        "Ottimo lavoro con quella radio" commenta Lira. "Sapere del gas ci evita di morire soffocati come topi. Ora dobbiamo scegliere le nostre prossime mosse"
    }
    { HasCompanion("Elias"):
        Elias espira lentamente, lasciando cadere la matita. Ti guarda con gratitudine: sapere prima di agire è un lusso.
    }
    { not HasCompanion("Lira") && not HasCompanion("Elias"):
        Resti immobile per un istante. <nl><>
        Sei solo, ma almeno ora non sei cieco. Hai costruito una mappa mentale dei pericoli decifrando i suoni del nemico. È l'unica compagnia che puoi permetterti.
    }
}

Ti allontani dalla radio ed esci dalla casa di comando.
-> village_hub

=== final_planning ===
~ IncreaseGlobalStatCapped("Fatigue", 5, FATIGUE_CAP)

{ HasCompanion("Lira") or  HasCompanion("Elias"):
    Vi ritrovate <>
- else:
    Ti ritrovi <>
}
al centro della piazza, sotto un cielo che si sta chiudendo in una morsa di piombo. È il momento di decidere. Non ci sono più angoli da esplorare, né scuse per restare.

Dalla valle a sud sale il rumore lontano dei motori. La strada militare. È larga, battuta, logisticamente perfetta per spostare truppe e risorse. È la via della civiltà, ma anche quella del nemico.
{ KNOWN_MILITARY_ROAD_STATUS:
    Grazie alla radio, sai che quella strada ha un punto debole. "Status Gamma". Alle quattro del mattino il nemico abbassa la guardia per il cambio turno. Dieci minuti di cecità nel loro sistema di sorveglianza. Se arrivate puntuali, la strada più pericolosa potrebbe diventare un corridoio sicuro. Ma il tempismo deve essere perfetto.
- else:
    Ma è anche la gola del lupo. Pattuglie, posti di blocco, convogli. Senza sapere quando passano, ogni passo sull'asfalto è una scommessa con la morte. Eppure, è l'unica strada che garantisce di non perdersi e di non morire di freddo.
}

{ KNOWN_MOUNTAINPASS:
    A est, le montagne si alzano come un muro di roccia nera. Stringi la mappa del disertore tra le mani. Indica un passaggio attraverso le vecchie miniere. <nl><>
    
    { KNOWN_MOUNTAINPASS_STATUS:
        Le notizie della radio, però, gettano un'ombra su quella via. Probabilmente il ponte principale è crollato e i tunnel sono saturi di gas. La mappa ti dice dove andare, ma la radio ti ha rivelato cosa ti ucciderà se non sei preparato. È una via deserta, ma ostile. La natura lì è un nemico tanto quanto i soldati a valle.
    - else:
        Sulla carta, sembra la via di fuga perfetta. Lontano dalla strada, lontano dai posti di blocco, nascosta tra le vecchie miniere. Un percorso fantasma che il nemico sembra ignorare. L'assenza di pattuglie la rende allettante, ma non sai se sia ancora agibile o cosa si nasconda nel buio delle miniere. È un salto nel vuoto: pace apparente contro pericoli ignoti.
    }
}

{ HasCompanion("Lira") && HasCompanion("Elias"):
    Lira osserva la valle con occhio critico. <nl><>
    { KNOWN_MILITARY_ROAD_STATUS:
        "Quattro del mattino" mormora, controllando il suo orologio. "Abbiamo una finestra stretta. Se abbiamo ragione sui codici, la strada sarà deserta per dieci minuti. È un rischio, ma forse lo possiamo gestire". <nl><>
        Elias scuote la testa, indicando le montagne. Per lui il rischio sulla strada è eccessivo.
    - else:
        "Senti quel rombo?" chiede Lira. "Convogli pesanti. Senza sapere gli orari di passaggio, scendere sulla strada è come firmare la propria condanna a morte. Preferisco l'ignoto".
    }
    
    { KNOWN_MOUNTAINPASS:
        Lira si volta verso le cime. <nl><>
        { KNOWN_MOUNTAINPASS_STATUS:
            "Gas e crolli" continua, più titubante. "Lassù sarà un inferno logistico. Ma almeno il nemico è la gravità, non un fucile puntato. Se abbiamo l'equipaggiamento, è meglio".
        - else:
            "Le miniere sono un'incognita, ma se la mappa è vera, potremmo sparire dai loro radar completamente. Non mi fido di una strada non marcata, ma il rischio tattico è migliore di uno scontro frontale".
        }

        Elias tocca la mappa con un dito calloso. <nl><>
        { KNOWN_MOUNTAINPASS_STATUS:
            Alza lo sguardo verso di te, il terrore del gas visibile nei suoi occhi. Mimando il soffocamento, indica la valle, quasi preferisse i fucili. Evidentemente, il pericolo del gas lo spaventa. La strada, per quanto pericolosa, è aria pulita. <nl><>
        - else:
            Annuisce verso le montagne con una speranza silenziosa. Per lui, la roccia è meglio della divisa che ha rinnegato. La strada militare è solo morte certa. <nl><>
        }
        
        Si guardano, un soldato e una spia, divisi sulla via da prendere. La decisione spetta a te.
    }   
}

{ HasCompanion("Lira") && not HasCompanion("Elias"):
    { KNOWN_MILITARY_ROAD_STATUS:
        Lira: "La nostra unica finestra è il cambio della guardia alle quattro. Dobbiamo essere chirurgici sulla strada. Se sbagliamo i tempi, siamo morti. Ma se li azzecchiamo, raggiungeremo il confine per l'alba". 
    - else:
        Lira: "La strada è un rischio enorme. Senza sapere gli orari delle pattuglie, ogni metro è un'imboscata potenziale. Non mi piace affidarmi alla fortuna".
    }
    
    { KNOWN_MOUNTAINPASS:
        Fa una pausa, stringendo le cinghie dello zaino e valutando l'alternativa. <nl><>
        { KNOWN_MOUNTAINPASS_STATUS:
            "Quella mappa è un invito al martirio se non abbiamo l'equipaggiamento giusto. Gas, ponti crollati... Ma la strada è sorvegliata. È una scelta tra veleno certo o piombo probabile".
        - else:
            "La mappa ci dà un vantaggio. Se passiamo dalle miniere, evitiamo l'esercito. Non sappiamo cosa nascondono, ma l'assenza di pattuglie nemiche è un dato rassicurante. Forse vale la pena rischiare l'ignoto".
        }
    }
}

{ not HasCompanion("Lira") && HasCompanion("Elias"):
    Elias ascolta il rombo dalla valle. <nl><>
    { KNOWN_MILITARY_ROAD_STATUS:
        Indichi il tuo orologio e alzi quattro dita. Lui ti guarda, calcola mentalmente, poi fa un cenno di assenso lento. Capisce che c'è un buco nella rete. Indica la strada, poi mima un movimento rapido. È pericoloso, ma veloce.
    - else:
        Scuote la testa con veemenza, indicando i veicoli invisibili ma udibili laggiù. "Nerk" ringhia. "Tod". Senza un piano, la strada è un muro invalicabile per lui.
    }

    { KNOWN_MOUNTAINPASS:
        Elias sposta lo sguardo sulle montagne. <nl><>
        { KNOWN_MOUNTAINPASS_STATUS:
            Guarda le cime con terrore puro, toccandosi la gola. Sa cosa fa il gas. Poi guarda la valle piena di camion. È una scelta tra due incubi, e l'idea di soffocare nel buio lo paralizza quasi quanto l'idea di essere fucilato.
        - else:
            Guarda la valle con timore, riconoscendo il rumore dei camion. Poi guarda te e indica le montagne, aspettando il tuo parere con fiducia. Lì non ci sono motori, non ci sono ufficiali. Per lui, quello basta.
        }
    }
}
{ not HasCompanion("Lira") && not HasCompanion("Elias"):
    Sei solo. Nessuno coprirà le tue spalle sulla strada, nessuno ti tirerà su se scivoli nel burrone. <nl><>
    Controlli i lacci degli scarponi e la Kruger nella fondina. <nl><>
    Una via richiede velocità e nervi saldi, l'altra resistenza e adattamento.
}

Un ultimo respiro gelido. Hai tutte le informazioni che potevi raccogliere. <nl><>
Ora conta solo il prossimo passo.

~ COMPLETED_VILLAGE = true
~ CAN_SET_CAMP = true
-> END