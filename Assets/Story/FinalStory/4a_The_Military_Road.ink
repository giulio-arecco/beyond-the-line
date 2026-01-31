INCLUDE Globals.ink

// ============================================================
// SECTION IV-A: THE MILITARY ROAD
// ============================================================

// --- Local Logic Variables ---
VAR alert_level = 0
VAR truck_alerted = false

-> road_entry

=== road_entry ===
~ PlayMusic("TheMilitaryRoad", 2)

La valle si apre sotto di voi come una ferita scura nella terra.
L'aria vibra. Non è il vento, ma il rombo costante, sordo, di decine di motori diesel.
La strada militare. L'arteria principale che alimenta il fronte.
Camion pesanti, blindati leggeri e trasporti truppe scorrono su un nastro di asfalto bagnato, illuminati da coni di luce giallastra che tagliano la pioggia.

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Vi appiattite contro il terreno fangoso del costone, osservando il movimento.
- else:
    Ti appiattisci contro il terreno fangoso del costone, osservando il movimento.
}
Passare lì in mezzo sembra un suicidio.

{ KNOWN_MILITARY_ROAD_STATUS:
    Guardi l'orologio. Sono le 03:45.
    { HasCompanion("Lira"):
        Lira controlla il quadrante del suo cronografo, poi la valle. "Quindici minuti allo Status Gamma," sussurra. "Se la radio ha detto il vero, il flusso si interromperà per il cambio turno logistico."
    }
    { HasCompanion("Elias"):
        Elias osserva il traffico incessante con ansia, ma quando gli indichi l'ora e mimi il numero quattro, annuisce. Si fida dell'informazione, anche se il suo corpo è teso come una corda di violino.
    }
    
    { not HasCompanion("Lira") and not HasCompanion("Elias"):
        Sei solo, ma l'informazione che hai decifrato è la tua unica compagnia. Alle quattro il flusso dovrebbe fermarsi.
    }
    
    { HasCompanion("Lira") or HasCompanion("Elias"):
        Decidete di aspettare. Il freddo vi entra nelle ossa, ma rimanete immobili.
    - else:
        Decidi di aspettare. Il freddo ti entra nelle ossa, ma rimani immobile.
    }
    
    Alle 03:58, accade l'impossibile.
    Il rombo diminuisce. I fari si diradano.
    Alle 04:00 in punto, la strada è deserta. Solo i lampioni di guardia ai checkpoint rimangono accesi, ma il fiume di metallo si è fermato.
    Avete una finestra di opportunità. Dieci minuti, forse meno, prima che il turno successivo riprenda il ritmo.
    
    ~ IncreaseGlobalStat("Fatigue", 5) 
- else:
    Non sai nulla dei loro orari. Vedi solo un flusso continuo di mezzi militari.
    Aspettare al freddo senza un piano sembra inutile e rischioso. Bisogna muoversi ora, sperando di trovare un intervallo tra un convoglio e l'altro.
    { HasCompanion("Lira") or HasCompanion("Elias"):
        Scendete verso il perimetro esterno, sfruttando le ombre. Il rumore dei motori copre i vostri passi, ma aumenta la tensione a ogni metro.
    - else:
        Scendi verso il perimetro esterno, sfruttando le ombre. Il rumore dei motori copre i tuoi passi, ma aumenta la tensione a ogni metro.
    }
    
    ~ alert_level = 10 
}

Vi trovate davanti a una scelta tattica. Il perimetro è recintato, ma ci sono due punti deboli visibili.

A sinistra, la recinzione costeggia una serie di magazzini prefabbricati e container accatastati. Ci sono ombre profonde, casse per coprirsi, ma è zona di pattugliamento.
A destra, più in basso, c'è l'imbocco di un canale di scolo che passa sotto la strada. L'acqua è nera, oleosa, e puzza di prodotti chimici.

{ HasCompanion("Lira"):
    "I magazzini offrono copertura tattica," valuta Lira, occhi stretti che calcolano le linee di tiro. "Rischioso, ma veloce. E potremmo trovare equipaggiamento utile."
    Guarda il canale con disgusto. "Quella fogna è sicura dagli occhi, ma l'aria lì dentro è veleno. Uscirne vivi sarà una scommessa."
}

{ HasCompanion("Elias"):
    Elias indica i container, riconoscendo la struttura logistica. "Scklat," mormora. Poi indica il canale e fa una smorfia, tappandosi il naso, scuotendo la testa come a dire che non c'è onore nel morire nel fango.
}

{ not HasCompanion("Lira") and not HasCompanion("Elias"):
    Devi scegliere: rischiare di essere visto tra i magazzini o rischiare di soffocare nel canale.
}

* [Scegliete la via dei magazzini.]
    { HasCompanion("Lira") or HasCompanion("Elias"):
        Vi muovete verso i container, cercando di fondervi con le ombre.
    - else:
        Ti muovi verso i container, cercando di fonderti con le ombre.
    }
    -> warehouses_start
