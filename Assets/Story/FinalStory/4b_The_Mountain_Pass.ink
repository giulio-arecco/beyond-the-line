INCLUDE Globals.ink

// ============================================================
// SECTION III-B: THE MOUNTAIN PASS
// ============================================================

VAR elias_fell = false
VAR hermit_threatened = false
VAR bridge_cable_fixed = false
VAR bridge_analyzed = false
LIST crossing_method = crawl, cable, run
VAR mask_broken = false

-> mountain_approach

=== mountain_approach ===
~ CPS_Route_Chosen_Mountain = true
{KNOWN_MOUNTAINPASS_STATUS:
    ~ CPS_Route_Was_Prepared = true
}

~ CPS_Fatigue_MountainPass_Entry = GetGlobalStat("Fatigue")

~ PlayMusic("TheMountainPassWind", 2)

Il vento è una lama di ghiaccio che cerca di trovare uno spiraglio nei vestiti, intorpidendo la pelle fino a farla sembrare cartapesta. <nl><>
Il sentiero si inerpica verso la gola, fiancheggiato da vecchi piloni della teleferica che emergono dalla nebbia come croci di ferro arrugginito.

Davanti a {HasCompanion("Elias") or HasCompanion("Lira"):voi|te}, prima che la strada si restringa verso il vero e proprio valico, sorge una struttura tozza in cemento armato. Un vecchio avamposto per i minatori, o forse un punto di controllo dismesso. La porta è scardinata, ma è l'unico riparo visibile prima dell'abisso.

{ HasCompanion("Elias"):
    Elias si stringe nel cappotto, i denti che battono con un suono ritmico, incontrollabile. Guarda l'edificio con sospetto, ma il freddo è un nemico più immediato di qualsiasi cosa possa nascondersi lì dentro. <nl><>
    "Grahz" sibila stringendosi le braccia al petto, il fiato che si cristallizza all'istante.
}
{ HasCompanion("Lira"):
    "Dobbiamo controllare" dice Lira con voce rauca a causa dell'aria gelida e rarefatta. Si passa una mano sugli occhi arrossati dal vento. "Se dobbiamo attraversare la montagna, ci serve equipaggiamento migliore di questo".
}

-> miners_outpost

=== miners_outpost ===
Varcate la soglia dell'edificio. <nl><>
L'interno è in penombra e l'aria è ferma, pesante, carica d'odore di muffa e grasso per motori.

La stanza principale è devastata. Una parte del soffitto in calcestruzzo ha ceduto, creando una barriera di travi marce, calcinacci e tondini di ferro contorti che taglia in due l'ambiente. <nl><>
Dall'altra parte del crollo, semi-sepolte dai detriti, intravedi delle casse di legno rinforzato. Non sono casse da minatori; il legno è scuro, trattato, e gli angoli sono rinforzati in metallo.

{ KNOWN_MOUNTAINPASS_STATUS:
    Se {HasCompanion("Elias") or HasCompanion("Lira"):avete|hai} intuito correttamente il significato della comunicazione via radio alla casa di comando, la via mineraria potrebbe essere satura di gas. Quelle casse potrebbero contenere l'equipaggiamento che potrebbe fare la differenza tra la vita e la morte. <nl><>
- else:
    Osservi le casse. Sono di fattura militare recente, troppo nuove per questo posto abbandonato. Qualcuno le ha stoccate qui prima che il soffitto crollasse. Se contengono provviste o equipaggiamento, potrebbero fare la differenza tra la vita e la morte. <nl><>
}

Il crollo è instabile. Un masso in bilico minaccia di tirare giù il resto del tetto. Spostare quei detriti richiede forza e precisione. Valuti le tue opzioni.

+ {HasItem("Crowbar")} [Usi il piede di porco per fare leva sulle travi.]
    Incasti il piede di porco tra due blocchi di cemento e spingi. Il metallo stride, ma la leva moltiplica la tua forza. Con uno sforzo controllato, riesci a creare un varco sufficientemente grande da passare senza rischiare che tutto ti crolli addosso.
    ~ IncreaseGlobalStatCapped("Fatigue", 5, FATIGUE_CAP)
    -> loot_mask

+ {HasCompanion("Elias") or HasCompanion("Lira")} [Chiedi aiuto per spostare le macerie.]
    "Ho bisogno di una mano" dici, indicando la trave più grossa. <nl>
    { HasCompanion("Elias"):
        <>Elias annuisce. Si affianca a te, puntando i piedi. Al tuo tre, spingete insieme. Senti i suoi muscoli tendersi allo spasimo accanto ai tuoi. <nl><>
        "Hruk-ta!" grida, e la trave si sposta con un tonfo sordo, alzando una nuvola di polvere.
    }
    { HasCompanion("Lira") and not HasCompanion("Elias"):
        <>Lira valuta il peso, poi si posiziona. "Sincronizziamoci" ordina. <nl><>
        Insieme sollevate il blocco di cemento quel tanto che basta per farlo scivolare via.
    }
    
    Dividere il peso ha risparmiato le tue energie, ma lo sforzo a questa altitudine si fa sentire per tutti.
    
    ~ IncreaseGlobalStatCapped("Fatigue", 5, FATIGUE_CAP)
    ~ IncreaseGlobalStat("Cohesion", 5)
    -> loot_mask