* [Scegliete la via del canale.]
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Scivolate verso l'acqua scura, trattenendo il respiro.
    - else:
        Scivoli verso l'acqua scura, trattenendo il respiro.
    }
    -> sewer_start

=== warehouses_start ===
{ HasCompanion("Lira") || HasCompanion("Elias"):
    Superate la recinzione attraverso un taglio nella rete.
    Siete nel settore logistico. Pile di container arrugginiti formano un labirinto di vicoli stretti.
- else:
    Superi la recinzione attraverso un taglio nella rete.
    Sei nel settore logistico. Pile di container arrugginiti formano un labirinto di vicoli stretti.
}

{ KNOWN_MILITARY_ROAD_STATUS:
    Il silenzio innaturale è rotto solo dal ronzio dei lampioni e dal passo ritmico degli scarponi delle sentinelle. Ogni suono è amplificato.
- else:
    Il rumore dei camion copre i passi, ma le luci dei fari che spazzano i vicoli costringono a una danza continua per restare nell'ombra.
}

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Vi muovete da un riparo all'altro.
- else:
    Ti muovi da un riparo all'altro.
}
~ IncreaseGlobalStat("Fatigue", 5)

Noti un container diverso dagli altri. Ha il portellone socchiuso, bloccato da una catena.
Sull'esterno è stampigliato un simbolo bianco.

{ HasCompanion("Elias"):
    Elias si blocca appena lo vede. Ti afferra il braccio, indicando il simbolo con urgenza. Mima il gesto di scrivere e poi fa un saluto rigido, indicando le mostrine immaginarie sulle spalle.
    Capisci: logistica ufficiali. Lì dentro potrebbero esserci documenti importanti.
- else:
    Il simbolo è una forma geometrica astratta. Non ne conosci il significato, ma la catena esterna suggerisce che contenga qualcosa che non deve essere preso dalla fanteria semplice.
}

Potrebbe contenere risorse utili, ma la catena fa rumore se mossa. E c'è una garitta non lontano, con la luce accesa.

* {HasItem("Crowbar")} [Usi il piede di porco per forzare silenziosamente la catena.]
    Incastri il piede di porco tra le maglie. Fai leva progressiva, attento a non far stridere il metallo.
    Uno scatto secco. La catena cede.
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Entrate, richiudendo il portellone alle vostre spalle.
    - else:
        Entri, richiudendo il portellone alle tue spalle.
    }
    
    L'interno è un ufficio da campo mobile, immerso nella penombra. C'è odore di carta vecchia e tabacco stantio. 
    Il fascio della tua torcia rivela una scrivania di metallo ingombra di carteggi e pareti coperte di mappe tattiche.
    
    { HasItem("Notebook"):
        Su uno scaffale, noti un grosso volume rilegato con il dorso rinforzato.
        Lo apri rapidamente: colonne di parole in due lingue. È un dizionario militare bilingue.
        ~ AddItemToInventory("Dictionary")
    }
    
    Accanto, una cassa di munizioni aperta contiene fumogeni tattici. Ne prendi uno.
    ~ AddItemToInventory("SmokeGrenade")
    
    { HasCompanion("Lira"):
        "Se le cose si mettono male, quella ci potrebbe salvare la vita" commenta Lira.
    }
    
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Uscite senza aver attirato l'attenzione, scivolando via come fantasmi.
    - else:
        Esci senza aver attirato l'attenzione, scivolando via come un fantasma.
    }
    
    ~ IncreaseGlobalStat("Fatigue", 5)
    ~ alert_level = alert_level + 10 
    -> truck_encounter_active

* [Provi ad aprirlo a mani nude, rischiando il rumore.]
    Afferri la catena con entrambe le mani. È fredda, pesante. Tiri.
    Il metallo sbatte contro la parete vuota del container. Un suono metallico, netto, simile a una campana nel silenzio notturno.
    
    Ti congeli.
    Una voce abbaia qualcosa da lontano. Un fascio di luce spazza l'area, fermandosi a pochi metri da te.
    "Vruk nar?" urla una guardia.
    
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Vi buttate dentro il container giusto in tempo, schiacciandovi contro la parete metallica.
    - else:
        Ti butti dentro il container giusto in tempo, schiacciandoti contro la parete metallica.
    }
    
    Il cuore ti martella contro le costole.
    Attraverso la fessura del portellone, vedi gli stivali della guardia fermarsi davanti all'ingresso.
    La luce della torcia taglia l'interno, passando a pochi centimetri dal tuo viso.
    
    Per un istante vedi l'interno del container illuminato a scatti: casse accatastate, una scrivania piena di fogli sparsi come se qualcuno fosse fuggito, e mappe con linee rosse tracciate ovunque.
    
    La guardia spinge il portellone, ma la catena incastrata fa resistenza.
    Senti gracchiare una radio sulla sua spalla. Una voce distorta gracchia ordini incomprensibili.
    "Sektor Vork... Krurt... Negat... Vorken."
    L'uomo sbuffa, dà un calcio alla porta per frustrazione e si allontana, rispondendo alla chiamata. "Za, za. Vorken."
    
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Aspettate che i passi si allontanino. Siete madidi di sudore freddo.
    - else:
        Aspetti che i passi si allontanino. Sei madido di sudore freddo.
    }
    
    Ora puoi muoverti.
    
    { HasItem("Notebook"):
        Recuperi rapidamente il volume che avevi intravisto sulla scrivania: un dizionario bilingue.
        ~ AddItemToInventory("Dictionary")
    }
    
    Afferri anche una granata fumogena da una cassa aperta.
    ~ AddItemToInventory("SmokeGrenade")
    
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Siete stati quasi scoperti.
    - else:
        Sei stato quasi scoperto.
    }
    La guardia non ha dato l'allarme, ma ora sanno che c'è qualcosa che non va.
    
    ~ alert_level = alert_level + 35
    ~ IncreaseGlobalStat("Fatigue", 10)
    -> truck_encounter_active

* [Ignori il container. Troppo rischioso.]
    Passi oltre. La curiosità non vale una pallottola.
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Mantenete un basso profilo.
        { HasCompanion("Lira"):
            Lira ti lancia un'occhiata veloce, disapprovando l'occasione persa, ma continua a coprirti.
        }
    - else:
        Mantenete un basso profilo.
    }
    -> truck_encounter_active

=== sewer_start ===
{ HasCompanion("Lira") || HasCompanion("Elias"):
    Entrate nell'acqua gelida fino alle ginocchia. Il fondo è melmoso, instabile. Ogni passo richiede uno sforzo cosciente per estrarre lo scarpone dal fango che sembra volerlo inghiottire.
- else:
    Entri nell'acqua gelida fino alle ginocchia. Il fondo è melmoso, instabile. Ogni passo richiede uno sforzo cosciente per estrarre lo scarpone dal fango che sembra volerlo inghiottire.
}
L'odore è atroce. Ammoniaca e marciume che riempiono la gola.

{ HasCompanion("Lira"):
    Lira avanza trattenendo i conati, la pistola alta sopra la testa per non bagnarla. "Se usciamo vivi da qui," sibila, "mi dovete un bagno caldo."
}

~ IncreaseGlobalStat("Fatigue", 5)

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Avanzate nel buio del tunnel di cemento.
- else:
    Avanzi nel buio del tunnel di cemento.
}
Sopra le teste, attraverso le grate, si vedono i fari dei veicoli passare.
Qualcosa blocca il deflusso dell'acqua più avanti.
È un accumulo di detriti... e corpi.
Sono soldati, scarti di qualche epurazione o feriti morti durante il trasporto e gettati via come spazzatura.

Tra i rifiuti, vedi la tracolla di una borsa medica impigliata in un tondino di ferro arrugginito, rimasta sospesa sopra il livello dell'acqua. Potrebbe contenere morfina o bende ancora asciutte.
{ HasItem("Notebook"):
    Noti anche una borsa di cuoio cerato, rigida e impermeabile, del tipo usato dai portaordini, incastrata in alto tra i tubi di scolo, lontana dal liquame.
}

Recuperarli significa arrampicarsi sulla catasta di detriti e cadaveri. Senza calcolare ogni singolo movimento, si rischia di scivolare nella poltiglia tossica o, peggio, di allertare i soldati in strada.