+ [Sposti le macerie a mani nude. Non c'è altra scelta.]
    Afferri la prima trave. È pesante e scheggiata. Tiri con tutto il peso del corpo, sentendo i tendini protestare e il respiro farsi corto. La polvere ti riempie la gola, facendoti tossire. Sposti le pietre una ad una, scorticandoti le mani. Quando finalmente apri un varco, sei madido di sudore che comincia a gelare sulla pelle.
    ~ IncreaseGlobalStatCapped("Fatigue", 10, FATIGUE_CAP)
    -> loot_mask

=== loot_mask ===
Ti insinui nel varco e raggiungi le casse. Con il cuore che rimbomba nel petto, forzi il coperchio della prima.

All'interno, tra paglia e imballaggi antimuffa, trovi una maschera antigas. È un modello vecchio, ingombrante, con un filtro a cartuccia ancora sigillato, ma la gomma sembra integra. Rovesci il contenitore, cerchi freneticamente nelle altre casse schiacciate dal crollo, ma trovi solo filtri scaduti o maschere lacerate. Questa è l'unica funzionante.

~ AddItemToInventory("GasMask")
~ CPS_Found_GasMask++

{ HasCompanion("Elias") or HasCompanion("Lira"):
    Ti giri verso {HasCompanion("Elias"):Elias|Lira} con la maschera in mano. <nl><>
    Una maschera. Siete in {HasCompanion("Elias") and HasCompanion("Lira"):tre|due}.
    
    { GetGlobalStat("Cohesion") >= 50:
        { HasCompanion("Lira"):
            Lira osserva l'oggetto e sospira, passandosi una mano tra i capelli sporchi. "Una è meglio di niente" commenta con voce neutra. "Tienila tu per ora. Decideremo come usarla quando saremo davanti al gas".
        }
        { HasCompanion("Elias") and not HasCompanion("Lira"):
            Elias guarda la maschera, poi incrocia il tuo sguardo. Annuisce lentamente, un gesto misurato. Capisce la gravità della situazione, ma non sembra aspettarsi un miracolo né un tradimento. Resta in attesa.
        }
    - else:
        { HasCompanion("Lira"):
            Lira inarca un sopracciglio, un sorriso amaro sulle labbra. "Certo. Una sola". Ti osserva attentamente. "Vedi di non farti venire strane idee alla 'si salvi chi può'".
        }
        { HasCompanion("Elias") and not HasCompanion("Lira"):
            Elias fissa la maschera, poi te. Deglutisce, lo sguardo inquieto. Si passa la lingua sulle labbra secche, valutando le sue probabilità, ma resta in silenzio, osservando ogni tuo movimento.
        }
    }
}

Non c'è altro che potrebbe tornare utile. {HasCompanion("Elias") or HasCompanion("Lira"):Uscite|Esci} dalla struttura.

-> the_bridge

=== the_bridge ===
Poco oltre l'avamposto, il sentiero si interrompe bruscamente. <nl><>
Una gola profonda taglia la montagna come una ferita infetta. Ciò che unisce i due lati è un ponte di ferro e legno, vecchio di decenni, sospeso sul nulla.

Lo scheletro metallico della struttura è arrugginito ma ancora teso sull'abisso, un arco di tralicci che unisce le due pareti della gola. <nl><>
Il problema è l'impalcato di legno. Le assi sono nere di marciume, gonfie di umidità e gelo; in molti punti sono crollate del tutto, lasciando nude le putrelle di ferro sottostanti come costole scarnificate. Sotto di {HasCompanion("Elias") or HasCompanion("Lira"):voi|te}, decine di metri di caduta libera terminano sulle rocce del torrente ghiacciato, che da qui appare come una linea bianca e irregolare.

Non ci sono altre vie. Le pareti della gola sono verticali e friabili. {HasCompanion("Elias") or HasCompanion("Lira"):Dovete|Devi} passare di qui.

{ KNOWN_MOUNTAINPASS_STATUS:
    Ricordi la trasmissione radio: il ponte a cui si riferiva è senz'altro questo. <nl><>
    Il carico strutturale è compromesso. Qualsiasi peso eccessivo o movimento che inneschi oscillazioni potrebbe far saltare i perni arrugginiti.
- else:
    Il ponte geme sotto le raffiche di vento, un suono simile a metallo che si strappa. Il legno è marcio e il metallo è corroso. Sembra reggersi per miracolo.
}

{ HasCompanion("Elias"):
    Elias si avvicina al bordo, pallido. La sua stazza, in questo momento, è uno svantaggio mortale. Deglutisce visibilmente.
}

{HasCompanion("Elias") or HasCompanion("Lira"):Dovete|Devi} raggiungere l'imbocco del tunnel sull'altro lato. Devi valutare come affrontare la traversata.

+ [Ti fermi per analizzare la struttura.]
    Ti accucci, ignorando il vento che ti frusta il viso, e studi il ponte. <nl><>
    Confermi che l'impalcato di legno è una trappola mortale, ma le due travi portanti laterali in ferro sono continue.Inoltre, vedi un vecchio cavo d'acciaio che corre lungo il passamano di sinistra. È incrostato di ghiaccio spesso, inutilizzabile così com'è per tenersi, ma il metallo sotto sembra integro e ancorato alla roccia.
    
    ++ {HasItem("Crowbar")} [Usi il piede di porco per rompere il ghiaccio sul cavo.]
        Colpisci il ghiaccio con precisione. Le schegge saltano via come vetro. Dopo pochi minuti di lavoro frenetico, hai liberato una sezione sufficiente del cavo per usarla come corrimano sicuro. <nl><>
        "Ora {HasCompanion("Elias") or HasCompanion("Lira"):abbiamo|ho} un appiglio" dici. "{HasCompanion("Elias") or HasCompanion("Lira"):Possiamo|Posso} passare camminando sulle travi ma {HasCompanion("Elias") or HasCompanion("Lira"):tenendoci|tenendomi} al cavo".
        
        ~ IncreaseGlobalStatCapped("Fatigue", 5, FATIGUE_CAP)
        ~ CPS_Info_Points_Gathered++
        ~ bridge_cable_fixed = true
        -> bridge_crossing_choices
        
    ++ [Non hai modo di liberare il cavo. Bisogna scegliere un'altra via.]
        Il ghiaccio è troppo duro per romperlo a mani nude {HasItem("Pistol"):o col calcio della pistola}. Ti rialzi, frustrato.
        -> bridge_crossing_choices

+ Non c'è tempo per pensare. Bisogna muoversi.
    -> bridge_crossing_choices

=== bridge_crossing_choices ===
+ [Procedi strisciando sulle travi portanti.]
    ~ crossing_method = crawl
    ~ CPS_MountainPass_Bridge_Crossing_Crawl = true
    
    È un processo lento, estenuante, ma è l'unico modo per evitare le assi marce e distribuire il peso direttamente sul metallo. <nl><>
    Ti sdrai. Il ferro è gelido attraverso i vestiti, succhia via il calore dal tuo corpo. Ogni raffica di vento fa oscillare l'intera struttura, facendoti chiudere gli occhi per la vertigine.
    ~ IncreaseGlobalStatCapped("Fatigue", 15, FATIGUE_CAP)
    
    { HasCompanion("Elias"): 
        -> elias_accident 
    }
    
    Arrivi dall'altra parte, i muscoli contratti per la tensione, le dita intorpidite.
    
    -> tunnel_entrance

+ {bridge_cable_fixed} [Usi il cavo liberato come sicurezza.]
    ~ crossing_method = cable
    ~ CPS_MountainPass_Bridge_Crossing_Cable = true
    
    {HasCompanion("Elias") or HasCompanion("Lira"):"Tenetevi al cavo!" ordini.|Afferri il cavo con forza.}
    {HasCompanion("Elias") or HasCompanion("Lira"):Avanzate|Avanzi} in piedi sulle travi laterali, ma assicurati. Il cavo d'acciaio regge il peso laterale. Potete muovervi più velocemente che strisciando, scaricando parte del peso sulle braccia invece che sulle gambe tremanti.
    ~ IncreaseGlobalStatCapped("Fatigue", 5, FATIGUE_CAP)
    
    { HasCompanion("Elias"): 
        -> elias_accident 
    }
    
    {HasCompanion("Elias") or HasCompanion("Lira"):Arrivate|Arrivi} dall'altra parte {HasCompanion("Elias") or HasCompanion("Lira"):stanchi ma indenni|stanco ma indenne}.
    
    -> tunnel_entrance

+ [Attraversi di corsa, saltando le assi marce.]
    ~ crossing_method = run
    ~ CPS_MountainPass_Bridge_Crossing_Run = true
    
    {HasCompanion("Elias") or HasCompanion("Lira"):"Andiamo, svelti" ordini. "Distanti tre passi l'uno dall'altro. Saltate dove manca il legno".|Prendi un respiro profondo. Devi essere veloce e leggero.}
    Non ti fidi a restare appeso lì sopra troppo a lungo. Il freddo vi sta uccidendo stando fermi e la struttura geme sotto il vento. <nl><>
    {HasCompanion("Elias") or HasCompanion("Lira"):Iniziate|Inizi} ad attraversare. Il ponte scricchiola sinistramente.
    
    { HasCompanion("Elias"): 
        -> elias_accident 
    }
    
    {HasCompanion("Lira"):Siete|Sei} a metà strada quando una raffica di vento improvvisa investe la gola. <nl><>
    La velocità {HasCompanion("Lira"):vi salva|ti salva}. {HasCompanion("Lira"):Arrivate|Arrivi} dall'altra parte col cuore in gola, appena prima che una sezione di passamano precipiti nel vuoto alle {HasCompanion("Lira"):vostre|tue} spalle.
    
    -> tunnel_entrance

=== elias_accident ===
{ crossing_method == crawl:
    Tocca a Elias. È terrorizzato. Si muove a scatti, abbracciando la trave. <nl><>
    Improvvisamente, un bullone di giunzione salta con un rumore secco proprio mentre lui vi appoggia il peso. La trave vibra violentemente. 
    
    Elias perde la presa e scivola lateralmente. Riesce per miracolo ad aggrapparsi al metallo con le braccia, ma il resto del corpo dondola nel vuoto.
}
{ crossing_method == cable:
    Elias avanza tenendosi al cavo. Improvvisamente, mette un piede su una chiazza di ghiaccio nero invisibile sulla trave. <nl><>
    Gambe all'aria in un istante. Il cavo regge, ma lo strattone gli fa perdere la presa con una mano. Rimane appeso per un solo braccio, roteando sopra l'abisso mentre il vento cerca di staccarlo.
}
{ crossing_method == run:
    Senti un rumore lacerante alle tue spalle. Elias, troppo pesante e scoordinato per quel ritmo, atterra male su un'asse marcia che cede all'istante. <nl><>
    Lui perde l'equilibrio e scompare nel buco, riuscendo ad aggrapparsi a una traversina con una sola mano un attimo prima di precipitare. <nl><>
    "Vras!" urla, la voce strappata dal vento.
}

Il ponte oscilla. Hai un solo istante per reagire.

+ [Ti lanci per afferrarlo]
    ~temp fatigue_threshold = 50
    
    { crossing_method == run: 
        ~ fatigue_threshold = fatigue_threshold - 10 
    }
    { crossing_method == crawl or crossing_method == cable: 
        ~ fatigue_threshold = fatigue_threshold + 10 
    }
    
    Ti butti a terra, sporgendoti nel vuoto. Afferri il polso di Elias. <nl><>
    Il peso del suo corpo ti strappa quasi la spalla.
    
    { GetGlobalStat("Fatigue") > fatigue_threshold:
        { crossing_method == run:
            L'inerzia è troppa e tu sei troppo stanco. Senti i muscoli cedere. Se non molli, ti trascinerà giù con lui.
        - else:
            Le tue dita, intorpidite dal freddo e dallo sforzo, scivolano sul tessuto bagnato del suo cappotto.
        }
        
        I {HasCompanion("Lira"):vostri sguardi si incrociano|suoi occhi incrociano i tuoi}. Terrore puro. <nl><>
        La presa si spezza. <nl><>
        Elias cade nel buio.
        
        ~ RemoveCompanionFromParty("Elias")
        ~ DecreaseGlobalStat("Cohesion", 20)
        ~ CPS_Elias_Bridge_Fall = true
        ~ elias_fell = true
    - else:
        { crossing_method == run:
            Digrigni i denti, urlando per lo sforzo disumano. Rischi di lussarti la spalla, ma contrasti l'inerzia e lo tiri su di peso morto. <nl><>
            Il recupero ti lascia distrutto.
            ~ DecreaseGlobalStat("Health", 20)
            ~ IncreaseGlobalStatCapped("Fatigue", 15, FATIGUE_CAP)
        - else:
            Sfrutti la tua posizione stabile e fai leva. Con uno strattone violento, lo riporti sulla trave. <nl><>
            Vi accasciate sul metallo, tremanti ma salvi.
            ~ DecreaseGlobalStat("Health", 10)
            ~ IncreaseGlobalStatCapped("Fatigue", 15, FATIGUE_CAP)
        }
        
        Lui rotola sul ponte, pallido come un cadavere. "Vok..." sussurra, "Vok". <nl><>
        Vi trascinate fino alla fine del ponte.
    }

+ [È troppo pericoloso. Resti indietro.]
    Il rischio è troppo alto. Il ponte sta per crollare anche sotto di te. <nl><>
    Indietreggi, guardandolo con orrore. "Mi dispiace" sussurri. <nl><>
    Elias cerca di resistere, ma le forze lo abbandonano. Le dita si aprono. Scompare nel vuoto con un urlo strozzato.
    
    Corri verso la fine del ponte per salvarti la vita.
    
    ~ RemoveCompanionFromParty("Elias")
    ~ DecreaseGlobalStat("Cohesion", 20)
    ~ CPS_Elias_Bridge_Fall = true
    ~ elias_fell = true
    
- (gather)

-> tunnel_entrance

=== tunnel_entrance ===
~ PlayMusicCustomTransition("TheMountainPassTunnels", 3, 1.25)

{HasCompanion("Elias") or HasCompanion("Lira"):Siete|Sei} dall'altra parte della gola. Davanti a {HasCompanion("Elias") or HasCompanion("Lira"):voi|te} si apre la bocca nera del tunnel. <nl><>
L'ingresso è segnato da un arco di pietra annerita. Appena {HasCompanion("Elias") or HasCompanion("Lira"):vi avvicinate|ti avvicini}, un odore denso e dolciastro {HasCompanion("Elias") or HasCompanion("Lira"):vi investe|ti investe}. Mandorle amare e zolfo.

Non c'è vento qui. Il gas, più pesante dell'aria, ha saturato la parte bassa del tunnel, coprendo i binari con una marea giallastra, densa come latte andato a male, che vortica pigramente intorno agli stivali. <nl><>
In alto, lungo la parete sinistra, corre una vecchia passerella metallica di servizio, parzialmente arrugginita ma sopra il livello della nebbia tossica.

{ HasCompanion("Elias") and not elias_fell:
    Elias indietreggia bruscamente, coprendosi la bocca. "Vesk!" esclama, la voce strozzata dal terrore. "Vesk-gora!" <nl><>
    Conosce questo odore. Sa cosa fa ai polmoni.
}

{ HasCompanion("Lira"):
    Lira osserva la passerella in alto, poi indica una grata divelta e contorta sulla parete. "Il condotto di ventilazione è crollato" constata freddamente. "Non c'è via d'uscita lassù. Dobbiamo passare dal tunnel principale". <nl><>
    Guarda la passerella di servizio. "Quella passerella è l'unica zona franca dal gas. Ma qualcuno deve restare giù sui binari per sbloccare le paratie di sicurezza... vedo la prima da qui, ed è chiusa".
}

-> toxic_bifurcation

=== toxic_bifurcation ===
{HasCompanion("Elias") or HasCompanion("Lira"):Dovete|Devi} decidere come affrontare la traversata. Il gas in basso è letale senza protezione. La passerella in alto è sicura ma isolata da cancelli chiusi dal basso.

{ HasCompanion("Elias") and not elias_fell:
    Elias guarda la scala di servizio e scuote la testa. È ferito, esausto e corpulento. <nl><>
    "Na... na velk" mormora, indicando la sua gamba. Non ce la farebbe mai ad arrampicarsi e camminare su quella grata sospesa. Per lui, c'è solo il tunnel principale.
}

+ {HasItem("GasMask") && HasItem("Crowbar") && (HasCompanion("Elias") or HasCompanion("Lira"))} [Procedete a squadre separate.]
    ~ CPS_MountainPass_Tunnel_Split_Teams = true
    
    "Ci dividiamo" ordini.
    
    { HasCompanion("Elias") and not elias_fell:
        Consegni la maschera a Elias. "Tu stai giù. È l'unica via per la tua gamba". <nl><>
        Lui la guarda, stupito. "Gah?" <nl><>
        "Io passo di sopra. Vai".
        
        Ti aiuti con il piede di porco per raggiungere la passerella arrugginita{HasCompanion("Lira"): seguito da Lira}.
         ~ IncreaseGlobalStat("Fatigue", 20)
         
        { GetGlobalStat("Fatigue") >= 100: 
            Non appena ti rimetti in piedi sulla passerella, ti si offusca la vista. Ti cedono le gambe e crolli sul freddo metallo arrugginito. <nl><>
            Sei sfinito. Lo sforzo fisico degli ultimi giorni è stato insopportabile e ora il tuo corpo non ce la fa più. 
            -> collapse_physical 
        }
         
        L'aria lassù è respirabile, ma satura di umidità ferrosa. Sotto di voi, Elias cammina nella nebbia tossica.
        
        Avanzate paralleli. Improvvisamente, la passerella è sbarrata da una paratia di sicurezza in ferro, saldata dalla ruggine. Non c'è modo di aprirla da lì. <nl><>
        "Il meccanismo è giù!" urli a Elias attraverso il frastuono del sangue nelle orecchie e il ronzio delle lampade rotte.
        
        Elias non vede nulla. La maschera è appannata, il gas è denso e avvolgente. <nl><>
        "Vex?" grida, la voce ovattata e metallica. <nl><>
        Devi guidarlo a voce.
        
        ++ ["A sinistra! Dietro i detriti!"]
            Elias annaspa, sposta macerie alla cieca. Senti la passerella scricchiolare pericolosamente sotto il tuo peso mentre il metallo stride. <nl><>
            "Drov! Hek!" <nl><>
            Tira una leva arrugginita. Con un gemito metallico che riverbera nel tunnel, la paratia si apre.
            ~ IncreaseGlobalStat("Cohesion", 10)
            -> convergence
    - else:
        "Tieni il piede di porco, meglio se sali tu" dici a Lira. Tu indossi la maschera.
        
        Sei immerso nel gas. È come camminare dentro un liquido sporco. La visibilità è zero, il mondo è ridotto a ombre giallastre. <nl><>
        "Sono bloccata!" urla Lira dall'alto, la voce distante. "Cerca una leva sulla parete destra! Muoviti!"
        
        Annaspi tra i detriti. Il sapore di mandorle amare filtra leggermente, depositandosi sulla lingua come polvere.
        ++ [Cerchi freneticamente tastando la parete.]
            Le dita scorrono sulla roccia viscida. Ti tagli le mani su ferri sporgenti, ma trovi la leva fredda e unta. La tiri con tutto il peso del corpo. <nl><>
            Il cancello in alto si apre con uno schianto.
            
            ~ IncreaseGlobalStat("Fatigue", 10)
            ~ IncreaseGlobalStat("Cohesion", 10)
            
            { GetGlobalStat("Fatigue") >= 100: 
                Non appena rilassi i muscoli e smetti di stringere la leva, ti si offusca la vista, ti cedono le gambe e crolli a terra. <nl><>
                Sei sfinito. Lo sforzo fisico degli ultimi giorni è stato insopportabile e ora il tuo corpo non ce la fa più. 
                -> collapse_physical 
                
            }
            
            -> convergence
    }

+ {HasItem("GasMask") && (HasCompanion("Elias") or HasCompanion("Lira"))} [Condividete la maschera sui binari.]
    ~ CPS_MountainPass_Tunnel_Share_Mask = true
    
    Non potete raggiungere la passerella. Dovete attraversare la sacca di gas sui binari, passandovi l'unica maschera.
    
    "Restiamo uniti" dici. "Respiri contati. Scambio rapido". <nl><>
    Entrate nella nebbia. Il gas è un muro fisico. L'odore di mandorle amare e zolfo vi riempie le narici nei secondi di esposizione, bruciando le mucose come acido nebulizzato.
    
    A metà strada, tocca a te passare la maschera a {HasCompanion("Elias"):Elias|Lira}. Le tue mani sono sudate, tremano per la paura e l'adrenalina. <nl><>
    Nel passaggio concitato, la maschera vi scivola, cadendo a terra. La senti rotolare per qualche metro, ma non riesci a vedere a un palmo dal naso.
    
    Ti getti a terra per recuperarla, mentre ti lacrimano gli occhi per l'esposizione al gas. <nl><>
    La maschera si è incastrata sotto un vecchio carrello minerario, con la cinghia di gomma che sporge oltre le ruote. <nl><>
    {HasCompanion("Elias"):Elias|Lira} è in apnea totale. Inizia a tossire, gli occhi sbarrati dal panico, le vene del collo che pulsano mentre cerca disperatamente aria che non sia veleno.
    
    ++ [Afferri la cinghia e tiri con forza]
        Non c'è tempo per la delicatezza. Tiri con violenza brutale. <nl><>
        La cinghia cede, ma la maschera si libera. 
        
        ~ mask_broken = true
        ~ DecreaseGlobalStat("Health", 5)
        ~ IncreaseGlobalStat("Fatigue", 15)
        
        { GetGlobalStat("Fatigue") >= 100:
            Non appena ti rialzi, ti si offusca la vista. Ti cedono le gambe e crolli sul binario arrugginito. <nl><>
            Sei sfinito. Lo sforzo fisico degli ultimi giorni è stato insopportabile e ora il tuo corpo non ce la fa più. 
            -> collapse_physical
        }
        
        La premi sul volto sudato di {HasCompanion("Elias"):Elias|Lira}. La maschera ora è danneggiata, non terrà più perfettamente, ma {HasCompanion("Elias"):lui|lei} respira, emettendo un suono rauco e disperato. <nl><>
        Correte verso l'uscita.
        
        -> convergence
        
    ++ [Ti fermi a districare il nodo con calma.]
        {HasCompanion("Elias"):"Fermo!"|"Ferma!"} ordini, bloccando le sue mani che annaspano. <nl><>
        Costringi le tue dita a muoversi con precisione chirurgica mentre il gas ti brucia gli occhi facendoli lacrimare copiosamente. Ci vogliono cinque secondi. Cinque secondi eterni in cui il tuo compagno inala veleno, sussultando. <nl><>
        Riesci a recuperare la maschera, ma hai la gola e gli occhi in fiamme.
        
        ~ DecreaseGlobalStat("Health", 15)
        ~ IncreaseGlobalStat("Fatigue", 5)
        
        { GetGlobalStat("Fatigue") >= 100:
            Non appena ti rialzi, ti si offusca la vista. Ti cedono le gambe e crolli sul binario arrugginito. <nl><>
            Sei sfinito. Lo sforzo fisico degli ultimi giorni è stato insopportabile e ora il tuo corpo non ce la fa più. 
            -> collapse_physical
        }
        
        -> convergence

+ {not HasItem("GasMask") and not HasItem("Crowbar") and (HasCompanion("Elias") or HasCompanion("Lira"))} [Correte attraverso il gas.]
    ~ CPS_MountainPass_Tunnel_Group_Run = true
    
    ~ crossing_method = run
    Nessun metodo per evitare il gas. Nessun sostegno per raggiungere la passerella. Non c'è altra scelta che correre in apnea. <nl><>
    "Dobbiamo correre" dici, con voce tremante. "Trattenete il fiato. Non fermatevi".
    
    { HasCompanion("Elias"): Elias annuisce, terrore puro negli occhi. <nl><>}
    { HasCompanion("Lira"): Lira stringe i pugni fino a sbiancare le nocche: "Andiamo". <nl><>}

    Vi lanciate nel tunnel, immergendovi nella nube tossica.
    
    Dopo pochi secondi, il gas penetra ovunque. Il sapore di mandorle amare è sulla lingua, insopportabile. <nl><>
    I polmoni implorano aria, ma inspirare significa morire. <nl><>
    La mancanza di ossigeno crea un effetto tunnel. La vista si restringe. Le ombre dei tuoi compagni diventano macchie confuse nel fumo giallo.
    
    Senti un tonfo sordo, poi un colpo di tosse violento. Qualcuno è caduto o è rimasto indietro. Il panico ti assale.
    
    ++ [Ti volti e torni indietro alla cieca.]
        Non puoi lasciarli. Ti giri nel fumo, annaspando con le braccia. <nl><>
        "Dove siete?!" urli, inalando una boccata letale di gas che ti incendia la trachea.
        
        Afferri una mano nel buio. La tiri con tutta la tua forza. <nl><>
        Senti un corpo trascinarsi contro il tuo. Vi sostenete a vicenda, barcollando verso l'uscita.
        ~ DecreaseGlobalStat("Health", 30)
        ~ IncreaseGlobalStat("Fatigue", 25)
        ~ IncreaseGlobalStat("Cohesion", 20)
        
        { GetGlobalStat("Fatigue") >= 100:
            Mentre vi avvicinate alla luce, la tua vista inizia ad offuscarsi, prima ai lati, poi completamente. I suoni iniziano a rimbobarti come scoppi nelle orecchie, accompagnati dal ritmo del battito cardiaco. <nl><>
            Il gas inalato, la carenza di ossigeno e la fatica devastante degli ultimi giorni stanno colpendo nel momento peggiore. Inciampi nelle tue stesse gambe, crollando al suolo.
            -> collapse_hypoxia
        }
        
        -> convergence

    ++ [Segui la rotaia e urli di seguirti.]
        Non puoi fermarti o morirete tutti. Ti butti a terra, seguendo il metallo freddo della rotaia. <nl><>
        "Seguite la mia voce!" gridi, sputando saliva e cararro.
        
        ~ DecreaseGlobalStat("Health", 20)
        ~ DecreaseGlobalStat("Cohesion", 15)
        ~ IncreaseGlobalStat("Fatigue", 15)
        
        { GetGlobalStat("Fatigue") >= 100:
            Mentre ti avvicini alla luce, la vista inizia ad offuscarsi, prima ai lati, poi completamente. I suoni iniziano a rimbobarti come scoppi nelle orecchie, accompagnati dal ritmo del battito cardiaco. <nl><>
            Il gas inalato, la carenza di ossigeno e la fatica devastante degli ultimi giorni stanno colpendo nel momento peggiore. Inciampi nelle tue stesse gambe, crollando al suolo.
            -> collapse_hypoxia
        }
        
        Procedi carponi, veloce come un animale in trappola. Senti dei passi trascinati dietro di te, ma non ti volti. Non puoi rischiare la tua stessa vita.
        -> convergence

+ {not HasCompanion("Elias") and not HasCompanion("Lira") and HasItem("GasMask")} [Avanzi nel tunnel con la maschera.]
    ~ CPS_MountainPass_Tunnel_Solo_Mask = true
    
    Indossi la maschera e ti immergi nella nebbia. Il respiro rimbomba nelle tue orecchie.
    
    A metà strada, il percorso è sbarrato. Una paratia di sicurezza è scesa sui binari, bloccando il passaggio. <nl><>
    Sei solo. Nessuno sulla passerella in alto può azionare il contrappeso per sollevarla. Nessuno può dirti se c'è un'altra via. Devi trovare il meccanismo manuale qui in basso, nel buio e nel gas.
    
    Tasti la paratia e trovi una ruota idraulica incrostata di ruggine e sale minerale. <nl><>
    Provi a girarla. È bloccata. <nl><>
    Il filtro della maschera inizia a sibilare. Stai consumando aria troppo in fretta per lo sforzo.
    
    ++ [Usi la forza bruta per sbloccare la ruota.]
        Punti i piedi nel fango tossico. Afferri la ruota con entrambe le mani e tiri, urlando nella maschera. Senti i muscoli della schiena che minacciano di strapparsi. La ruggine ti taglia le mani. 
        
        La ruota cede con uno stridio acuto. La paratia si alza di mezzo metro.
        Ti butti sotto, strisciando nel fango.
        ~ IncreaseGlobalStat("Fatigue", 30)
        { GetGlobalStat("Fatigue") >= 100:
            Non appena ti rialzi, ti si offusca la vista, ti cedono le gambe e crolli a terra. <nl><>
            Sei sfinito. Lo sforzo fisico degli ultimi giorni è stato insopportabile e ora il tuo corpo non ce la fa più. 
            -> collapse_physical
        }
        -> convergence
        
    ++ {HasItem("Crowbar")} [Usi il piede di porco come leva.]
        Inserisci il piede di porco tra i raggi della ruota. Fai leva con tutto il peso del corpo. Il metallo si piega, ma la ruota gira. La paratia si alza quel tanto che basta. <nl><>
        Ti getti oltre con il cuore che batte all'impazzata.
        ~ IncreaseGlobalStat("Fatigue", 10)
        { GetGlobalStat("Fatigue") >= 100:
            Non appena ti trovi dall'altro lato, ti si offusca la vista, ti cedono le gambe e crolli nel fango. <nl><>
            Sei sfinito. Lo sforzo fisico degli ultimi giorni è stato insopportabile e ora il tuo corpo non ce la fa più. 
            -> collapse_physical
        }
        -> convergence

+ {not HasCompanion("Elias") and not HasCompanion("Lira") and not HasItem("GasMask")} [Corri attraverso il gas.]
    ~ CPS_MountainPass_Tunnel_Solo_Run = true
    
    ~ crossing_method = run
    Puoi solo contare sui tuoi polmoni. Prendi una boccata d'aria e ti lanci nel tunnel, immergendoti nella nube tossica.
    
    Dopo trenta secondi sembra che i polmoni abbiano preso fuoco. Lacrimi finché il mondo non diventa una macchia indistinta. <nl><>
    La mancanza di ossigeno causa un effetto tunnel immediato e il battito cardiaco ti pulsa incessantemente nelle orecchie.
    
    Inciampi nei binari, cadendo in ginocchio nel fango tossico. Non capisci più dov'è l'uscita. La testa gira violentemente.
    
    ++ [Ti fidi dei tuoi occhi e corri verso le "luci".]
        Vedi un chiarore. Sembra l'uscita. Corri verso di essa. <nl><>
        Sbatti violentemente contro la parete di roccia. Era un'allucinazione. <nl><>
        Cadi. Il gas entra. Tossisci, inalando veleno.
        
        ~ DecreaseGlobalStat("Health", 30)
        ~ IncreaseGlobalStat("Fatigue", 20)
        
        { GetGlobalStat("Fatigue") >= 100:
            Cerchi di rialzarti, ma la vista inizia ad offuscarsi, prima ai lati, poi completamente. I suoni iniziano a rimbobarti come scoppi nelle orecchie, accompagnati dal ritmo del battito cardiaco. <nl><>
            Il gas inalato, la carenza di ossigeno e la fatica devastante degli ultimi giorni stanno colpendo nel momento peggiore.
            -> collapse_hypoxia
        }
        
        Ti rialzi per puro istinto di sopravvivenza, trascinandoti alla cieca. <nl><>
        Arrivi all'uscita per miracolo, strisciando come un animale ferito.
        -> convergence
        
    ++ [Chiudi gli occhi e segui la rotaia al tatto.]
        Ignori i lampi di luce finta. Ti butti a terra, faccia nel fango chimico. La rotaia è fredda, umida, solida. È l'unica cosa reale in questo incubo. La segui con la mano scorticata, usandola come unica indicazione nella nube di gas. <nl><>
        È un processo lento, agonizzante. Senti la pelle bruciare.
        
        ~ DecreaseGlobalStat("Health", 20) 
        ~ IncreaseGlobalStat("Fatigue", 30)
        
        { GetGlobalStat("Fatigue") >= 100:
            Mentre ti avvicini a quella che sembra la luce dell'uscita, la tua vista inizia ad offuscarsi, prima ai lati, poi completamente. I suoni iniziano a rimbobarti come scoppi nelle orecchie, accompagnati dal ritmo del battito cardiaco. <nl><>
            Il gas inalato, la carenza di ossigeno e la fatica devastante degli ultimi giorni stanno colpendo nel momento peggiore. Inciampi nelle tue stesse gambe, crollando al suolo.
            -> collapse_hypoxia
        }
        
        Ma la rotaia ti porta fuori.
        -> convergence

=== collapse_physical ===
~ CPS_Collapse_MountainPass_TunnelPhysical = true

{ HasCompanion("Elias") or HasCompanion("Lira"):
    ~ DecreaseGlobalStat("Cohesion", 20)
    Senti delle mani che ti afferrano, ti scuotono. Voci preoccupate che appaiono infinitamente lontane. <nl><>
    {HasCompanion("Lira"): "Ehi! Non crollare ora!" urla Lira, tirandoti su di peso. }
    {HasCompanion("Elias"): "Nek! Urd nek dar kentar!" Elias ti sostiene la testa, dandoti degli scossoni. }
    
    Ti trascinano per gli ultimi metri. Sei un peso morto, un fardello. <nl><>
    Riemergi lentamente. Ogni muscolo è un nodo di dolore. La vergogna brucia più della fatica mentre ti rimetti in piedi con il loro aiuto.
- else:
    Svieni e rinvieni diverse volte nel giro di minuti che sembrano eterni. Ogni volta che cerchi di rimetterti in piedi crolli di nuovo a terra, finchè non collassi sbattendo la testa e perdi completamente i sensi. <nl><>
    Il risveglio è lento e doloroso. Potresti essere svenuto per secondi o minuti preziosi. <nl><>
    Sei vivo per miracolo. Ti rialzi tremando e ti dirigi verso l'uscita dei tunnel barcollando; ogni passo è un calvario.
}

~DecreaseGlobalStat("Fatigue", 40)
-> convergence

=== collapse_hypoxia ===
~ CPS_Collapse_MountainPass_TunnelHypoxia = true

La gola ti brucia come se fosse piena di tizzoni ardenti. <nl><>
Le gambe non rispondono più.

Luce... casa? Uscita? <nl><>
Cadi sul fianco. <nl><>
Cos'era? Un ratto? Impossibile... il gas... <nl><>
Quello è il soffitto o il pavimento...?

{ HasCompanion("Elias") or HasCompanion("Lira"):
    ~ DecreaseGlobalStat("Cohesion", 20)
    Qualcuno ti sta trascinando. La roccia gratta la pelle. 
    
    "R-respira! Dannazione!" <nl><>
    Uno schiaffo. Aria gelida nei polmoni. Un colpo di tosse che ti strappa il torace.
    
    Apri gli occhi. Lira ti guarda con terrore. Ti hanno dovuto portare fuori di peso. Hai rischiato di uccidere tutti rallentandoli. <nl><>
    Ti senti stordito, inutile.
- else:
    Nulla.
    ...
    ...
    Riemergi come da un annegamento profondo. Il primo respiro è uno spasmo che ti piega in due. I polmoni fischiano, rifiutano l'aria fredda. Sei disteso poco oltre lo sbocco dei tunnel. Devi aver strisciato per puro istinto mentre la mente era spenta. <nl><>
    Ti guardi le mani: sono scorticate a sangue. Hai perso tempo. Hai perso lucidità. La testa pulsa come se volesse aprirsi.
}

~ DecreaseGlobalStat("Fatigue", 40)
-> convergence

=== convergence ===
~ StopMusicCustomDuration(2.5)

All'uscita del tunnel, il terreno sale bruscamente. L'aria torna respirabile, gelida e pura.

{ crossing_method == run:
    Esci strisciando, vomitando muco nero nella neve. Ci vogliono minuti prima che il mondo smetta di girare. I tuoi polmoni fischiano a ogni respiro.
}

{ (HasCompanion("Elias") or HasCompanion("Lira")): 
    Vi riunite sulla neve, guardandovi. Sporchi, intossicati, ma vivi.
}

{ mask_broken:
    Controlli la maschera. La cinghia è spezzata irrimediabilmente. È diventata spazzatura. La getti via.
    ~ RemoveItemFromInventory("GasMask", 1)
}

La discesa verso valle è più dolce qui. La nebbia si dirada a tratti, lasciando intravedere la piana sottostante. <nl><>
Linee di trincee. Filo spinato. Crateri. <nl><>
La terra di nessuno.

{ elias_fell:
    Improvvisamente, senti un rumore di sassi che rotolano alla tua destra, dove il canalone risale per incontrare il sentiero. <nl><>
    Una mano guantata, tremante, afferra il bordo della strada. Poi un'altra.
    
    Elias si issa sulla strada, rotolando sulla schiena. È una maschera di fango ghiacciato, il cappotto lacerato in più punti, le mani sanguinanti. Il suo petto si alza e si abbassa in spasmi violenti. <nl><>
    Ha scalato la parete del canalone, esposto al vento e al gelo, per aggirare il tunnel. È vivo, ma è l'ombra di se stesso. <nl><>
    Si gira verso di te. Non ha la forza di parlare. Nei suoi occhi non c'è accusa, solo puro sfinimento. {HasCompanion("Lira"):Avete tutti|Avete entrambi} attraversato l'inferno.
    
    ~ AddCompanionToParty("Elias")
}

{HasCompanion("Elias") or HasCompanion("Lira"):Avete|Hai} superato il valico, ma {HasCompanion("Elias") or HasCompanion("Lira"):siete tutti|sei} al limite delle forze. Non {HasCompanion("Elias") or HasCompanion("Lira"):riuscirete|riuscirai} ad arrivare alle trincee in queste condizioni.

Poi, noti qualcosa. <nl><>
Poco sotto la linea del sentiero, incastrata in un anfratto di roccia al riparo dal vento dominante, c'è una piccola capanna di pietra. Un filo di fumo azzurrognolo esce da un buco nel tetto.

-> the_hermit_hut

=== the_hermit_hut ===
~PlayMusic("TheMountainPassHermit", 1)

Fumo significa fuoco. Fuoco significa calore, ma significa anche presenza umana, e conosci bene il rischio che questo comporta. <nl><>
Lira estrae la pistola, controllando il caricatore. "Potrebbe essere un posto di osservazione avanzato, o un cecchino" sussurra. <nl>
    
{ HasCompanion("Elias"):
    <>Elias fissa il fumo con desiderio disperato. Trema così forte che i suoi denti fanno un rumore udibile.
}

{HasCompanion("Elias") or HasCompanion("Lira"):Vi avvicinate|Ti avvicini} silenziosamente. Non ci sono fili d'allarme, né impronte di scarponi militari recenti. Solo tracce di legna trascinata. <nl><>
Sulla soglia della capanna, seduto su una cassetta di munizioni vuota, c'è un uomo. È anziano, con una barba ispida e vestiti fatti di pelli e stracci. Pulisce un vecchio fucile da caccia con gesti lenti e metodici.

Non {HasCompanion("Elias") or HasCompanion("Lira"):vi ha ancora visti|ti ha ancora visto}. Oppure finge di non averlo fatto. <nl><>
Devi decidere come procedere. Hai imparato che in questa guerra non tutti quelli che incontri vogliono ucciderti, ma l'errore si paga caro.

* {HasItem("Pistol")} [Avanzi armi in pugno. Minacci l'uomo per prendere il rifugio.]
    ~ hermit_threatened = true
    Scatti in avanti, puntando l'arma. "Mani dove posso vederle!" urli. <nl>
    { HasCompanion("Lira"): <>Lira ti copre immediatamente, puntando alla testa dell'uomo. }
    
    Il vecchio non sussulta. Alza lo sguardo lentamente, posa il fucile a terra e alza le mani aperte. I suoi occhi sono pietre grigie, privi di paura, pieni solo di un'infinita stanchezza. <nl><>
    "Non c'è bisogno di gridare" dice con tono calmo.
    
    {HasCompanion("Elias") or HasCompanion("Lira"):Vi|Ti} fa entrare. Si siede in un angolo, {HasCompanion("Elias") or HasCompanion("Lira"):guardandovi mentre vi sedete.|guardandoti mentre ti siedi.} <nl><>
    Non offre nulla spontaneamente. L'atmosfera è gelida quanto l'esterno. Inizi a riscaldarti davanti al fuoco, ma non riesci a riposare davvero con i suoi occhi addosso.
    -> hermit_dialogue

* [Ti avvicini a mani alzate. Chiedi ospitalità.]
    Fai cenno di abbassare le armi. Ti alzi in piedi, visibile, mostrando le mani vuote.
    
    Il vecchio alza lo sguardo. Non tocca il fucile. {HasCompanion("Elias") or HasCompanion("Lira"):Vi studia|Ti studia} per un lungo istante: {HasCompanion("Elias") or HasCompanion("Lira"):le uniformi lacere|l'uniforme lacera}, {HasCompanion("Elias") or HasCompanion("Lira"):i volti sporchi|il volto sporco} di fuliggine, il tremito delle membra.
    
    Annuisce lentamente, poi sbuffa e fa un gesto verso la porta.
    
    {HasCompanion("Elias") or HasCompanion("Lira"):Vi|Ti} fa entrare. La capanna è piccola, vieni abbracciato dal calore del fuoco. <nl><>
    Mette un pentolino sul fuoco. Zuppa di radici e carne secca.
    { HasCompanion("Elias"):
        Quando porge la ciotola a Elias, vede i resti della divisa nemica sotto il cappotto. Si ferma un secondo. Elias si irrigidisce. <nl><>
        Poi il vecchio grugnisce e gli mette la ciotola in mano.
    }
    
    {HasCompanion("Elias") or HasCompanion("Lira"):Vi|Ti} dà anche delle erbe da masticare per il bruciore alla gola causato dal gas e {HasCompanion("Elias") or HasCompanion("Lira"):vi lascia|ti lascia} accomodare intorno al camino.
    
    ~ DecreaseGlobalStat("Fatigue", 20)
    ~ IncreaseGlobalStat("Health", 10)
    ~ AddItemToInventory("Ration")
    -> hermit_dialogue

=== hermit_dialogue ===
Per un momento il silenzio nella capanna è rotto solo dal crepitio del fuoco. <nl><>
L'eremita {HasCompanion("Elias") or HasCompanion("Lira"):vi|ti} osserva con occhi stanchi. {not hermit_threatened: Pulisce il percussore del suo fucile con movimenti lenti, meccanici, come se stesse riparando un orologio.}

"State scendendo" dice. La sua voce è piatta, raschiata dal fumo. Parla la tua lingua, ma la "r" è dura, gutturale: l'accento del nemico.

- (hermit_first_dialogue)
* ["Dobbiamo attraversare il fronte".]
    "Il fronte" ripete. Sembra assaggiare la parola e trovarla stantia. "Una linea immaginaria tracciata da uomini che bevono vino in stanze riscaldate. Ho visto quella linea spostarsi avanti e indietro per vent'anni. Prima erano gli uomini del re, poi i rivoluzionari, poi la mia gente, ora la tua". <nl><>
    "La terra non riconosce le uniformi. Seppellisce tutti con la stessa indifferenza".
    -> hermit_first_dialogue

* ["Perché vivi qui, isolato da tutto?"]
    Il vecchio alza lo sguardo. I suoi occhi sono lattiginosi, privi di giudizio. <nl><>
    "Perché laggiù bisogna scegliere una bugia per cui morire".
    -> hermit_first_dialogue

* -> 
    Sputa nel fuoco. Le braci sfrigolano. <nl><>
    "Non siete soldati. Siete variabili in un'equazione che è già stata risolta. Il risultato è sempre zero".
    
- (hermit_first_dialogue_end)

* ["Non credi che ci sia una fine a tutto questo?"]
    "La fine?" Fa una smorfia che potrebbe essere un sorriso. "Il conflitto è la condizione naturale. La pace è solo il tempo che serve a ricaricare le armi e a far crescere nuovi figli da mandare al macello. Ho combattuto a lungo. Ogni volta, credevo sarebbe stata l'ultima. Poi è arrivata questa e ce ne sarà un'altra dopo. È un ingranaggio che gira; voi siete solo il lubrificante".

* ["Stiamo solo cercando di tornare a casa".]
    "Casa" mormora. "La guerra scava via l'uomo e lascia il guscio. Anche se tornerai a quella casa, l'uomo che l'ha lasciata sarà morto". <nl>

- {not hermit_threatened: <>Posa il fucile assemblato. Il suono metallico risuona nella stanza. | Scuote il capo e si inarca verso il fuoco, silenzioso.}

{ HasItem("Notebook") and not HasCompanion("Elias"):
    Un pensiero ti attraversa la mente. Frugando nello zaino, le tue dita sfiorano la copertina di cuoio logoro del taccuino di Elias. <nl><>
    Ricordi di non averci dato peso quando lo hai preso. Ma ora... le parole del vecchio ti risuonano dentro: "siete delle variabili...".
    
    * [Gli chiedi se sa tradurre il taccuino]
        Lo tiri fuori. La pelle è fredda al tatto. <nl><>
        "Sai leggere questo?" chiedi, porgendogli il taccuino. "Credo sia scritto nella tua lingua".
        
        Il vecchio lo prende con gesti lenti. Apre la prima pagina. Non c'è emozione sul suo volto, solo la noia di chi ha già letto quella storia mille volte. <nl><>
        "Non sono ordini" dice secco. "È una lista della spesa di un condannato a morte".
        
        "Traduci".
        
        Lui legge, la voce monocorde, priva di pietà: "Mia piccola Sofie. Oggi la neve sembra lo zucchero della tua torta. Vorrei mandartela, ma si scioglierebbe. Mi manchi. Mi manca il pane di tua madre. Ho paura di dimenticare le vostre facce. Se torno, giuro che non toccherò mai più un fucile..."
        
        Chiude il taccuino di scatto e te lo lancia indietro. <nl><>
        "Solo un uomo, che ora sarà cibo per vermi in un fosso dimendicato da dio. La neve coprirà anche lui".
        
        Guardi il taccuino. Ricordi vagamente l'uomo che l'ha scritto e i suoi occhi che incrociavano i tuoi mentre lasciavi la fattoria. <nl><>
        Hai disumanizzato un uomo che voleva solo sentire il profumo di casa. Forse il vecchio ha ragione: la guerra scava via l'uomo e lascia il guscio, e qualcosa in te è già morto.
        ~ READ_NOTEBOOK = true
        ~ CPS_Elias_Notebook_Translated = true
        -> leave_hermit
}

-> leave_hermit

=== leave_hermit ===
~ StopMusic()

Quando {HasCompanion("Elias") or HasCompanion("Lira"):decidete|decidi} di ripartire, il vecchio è fuori che guarda la valle. Non si volta per {HasCompanion("Elias") or HasCompanion("Lira"):salutarvi|salutarti}. <nl><>
{HasCompanion("Elias") or HasCompanion("Lira"):Vi rimettete|Ti rimetti} in marcia. Il riposo {HasCompanion("Elias") or HasCompanion("Lira"):vi ha ridato|ti ha ridato} una parvenza di forze, ma la destinazione finale {HasCompanion("Elias") or HasCompanion("Lira"):vi attende|ti attende}. <nl><>
Il fronte è a pochi chilometri.

~ COMPLETED_MOUNTAINPASS = true
~ CAN_SET_CAMP = true
-> END