* [Ti arrampichi per recuperare tutto il possibile.]
    Ti fai forza e sali sui detriti. Senti le ossa dei morti cedere sotto i tuoi stivali. È una sensazione oscena.
    Afferri la borsa medica. È intatta.
    
    
    ~ AddItemToInventory("Medikit")
    
    { HasItem("Notebook"):
        Raggiungi la borsa di cuoio in alto. La apri con dita scivolose: l'interno è asciutto.
        Dentro trovi un dizionario tascabile bilingue, perfettamente conservato. Una fortuna sfacciata in mezzo all'orrore.
        ~ AddItemToInventory("Dictionary")
    }
    
    ~ IncreaseGlobalStat("Fatigue", 25)

    // --- CHECK FOR FATIGUE COLLAPSE IN SEWER START ---
    { GetGlobalStat("Fatigue") >= 100:
        L'ultimo sforzo per scendere è fatale. I tuoi muscoli non rispondono più.
        Non è un semplice scivolone. È un crollo sistemico. Le gambe diventano di piombo, la vista si oscura ai bordi.
        Cadi pesantemente sui detriti, trascinando con te metallo e pietre. Un frastuono che nel tunnel rimbomba come un'esplosione.
        
        { HasCompanion("Lira"):
            Senti due mani che ti afferrano la giubba con violenza, impedendoti di scivolare nell'acqua nera.
            "Svegliati! Dannazione, muoviti!" sbraita Lira, il volto a un centimetro dal tuo, gli occhi sbarrati dalla furia e dalla paura. Ti scuote, ma tu sei un peso morto.
        }
        { HasCompanion("Elias"):
            Senti un braccio magro passare sotto la tua spalla. Elias grugnisce per lo sforzo, cercando di tenerti su mentre le sue stesse gambe tremano nel fango. "Vok!" geme, "Vok nar!".
        }
        { not HasCompanion("Lira") and not HasCompanion("Elias"):
            Rimani lì, riverso sui cadaveri, con il sapore della ruggine e della morte in bocca. Cerchi di ordinare al tuo corpo di alzarsi, ma lui si rifiuta.
        }
        
        Boccheggi. Ogni respiro è una lotta per incamerare ossigeno. I secondi passano, dilatati. Uno, cinque, dieci secondi di immobilità totale in cui cercate solo di riprendere il controllo.
        Ed è proprio quel ritardo a condannarvi. Il rumore ha attirato l'attenzione, ma è la tua incapacità di nasconderti subito che vi espone.
        
        Sopra di voi, la luce di una torcia penetra la grata. Non spazza via velocemente: si ferma su di voi, immobili bersagli nel fango.
        "Zant! Vruk est!"
        
        ~ DecreaseGlobalStat("Fatigue", 40)
        ~ alert_level = 100
        ~ truck_alerted = true
        ~ DecreaseGlobalStat("Cohesion", 20)
        
        L'urlo dall'alto spezza la paralisi. Non c'è più tempo per la cautela, non c'è più modo di nascondersi.
        L'unica opzione è forzare l'uscita e correre prima che aprano il fuoco.
        -> escape_sequence_collapse_intro
    }
    
    Scendi giù, ma una trave marcia cede. Scivoli nell'acqua putrida, bevendo una boccata di quel veleno liquido.
    Ti rialzi, cercando di sopprimere i colpi di tosse. Non sembra ti abbiano sentito.
    
    ~ DecreaseGlobalStat("Health", 15)
    -> truck_encounter_passive

* [Prosegui senza fermarti. Non vuoi toccare quei morti.]
    L'istinto di conservazione vince sull'avidità. 
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Vi allontanate il più possibile dai cadaveri e dai veleni, ignorando le risorse.
    - else:
        Ti allontani il più possibile dai cadaveri e dai veleni, ignorando le risorse.
    }
    La fatica del cammino nel fango si fa sentire sempre di più.
    -> truck_encounter_passive

=== truck_encounter_active ===
{ HasCompanion("Lira") || HasCompanion("Elias"):
    Siete quasi alla fine del settore magazzini. L'uscita verso il bosco è vicina, intravedete gli alberi oltre l'ultimo cancello.
- else:
    Sei quasi alla fine del settore magazzini. L'uscita verso il bosco è vicina, intravedi gli alberi oltre l'ultimo cancello.
}

Dapprima è solo una vibrazione sotto i piedi. Poi il rombo si fa distinto, in avvicinamento rapido.
Un camion da trasporto truppe svolta l'angolo a velocità sostenuta, i fari che tagliano il buio.

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Vi schiacciate contro i container appena in tempo.
- else:
    Ti schiacci contro i container appena in tempo
}

Il veicolo frena bruscamente proprio davanti al cancello che devi superare. Stridio di freni. Vapore che esce dal cofano.
Il motore ha un colpo di tosse violento e muore.

L'autista scende dalla cabina urlando: "Grahz! Grahz!"
Altri due soldati saltano giù dal retro, iniziando a litigare con l'autista. Hanno aperto il cofano e il fumo bianco li avvolge.

{ KNOWN_MILITARY_ROAD_STATUS:
    Intanto, sulla strada principale, il traffico sta riprendendo vita. Senti i primi motori pesanti riavviarsi in lontananza. Il cambio turno è finito. La finestra di silenzio si è chiusa. Ora ogni minuto che passa aumenta il rischio esponenzialmente.
- else:
    Il traffico sulla strada è incessante. Il rumore copre le voci dei soldati, ma rende anche impossibile sentire se arrivano rinforzi o altri veicoli.
}

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Siete in trappola.
- else:
    Sei in trappola.
}
Davanti, il nemico blocca l'unica uscita. Alle spalle, il labirinto dei container dove le pattuglie potrebbero arrivare da un momento all'altro.
Il cuore ti batte nelle orecchie come un tamburo. Sei in preda al terrore.
{ HasCompanion("Lira"):
    Lira ha la mascella serrata, la mano pronta sull'arma.
}

Valuti la situazione. La distanza dal cancello è di circa venti metri. I tre soldati sono concentrati sul motore, ma uno si guarda spesso intorno.
{ GetGlobalStat("Fatigue") >= 60:
    Le mani ti tremano visibilmente. Il respiro è corto, irregolare. I muscoli delle gambe bruciano, rigidi come legno vecchio: fatichi anche solo a stare in piedi. Senti che la precisione e il controllo muscolare non sono tuoi amici stanotte.
- else:
    Il battito è accelerato ma controllato. I muscoli sono reattivi. Ti senti pronto a scattare o a muoverti con precisione millimetrica.
}

* {HasItem("SmokeGrenade")} [Lanci il fumogeno]
    Decidi che il rischio di essere scoperti muovendosi di riparo in riparo è troppo alto. Meglio dettare le regole del gioco.
    Sganci la sicura della granata fumogena. La lanci verso il camion.
    La nube grigia esplode, avvolgendo i soldati e il veicolo.
    "Vrast! Ruz!" urla uno dei soldati, tossendo.
    
    Hanno capito che siete lì, ma il muro di fumo vi protegge dalla vista diretta. Nessuno può mirare.
    ~ alert_level = 100
    ~ truck_alerted = true
    ~ RemoveItemFromInventory("SmokeGrenade", 1)
    
    "Ora!" urli.
    Scattate attraverso la nube, invisibili ma udibili, verso il cancello.
    -> escape_sequence

* [Provi a sgattaiolare via, approfittando della loro discussione]
    Decidi di sfruttare il rumore della lite per muoverti. Ti abbassi per passare inosservato.
    È una questione di controllo muscolare assoluto.
    
    { GetGlobalStat("Fatigue") >= 60:
        Provi a muoverti, ma la gamba ti cede per la stanchezza. Urta una cassa metallica vuota.
        Il suono taglia l'aria. La discussione al camion si ferma istantaneamente.
        I tre soldati si girano all'unisono verso la tua posizione.
        "Vruk est!"
        Sei stato visto.
        ~ alert_level = 100
        ~ truck_alerted = true
        ~ IncreaseGlobalStat("Fatigue", 5)
    - else:
        Ti muovi come un'ombra. Ogni movimento è fluido. Passi a pochi metri da loro, coperto dall'oscurità e dalla loro distrazione.
        Raggiungi il cancello senza che nessuno alzi la testa.
        ~ IncreaseGlobalStat("Fatigue", 5)
        // alert_level remains unchanged
    }
    -> escape_sequence

* [Aspetti il momento esatto in cui il motore scoppietta.]
    Rimani immobile. Aspetti che l'autista provi a riaccendere il motore. Sai che farà rumore.
    È snervante. I minuti passano.
    
    { KNOWN_MILITARY_ROAD_STATUS:
        Sai che ogni secondo perso avvicina la ripresa del traffico.
    }
    
    Finalmente, uno scoppio dal tubo di scappamento.
    Ti muovi in quell'esatto secondo. Ti fermi. Altro scoppio. Altro movimento.
    È lento, massacrante per i nervi, ma efficace.
    Superi il blocco senza essere notato, ma lo stress ti ha prosciugato.
    ~ IncreaseGlobalStat("Fatigue", 10)
    -> escape_sequence

=== truck_encounter_passive ===
{ HasCompanion("Lira") || HasCompanion("Elias"):
    Siete sotto la strada. Sopra le vostre teste c'è una grata pesante che dà sul ciglio della carreggiata. È l'unica uscita.
    Sentite un rombo in avvicinamento. Il terreno trema.
- else:
    Sei sotto la strada. Sopra la tua testa c'è una grata pesante che dà sul ciglio della carreggiata. È l'unica uscita.
    Senti un rombo in avvicinamento. Il terreno trema.
}

Un camion si ferma sulla piazzola di sosta, proprio sopra la grata. Lo pneumatico blocca parzialmente l'apertura.
Sentite voci sopra di voi. Soldati che ridono, accendini che scattano. Cenere calda cade attraverso le sbarre.

L'acqua gelida vi arriva alla vita. L'odore chimico è soffocante.
Devi decidere come gestire la situazione.
{ GetGlobalStat("Fatigue") >= 60:
    Ti senti mancare. Il freddo ti sta intorpidendo le estremità. Trattenere il respiro o rimanere immobili in posizioni innaturali sembra un'impresa titanica.
- else:
    Nonostante il freddo, hai ancora il controllo del tuo corpo. Puoi resistere o agire con precisione.
}

* [Ti immergi completamente per sparire alla vista]
    Decidi di nasconderti sotto il livello dell'acqua putrida per evitare che ti vedano se guardano giù.
    Inspiri a fondo e vai sotto.
    Il liquido nero ti chiude sopra la testa. Il freddo è come una lama nel cervello.
    
    { GetGlobalStat("Fatigue") >= 60:
        Non ce la fai. I polmoni bruciano troppo presto. Il corpo, esausto, ha uno spasmo involontario.
        Emergi tossendo e annaspando, schizzando acqua ovunque.
        "Krazt! Da unten!"
        Una torcia ti illumina in pieno volto mentre cerchi aria.
        ~ alert_level = 100
        ~ truck_alerted = true
        ~ DecreaseGlobalStat("Health", 5)
    - else:
        Rimani immobile, una statua di ghiaccio nel buio. Senti le vibrazioni dei loro passi sopra di te, ma non ti muovi.
        Quando riemergi, minuti dopo, il camion sta ripartendo.
        Sei congelato, ma invisibile.
        ~ IncreaseGlobalStat("Fatigue", 10)
    }
    -> escape_sequence

* [Sposti la grata sincronizzandoti col motore del camion]
    Non puoi aspettare lì sotto. Devi uscire ora, sfruttando il rumore del motore per coprire lo stridio del metallo.
    Aspetti che il camion acceleri a vuoto.
    Spingi. La grata si muove di pochi centimetri.
    Ti fermi.
    Altra accelerata. Spingi ancora.
    
    Riesci a creare un varco appena sufficiente per sgusciare fuori alle spalle del camion, rotolando nell'erba alta del fossato proprio mentre il veicolo riparte.
    È stato rischioso, ma ha funzionato.
    ~ IncreaseGlobalStat("Fatigue", 15)
    -> escape_sequence

* [Aspetti immobile che se ne vadano]
    Non fai nulla. Rimani lì, con l'acqua alla gola, pregando che finiscano la pausa sigaretta.
    Il tempo si dilata all'infinito. Il freddo ti entra nelle ossa, sottraendoti ogni energia residua.
    
    Dopo un'eternità, il camion riparte.
    Sei salvo, ma distrutto dall'ipotermia.
    ~ IncreaseGlobalStat("Fatigue", 20)
    -> escape_sequence

=== escape_sequence_collapse_intro
~ PlayMusicCustomTransition("TheMilitaryRoadEscape", 3, 1.25)

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Uscite dalla grata come animali braccati, spinti solo dall'adrenalina che brucia le ultime riserve di energia.
    Le grida esplodono sopra di voi. Non siete riusciti a sgattaiolare via in silenzio: il vostro trambusto vi ha tradito.
    Il camion, che era fermo sopra di voi, non è partito. I soldati sono lì, armi in pugno.
    Siete esposti.
- else:
    Esci dalla grata come un animale braccato, spinto solo dall'adrenalina che brucia le ultime riserve di energia.
    Le grida esplodono sopra di te. Non sei riuscito a sgattaiolare via in silenzio: il tuo trambusto ti ha tradito.
    Il camion, che era fermo sopra di te, non è partito. I soldati sono lì, armi in pugno.
    Sei esposto.
}
"Krazt! Krazt!"
-> escape_run_logic

=== escape_sequence ===
~ PlayMusicCustomTransition("TheMilitaryRoadEscape", 3, 1.25)

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Superate l'ultima barriera di asfalto e vi lanciate verso il ciglio della strada.
    Davanti a voi, il bosco nero promette salvezza.
- else:
    Superi l'ultima barriera di asfalto e ti lanci verso il ciglio della strada.
    Davanti a te, il bosco nero promette salvezza.
}

{ alert_level < 100 && !truck_alerted:
    Per un istante, il mondo sembra trattenere il respiro. Il rumore dei camion è alle spalle. Il buio vi avvolge, protettivo.
    Sembra fatta.
    
    \*CLACK.*
    
    Il suono secco di un interruttore ad alta tensione spezza l'illusione.
    Dalla torretta di guardia più vicina, un faro alogeno si accende con un ronzio elettrico, tagliando il buio come una lama incandescente.
    
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Il fascio di luce vi colpisce in pieno, cancellando le ombre.
        "Hark kor! Vrast!"
        L'urlo della sentinella non è una domanda, è una constatazione. Vi hanno trovato.
    - else:
        Il fascio di luce ti colpisce in pieno, cancellando le ombre.
        "Hark kor! Vrast!"
        L'urlo della sentinella non è una domanda, è una constatazione. Ti hanno trovato.
    }
    
- else:
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Non c'è tregua. Alle vostre spalle, il caos che avete scatenato si espande come un'onda d'urto.
    - else:
        Non c'è tregua. Alle tue spalle, il caos che hai scatenato si espande come un'onda d'urto.
    }

    "Skratz! Ruz! Ruz!"
    
    Le urla dei soldati si mescolano al latrato dei cani da guerra appena liberati.
    Una sirena inizia a ululare dalla caserma centrale, un suono meccanico e lugubre che rimbalza contro le pareti della valle, svegliando ogni unità nel raggio di chilometri.
    
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Non dovete più nascondervi: dovete solo essere più veloci dei proiettili.
        Le prime raffiche di traccianti iniziano a fischiare sopra le vostre teste, disegnando linee di fuoco giallo nell'aria umida.
    - else:
        Non devi più nascondervi: devi solo essere più veloce dei proiettili.
        Le prime raffiche di traccianti iniziano a fischiare sopra la tua testa, disegnando linee di fuoco giallo nell'aria umida.
    }
}

-> escape_run_logic

=== escape_run_logic ===
Bisogna correre. Non c'è strategia, non c'è piano. Solo puro terrore che pompa adrenalina nelle vene.

* {HasItem("SmokeGrenade")} [Lanci il fumogeno per coprirti la fuga.]
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Strappi la spoletta con i denti e lasci cadere la granata alle vostre spalle. Una coltre densa, grigia, esplode coprendo la ritirata.
    - else:
        Strappi la spoletta con i denti e lasci cadere la granata alle tue spalle. Una coltre densa, grigia, esplode coprendo la ritirata.
    }
    I proiettili nemici mordono il fumo, ciechi e inefficaci.
    ~ RemoveItemFromInventory("SmokeGrenade", 1)
    -> escape_run_continue
    
* {HasItem("Pistol") && HasItem("Ammo")} [Usi la pistola per coprirti la ritirata.]
    Ti giri di scatto, punti l'arma verso i bagliori delle torce e premi il grilletto.
    Due, tre colpi secchi.
    Vedi le sagome degli inseguitori tuffarsi a terra.
    Hai comprato il tempo che ti serviva. Ti giri e riprendi a correre.
    ~ RemoveItemFromInventory("Ammo", 1)
    -> escape_run_continue

* [Corri disperatamente verso gli alberi.]
    { HasCompanion("Lira") || HasCompanion("Elias"):
        Vi gettate in avanti senza guardarvi indietro.
    - else:
        Ti getti in avanti senza guardarti indietro.
    }
    Senti uno strappo bruciante alla spalla. Un proiettile ti ha graziato, portando via solo pelle e carne superficiale, ma il dolore è una fitta acuta che ti toglie il fiato.
    ~ DecreaseGlobalStat("Health", 10)
    -> escape_run_continue

- (escape_run_continue)

{ HasCompanion("Lira") || HasCompanion("Elias"):
    Vi lanciate verso il buio fitto degli alberi.
- else:
    Ti lanci verso il buio fitto degli alberi.
}

I proiettili schioccano contro i tronchi, facendo esplodere schegge di corteccia.
Il terreno è scivoloso. Ogni passo è una lotta contro la gravità e la morte.
Sentite il fiato dei cani alle spalle, i loro latrati si avvicinano rapidamente.

~ IncreaseGlobalStat("Fatigue", 20) 

// --- CHECK FOR FATIGUE COLLAPSE DURING ESCAPE ---
{ GetGlobalStat("Fatigue") >= 100:
    Improvvisamente, il cuore sembra pompare fango invece che sangue. La vista si chiude a tunnel, grigia e sfuocata ai bordi. 
    I suoni della battaglia — gli spari, le urla, i latrati — diventano un ronzio ovattato, lontano, come se la testa fosse sott'acqua.
    
    Inciampi nelle tue stesse gambe, incapaci di rispondere ai comandi. Cadi pesantemente tra i cespugli.
    { HasCompanion("Lira") and HasCompanion("Elias"):
        Senti vagamente le voci dei tuoi compagni che ti urlano di alzarti, ma sembrano provenire da un altro mondo.
    }
    { HasCompanion("Lira") and not HasCompanion("Elias"):
        Senti vagamente la voce di Lira che ti urla di alzarti, ma sembra provenire da un altro mondo.
    }
    { not HasCompanion("Lira") and HasCompanion("Elias"):
        Senti vagamente la voce di Elias che ti urla qualcosa, ma sembra provenire da un altro mondo.
    }
    
    Ti rialzi a fatica, graziato dai cespugli che non ti rendono un bersaglio troppo facile.
    Ansimando, riprendi la fuga disperata.
    
    ~ DecreaseGlobalStat("Health", 15) 
    ~ DecreaseGlobalStat("Fatigue", 40)
}

{ HasCompanion("Elias"):
    Elias è rimasto indietro. La sua vecchia ferita alla gamba non regge questo ritmo infernale. Zoppica vistosamente, perdendo terreno a ogni metro.
    
    { GetGlobalStat("Cohesion") >= 50:
        Ti volti per aiutarlo, ma lui si ferma. Capisce che in due sareste entrambi morti.
        
        { HasCompanion("Lira"):
            "Fauf gann!" urla a Lira, tendendo la mano sporca di fango. "Haart!"
            Lira esita una frazione di secondo, i suoi occhi spalancati per la prima volta. Poi gli lancia la sua arma. "Non fare l'eroe, idiota!" grida, ma la voce le trema, incrinata dalla disperazione.
        - else:
            Un soldato della retroguardia vi è quasi addosso. Elias non scappa. Si appiattisce contro un tronco, lascia passare l'inseguitore e gli si lancia contro.
            C'è una colluttazione feroce nel fango. Elias incassa un calcio, ma riesce a strappare il fucile d'assalto dalle mani del nemico e a calciarlo via.
            Ora è armato.
        }
        
        "Ruz!" ti urla Elias, gli occhi lucidi ma fermi, indicando gli alberi davanti a voi.
        Si gira verso i riflettori accecanti. Si erge in piedi, smettendo di scappare.
        Urla qualcosa nella sua lingua e apre il fuoco.
        
        Vedi i traccianti nemici convergere su di lui come uno sciame di vespe di fuoco. Il suo corpo sussulta una, due, tre volte. Cade in ginocchio, ma continua a premere il grilletto finché non viene inghiottito dalla luce e dal fumo.
        
        Corri via con le lacrime agli occhi e il suono della sua morte nelle orecchie. Il suo sacrificio ha comprato i secondi che vi servivano.
        { HasCompanion("Lira"):
            Lira corre accanto a te, imprecando sottovoce, asciugandosi rabbiosamente una lacrima mista a fango con il dorso della mano. "Dannazione..."
        }
    - else:
        Ti volti. Lo vedi inciampare in una radice esposta. 
        Ti guarda un'ultima volta, con gli occhi colmi di terrore.
        Scarta a lato, cercando di dileguarsi nel folto da solo.
        "Vok! Vok!"
        La sua sagoma solitaria attira il fuoco concentrato.
        
        Il fascio di luce lo inchioda mentre cerca di scavalcare un tronco.
        Una raffica di traccianti lo investe in pieno petto. Vedi il suo cappotto sollevarsi per gli impatti. Elias viene sbalzato all'indietro, le braccia spalancate in una crocifissione grottesca, prima di crollare nel fango immobile.
        
        Vedi le sagome scure dei soldati raggiungerlo. Non si muove più. Un ufficiale estrae la pistola e spara un colpo finale a terra.
        
        { HasCompanion("Lira"):
            "Non fermarti!" ti urla Lira, spingendoti avanti, il suo volto una maschera di orrore puro. "È finita per lui! Muoviti o siamo morti!"
        }
        
        Scappi, lasciandolo lì.
        ~ RemoveCompanionFromParty("Elias")
    }
}

{ HasCompanion("Lira") && !HasCompanion("Elias"):
    Lira ti copre la fuga con una professionalità glaciale, anche se ha il respiro corto. Spara colpi singoli, mirati, costringendo gli inseguitori a tenere la testa bassa.
    "Verso gli alberi!" ordina, la voce roca per lo sforzo.
}

~ StopMusicCustomDuration(2.5)

Corri finché i polmoni non bruciano come se avessi ingoiato vetro.

Finché le luci non sono solo puntini lontani e confusi nella valle sottostante.
Finché il rumore dei motori e degli spari non viene sostituito, finalmente, dal silenzio assoluto del vento tra gli alberi e le rocce.

{ !HasCompanion("Elias") and HasItem("Notebook") and HasItem("Dictionary"): -> diary_reading}

La strada è alle spalle e i soldati hanno persone le vostre tracce.
Il confine è vicino.

~ COMPLETED_ROAD = true
~ CAN_SET_CAMP = true
-> END

=== diary_reading ===
Ti fermi ansimante in una forra stretta, protetta da rocce aguzze che tagliano il vento. Sei al sicuro, almeno per ora.
Ti lasci scivolare a terra, la schiena contro la pietra gelida. Ti tremano le gambe per lo sforzo.

{ HasCompanion("Lira"):
    Lira si siede poco distante, pulendo meticolosamente la canna della sua arma con un lembo della manica. Non ti guarda. Non dice una parola. Siete entrambi troppo scossi e stremati per parlare.
}

Mentre cerchi di regolarizzare il respiro, apri lo zaino per controllare l'equipaggiamento. Le mani ti cadono su un oggetto rigido: il taccuino.
Quello che avevi trovato alla fattoria, appartenuto al soldato nemico. Accanto, il dizionario bilingue appena recuperato.

* [Provi a tradurre parte del taccuino.]

Qualcosa, forse solo il bisogno di distogliere la mente dall'orrore appena vissuto, ti spinge ad aprirlo.
La calligrafia è stretta, ordinata. Cerchi le parole sul dizionario, decifrando le frasi una ad una, lentamente, alla luce debole della luna.

"Mia piccola Sofie."
"Oggi la neve sembra lo zucchero della tua torta."
"Vorrei mandartela, ma si scioglierebbe. Mi manchi."
"Mi manca il pane di tua madre."
"Ho paura di dimenticare le vostre facce."
"Se torno, giuro che non toccherò mai più un fucile..."

Chiudi il taccuino di scatto, come se scottasse.
Ti passi una mano sul volto sporco di fango e polvere da sparo.
Hai disumanizzato un uomo che voleva solo sentire il profumo di casa. Per te era solo un nemico, un ostacolo o un fantasma senza nome. Invece era un padre, un marito, un uomo che sognava lo zucchero e il pane.
La guerra scava via l'uomo e lascia il guscio. Guardando quel taccuino, senti che qualcosa in te, stasera, è morto per sempre.

~ READ_NOTEBOOK = true
~ COMPLETED_ROAD = true
~ CAN_SET_CAMP = true
-> END