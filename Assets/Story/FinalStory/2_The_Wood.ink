INCLUDE Globals.ink

// ============================================================
// SECTION II: THE WOOD
// ============================================================

// --- Narrative variables for this section ---
VAR lira_respect = 0 // -1: Low, 0: Neutral, 1: High
VAR lira_patience = 2 
VAR clues_found = 0
VAR accusation_strength = 0.0 

VAR clue_boots = false
VAR clue_necklace = false
VAR clue_accent = false
VAR clue_posture = false
VAR clue_hands = false
VAR clue_watch = false

-> forest_entry

=== forest_entry ===
~ PlayMusic("TheWood", 2)

Gli alberi sono spettri alti e contorti, le cui fronde nascondono quel poco di luce lunare che riesce a bucare le nuvole.
Il terreno è infido. Radici sporgenti e buche scavate dalle piogge rendono ogni passo una scommessa.

{ HasCompanion("Elias"):
    La nebbia si alza dal sottobosco, fredda, avvolgendovi le caviglie.
    Elias cammina al tuo fianco, zoppicando leggermente. Il suo respiro è pesante, ma tiene il passo. Ogni tanto si ferma, annusa l'aria, scruta il buio con occhi abituati a cercare pericoli diversi dai tuoi. Non dice una parola, ma la sua presenza è un'ancora in quel mare di ombre.
- else:
    La nebbia si alza dal sottobosco, fredda, avvolgendoti le caviglie.
    Sei solo con il ritmo del tuo respiro e lo scricchiolio dei rami secchi sotto gli stivali.
}

Continui a marciare per ore, guidato solo dalla bussola mentale che punta verso ovest, verso casa. La fatica inizia ad appannarti la vista e a rendere le gambe pesanti come tronchi.

~ IncreaseGlobalStat("Fatigue", 10)

Improvvisamente, il silenzio del bosco viene violato.
Un *clic* metallico. Secco.
Viene da sotto il tuo stivale destro.
Il mondo si ferma. Il cuore salta un battito, poi riprende a martellare furioso contro le costole, rimbombando nelle orecchie come un tamburo.

* [Ti congeli all'istante. Non muovi un muscolo.]
    -> mine_assessment
* [L'istinto urla di scappare. Tenti di spostare il peso indietro.]
    -> mine_panic

=== mine_panic ===
// Higher fatigue cost due to physical stress and near-death shock.

Il terrore ti chiude la gola. L'istinto primordiale di fuga prende il sopravvento sulla logica.
Sposti il peso sulla gamba sinistra, i muscoli tesi allo spasmo, pronto a lanciarti nel vuoto.
{ HasCompanion("Elias"):
    La mano di Elias scatta come una trappola. Ti afferra la spalla con una forza sorprendente per un ferito, inchiodandoti sul posto. Ti guarda negli occhi, scuotendo la testa lentamente, il terrore riflesso nel suo sguardo.
- else:
    Ti fermi a mezz'aria, sbilanciato, un istante prima di staccare il piede. Un sudore gelido ti imperla la fronte mentre realizzi quanto sei stato vicino alla fine.
}
Hai sprecato energie preziose lasciandoti dominare dalla paura. Il respiro è corto, irregolare.

~ IncreaseGlobalStat("Fatigue", 15)
-> mine_analysis_logic

=== mine_assessment ===
Respiri a fondo, forzando l'aria nei polmoni. Ordini ai tuoi muscoli di diventare pietra.
Abbassi lo sguardo, millimetro dopo millimetro.
Sotto la suola, parzialmente coperto dal muschio e dalle foglie marce, c'è un disco di metallo arrugginito.
Il percussore è premuto. Se lo rilasci, la molla scatterà.

~ IncreaseGlobalStat("Fatigue", 5)
-> mine_analysis_logic

=== mine_analysis_logic ===
La luce della luna filtra appena, ma basta per vedere i marchi stampigliati sul metallo.
Non sono simboli stranieri.
Riconosci il numero di serie. Riconosci la fattura.
È una mina antiuomo M-14.
È una delle vostre.

Un'amara ironia ti torce lo stomaco. Il comando aveva parlato di campi minati tattici per coprire la ritirata, in caso la battaglia fosse finita male. Non pensavi che saresti stato tu a testarne l'efficacia.
La trappola che doveva uccidere il nemico ora tiene in ostaggio te.

{ HasCompanion("Elias"):
    Elias si inginocchia a distanza di sicurezza, studiando l'ordigno. I suoi occhi scorrono sui dettagli tecnici.
    Mormora qualcosa, indicando il meccanismo a pressione, poi scuote la testa. Sembra capire che è tecnologia tua, non sua. Fa un gesto di impotenza: non sa come disinnescarla senza farti saltare in aria.
- else:
    Sei bloccato. Senza un esperto o attrezzi specifici, sollevare il piede significa morire. Sei una statua di carne in attesa di esplodere.
}

Il tempo passa, dilatandosi all'infinito. Le gambe iniziano a tremare per lo sforzo isometrico di mantenere la pressione costante.
Poi, una luce taglia il buio.

-> lira_appearance

=== lira_appearance ===
Un fascio di luce bianca, accecante, ti investe il viso. Socchiudi gli occhi, cercando di vedere oltre l'abbaglio.
Dalle ombre emerge una figura.
Indossa l'uniforme grigio-verde.

Ma c'è qualcosa che non quadra. Non si muove come una pattuglia ed è sola. Procede bassa, silenziosa, sfruttando ogni copertura come un predatore o un ladro.
È una donna. Il viso è sporco di fango, i capelli corti incollati alla fronte dal sudore.

{ HasCompanion("Elias"):
    Ha una pistola in pugno. L'arma non resta fissa su di te: con un movimento fluido e controllato, la punta verso Elias, l'unica minaccia mobile, per poi oscillare brevemente indietro. Sa che tu non puoi muoverti dalla mina.
    
    // LOGIC ADJUSTMENT FOR ELIAS: Harder difficulty due to distraction.
    // Lira is nervous about two men. Patience reduced to 1 turn (1 Clue pick).
    ~ lira_patience = 1
    
    // STAT ADJUSTMENT: Elias gives a head start on deduction (Base 1.5).
    ~ accusation_strength = 1.5
    
    Elias si irrigidisce al tuo fianco. Dice qualcosa nella loro lingua, una frase secca, quasi un ordine o un saluto formale.
    Lei lo guarda, poi risponde nella stessa lingua aspra. Ma la sua pronuncia è... sbagliata. Le vocali sono troppo aperte, l'accento è marcato, stridente.
    Elias aggrotta la fronte, confuso, e ti lancia un'occhiata significativa. Quella voce non appartiene a quel posto.
    
    ~ clue_accent = true
    ~ clues_found = clues_found + 1
    // Note: accusation_strength already initialized to 1.5 to represent this.
    
    Lei ignora la confusione di Elias e sposta l'arma su di te, visibilmente agitata dalla presenza di un secondo uomo.
    "Krazt!" abbaia.
- else:
    // LOGIC ADJUSTMENT FOR SOLO: Standard difficulty (2 Clue picks).
    // Base Accusation Strength 0.0.
    ~ lira_patience = 2
    ~ accusation_strength = 0.0
    
    Ha una pistola in pugno, puntata dritta al tuo petto. La mano è ferma, professionale.
    "Krazt!" abbaia un ordine secco nella lingua del nemico.
}

Si avvicina, tenendo l'arma salda in posizione. Il fascio di luce scende sul tuo stivale, illuminando la mina, poi risale sul tuo equipaggiamento.
Non chiama rinforzi. Non ti ordina di alzare le mani.
"Toori... na krez?" incalza, con un tono impaziente.

Non capisci nulla. Ti sfugge un'imprecazione.

Lei si blocca. La sorpresa le attraversa il viso sporco di fango per un istante, incrinando la maschera di ostilità.
"La mia lingua?" mormora.
Sposta il dito sulla guardia del grilletto. Il suo tono cambia istantaneamente, abbandonando i suoni gutturali per un accento pulito, perfettamente comprensibile.
"Ascolta bene," dice freddamente. "Sei su una piastra a pressione. Se ti muovi, diventi carne macinata."

I suoi occhi non cercano gradi o mostrine. Cercano zaini e tasche gonfie.
Indossa la divisa del nemico, ma ti sta guardando come un bandito guarda una carovana inerme.

* ["Ti prego, aiutami. Voglio solo tornare a casa..."]
    -> lira_plea
* ["Non mi hai ancora sparato. Deduco che io ti serva vivo."]
    -> lira_pragmatic
* ["Se questa mina scatta, sei abbastanza vicina da morire con me."]
    -> lira_threat

=== lira_plea ===
Cerchi di appellarti alla sua pietà, mostrando i palmi vuoti. "Non siamo una minaccia. Siamo feriti, esausti... vogliamo solo tornare a casa."

Lei ti osserva, un angolo della bocca che si piega in una smorfia di stanchezza, non di disprezzo.
"Casa..." ripete, scuotendo leggermente la testa. "Siamo tutti lontani da casa. E chiedere *per favore* non accorcia la strada."

Il suo tono rimane freddo, pratico.
"Risparmia il fiato. La pietà non disinnesca le mine. Se vuoi uscire vivo da lì, devi offrirmi qualcosa di più concreto delle tue lacrime."

~ lira_respect = -1
-> lira_negotiation_hub

=== lira_pragmatic ===
Ignori la pistola. Vai dritto al punto: "Se volevi un cadavere, ti bastava aspettare che la mia gamba cedesse. Se sono ancora intero, è perché vuoi qualcosa."
La fissi negli occhi, sostenendo il suo sguardo.
"Parla."

Lei abbassa leggermente la pistola. Un angolo della sua bocca si solleva in un mezzo sorriso.
"Vedo che ci capiamo. In tutti i sensi."
Annuisce verso la mina sotto il tuo piede.
"Posso farti scendere intero da lì. Ma la mia competenza tecnica non è gratis."

~ lira_respect = 1
-> lira_negotiation_hub

=== lira_threat ===
Indurisci la mascella. Non sei nella posizione di dare ordini, ma puoi usare l'unica arma che ti resta: la tua stessa morte.
"Se spari, o se io mollo la presa, questo ordigno ci cancella entrambi. Non credere di essere al sicuro a quella distanza."

Lei ride. Una risata secca, priva di gioia.
"Una minaccia?"
Fa un passo indietro, quasi impercettibile. Ha calcolato il raggio dell'esplosione e sa che hai ragione.
"Hai fegato. O forse sei solo disperato."

~ lira_respect = 0
-> lira_negotiation_hub

=== lira_negotiation_hub ===
{ lira_patience <= 0:
    Lei stringe la presa sull'arma, spazientita.
    "Hai finito di fissarmi? La mia pazienza è finita. Dammi la tua roba se non vuoi saltare in aria."
    -> forced_choice_menu
}

{ lira_negotiation_hub == 1:
    Lei si accuccia parzialmente, mantenendo la distanza e l'arma puntata.
    "Ho bisogno di provviste. Cibo. Munizioni. Medicine. Dammi quello che hai, e ti libero la gamba."
    
    La guardi con diffidenza. "E come so che non prenderai il mio zaino per poi lasciarmi qui a esplodere? O peggio, che sai davvero disinnescare questa cosa?"
    
    La donna sbuffa, indicando la tua gamba. "Fisica elementare. Se provi a darmi lo zaino ora, lo sbilanciamento farà scattare la molla. Non posso riscuotere finché sei su quel grilletto."
    
    Si avvicina di un passo, sicura.
    "Devo inserire il fermo prima di prendere il pagamento. Ma chiariamo una cosa: appena sarai libero, io avrò ancora la pistola."
    
    { HasCompanion("Elias"):
        Lancia un'occhiata tagliente a Elias. "Se tu o il tuo amico provate a fare scherzi una volta disinnescata, vi pianto un proiettile in testa. Chiaro?"
    - else:
        Ti fissa negli occhi. "Se provi a fare scherzi una volta disinnescata, ti pianto un proiettile in testa. Chiaro?"
    }
    
    La tensione è estrema. Un singolo spasmo potrebbe mettere fine alla tua vita e l'unica speranza che hai per sopravvivere potrebbe essere una sconosciuta che ti punta una pistola.
    Eppure, un pensiero continua a martellarti la mente: indossa l'uniforme del nemico, ma parla la tua lingua come una madrelingua e si muove da sola nel bosco come un predatore. Una contraddizione troppo strana per essere ignorata.

    { HasCompanion("Elias") && lira_patience == 1:
    Lei lancia un'occhiata nervosa a Elias, poi torna su di te, il dito che freme sul grilletto. "Non ho tempo per i vostri giochi. Scegli. Ora."
    Non hai molto tempo per ragionare.
    }
}

// MATH BALANCE NOTE:
// Threshold: 4.5
// ELIAS (1 Pick, Base 1.5):
// - Pragmatic (+0.5): Need 2.5. Win: Accent(4), Boots(3.5), Watch(3.0), Posture(2.5). (4/6 = 67%)
// - Threat (+0.0): Need 3.0. Win: Accent(4), Boots(3.5), Watch(3.0). (3/6 = 50%)
// - Plea (-0.5): Need 3.5. Win: Accent(4), Boots(3.5). (2/6 = 33%) -> "Much closer to 40% than 16%"

* [Osservi i suoi stivali.]
    -> observe_boots
* [Studi i dettagli della sua uniforme e collana.]
    -> observe_uniform
* [Analizzi la sua postura e come impugna l'arma.]
    -> observe_posture
* [Guardi le sue mani sporche di fango.]
    -> observe_hands
* [Noti un dettaglio al suo polso sinistro.]
    -> observe_watch
* [Ripensi al modo in cui ha cambiato lingua.]
    -> observe_speech
* { clues_found >= 1 && not HasCompanion("Elias") } [Hai visto abbastanza. Tenti di smascherarla.]
    -> unmasking_attempt
* [Basta parlare. Accetti lo scambio.]
    -> trade_with_lira

=== forced_choice_menu ===
* [Accetti lo scambio.]
    -> trade_with_lira
* { clues_found >= 1 } [Tenti di smascherarla.]
    -> unmasking_attempt

=== observe_boots ===
~ lira_patience = lira_patience - 1
~ clues_found = clues_found + 1
~ accusation_strength = accusation_strength + 3.5 // Increased to High Value for balance
~ clue_boots = true

Sposti lo sguardo verso il basso, fingendo di controllare la mina.
I suoi stivali sono coperti di fango, ma la forma è inconfondibile.
Non sono gli stivali di cuoio grezzo, a punta tonda, della fanteria nemica. Hanno una suola Vibram rinforzata e un sistema di allacciatura rapida a ganci incrociati.
Sono anfibi tattici, del tipo in dotazione ai tuoi reparti speciali.
Un pensiero scettico ti attraversa la mente: un nemico potrebbe averli rubati a un cadavere. Tuttavia, l'idea svanisce presto. Le calzano troppo bene, senza le pieghe o i vuoti di una scarpa "recuperata". Sembrano plasmati sul suo piede, come se fossero suoi da sempre.
-> lira_negotiation_hub

=== observe_uniform ===
~ lira_patience = lira_patience - 1
~ clues_found = clues_found + 1
~ accusation_strength = accusation_strength + 2.0 // Low/Medium Value
~ clue_necklace = true

La torcia illumina il suo collo per un istante.
Sotto il colletto rigido della giacca grigio-verde, intravedi il luccichio dell'argento. Una catenina sottile, con un piccolo pendente lavorato.
I soldati nemici portano piastrine di ferro grezzo su cordini di canapa; l'argento lavorato è un lusso raro da quelle parti.
È un dettaglio che stona. Non è un oggetto militare, è un ricordo. E la fattura sembra familiare, troppo raffinata per essere ferraglia locale.
-> lira_negotiation_hub

=== observe_posture ===
~ lira_patience = lira_patience - 1
~ clues_found = clues_found + 1
~ accusation_strength = accusation_strength + 2.5 // Medium Value
~ clue_posture = true

Osservi come si muove. Nonostante il fango e la stanchezza, c'è una disciplina rigida nei suoi gesti.
Il dito indice è steso lungo il castello della pistola, lontano dal grilletto, pronto a scattare ma sicuro. Quando si sposta, controlla sempre il perimetro prima di guardare te.
C'è una differenza abissale con i coscritti nemici, addestrati in massa e spesso traditi dal nervosismo di un dito sempre incollato al grilletto. Lei si muove con una "memoria muscolare" da accademia.
-> lira_negotiation_hub

=== observe_watch ===
~ lira_patience = lira_patience - 1
~ clues_found = clues_found + 1
~ accusation_strength = accusation_strength + 3.0 // High Value
~ clue_watch = true

Mentre aggiusta la presa sull'arma, la manica della giacca scivola per un istante.
Al polso porta un orologio scuro, massiccio. Ma ciò che ti colpisce è come lo indossa: il quadrante è girato verso l'interno del polso.
È una disciplina tattica tipica dei vostri incursori: serve a evitare riflessi accidentali e a leggere l'ora mentre si tiene il fucile in puntamento.
Inoltre, riconosci il modello. Un cronografo con altimetro integrato. Troppo sofisticato per la fanteria di leva che hai affrontato finora.
-> lira_negotiation_hub

=== observe_hands ===
~ lira_patience = lira_patience - 1
~ clues_found = clues_found + 1
~ accusation_strength = accusation_strength + 1.0 // Low Value
~ clue_hands = true

Cerchi di guardare le sue mani mentre gesticola. Sono sporche di terra e grasso per armi.
Le unghie sono tagliate corte, pratiche. C'è sporcizia sotto di esse. Il fango nasconde molto e tre giorni di fuga nel bosco basterebbero a rovinare le mani di chiunque, persino di un ufficiale di alto rango. 
È un indizio debole, ambiguo. Non ti dice nulla di definitivo.
-> lira_negotiation_hub

=== observe_speech ===
~ lira_patience = lira_patience - 1
~ clues_found = clues_found + 1
~ accusation_strength = accusation_strength + 4.0 // Highest Value (Decisive)
~ clue_accent = true

Ripensi al momento in cui ha abbandonato la lingua nemica.
Non c'è stata l'esitazione di chi deve tradurre mentalmente. Il passaggio è stato immediato, fluido, istintivo.
Un nemico può studiare la lingua per anni, ma sotto lo stress di un incontro armato al buio, l'accento nativo tende a riaffiorare o la sintassi a semplificarsi. Lei, invece, possiede la cadenza naturale, le sfumature emotive e il ritmo della tua gente. Una padronanza troppo autentica e viscerale per essere solo frutto di studio scolastico.
-> lira_negotiation_hub

=== unmasking_attempt ===
Decidi di giocare d'azzardo. Se lei è chi pensi che sia, la dinamica di potere cambia drasticamente.
"Non sei una di loro," dici, la voce bassa ma ferma.
Lei si blocca, la mano a mezz'aria sulla mina. Ti guarda con un misto di scherno e curiosità.
"Ah sì? E sentiamo, cosa te lo fa pensare?"

Elenchi tutto ciò che hai notato, sperando che basti a rompere la sua facciata.

{ clue_boots:
    "Indossi stivali tattici dei nostri incursori. Allacciatura rapida. Troppo specifici per essere un semplice bottino di guerra."
}
{ clue_posture:
    "La tua disciplina di fuoco. Dito fuori dal grilletto, controllo del perimetro. Ti muovi come un'operativa addestrata, non come un coscritto spaventato."
}
{ clue_accent:
    "Il tuo accento. Quando sei passata alla mia lingua, non c'è stata esitazione. Quella non è una lingua imparata sui libri, è la tua lingua madre."
}
{ clue_watch:
    "L'orologio. Lo porti all'interno del polso, come insegnano ai nostri incursori. E quel modello con altimetro non lo distribuiscono ai coscritti."
}
{ clue_necklace:
    "Quella catenina d'argento. È artigianato del nord, delle nostre valli... anche se ammetto che potresti averla rubata."
}
{ clue_hands:
    "E le tue mani. Non hanno i segni di chi ha scavato trincee per mesi... per quanto il fango cerchi di nasconderlo."
}

Silenzio. Il bosco sembra trattenere il respiro.
Lei ti fissa. La torcia illumina i suoi occhi, e per un attimo vedi la sua mente calcolare le probabilità.

// Apply Modifier based on Respect
{ lira_respect == 1:
    ~ accusation_strength = accusation_strength + 0.5
}
{ lira_respect == -1:
    ~ accusation_strength = accusation_strength - 0.5
}

{ accusation_strength >= 4.5:
    -> lira_admits
- else:
    -> lira_denies
}

=== lira_admits ===
Lei rimane immobile per un lungo istante, poi le sue spalle si rilassano.
Sorride. Un sorriso vero, stanco, quasi ammirato.
"Osservatore. Molto bene. Hai occhio per i dettagli. Forse troppo."

~ lira_respect = lira_respect + 3 

"Diciamo solo che so riconoscere chi ha ricevuto il mio stesso addestramento," rispondi, sostenendo il suo sguardo. "Anche se indossa una divisa che non gli appartiene."
Lasci la frase in sospeso. Non ne eri certo fino a un attimo fa, ma i pezzi del puzzle combaciavano troppo bene per essere un caso. Non stavi guardando un nemico, né un bandito, ma un riflesso distorto della tua stessa disciplina.

Lei annuisce, un singolo gesto secco di approvazione.
"Già. Immagino sia difficile nascondere certe abitudini a chi sa dove guardare."

Si accuccia rapidamente. Con un gesto fluido, estrae un fermo di metallo e blocca il meccanismo della mina.
"Fatto. Sei libero."

Mentre ti sposti, sentendo il sangue tornare a circolare nella gamba intorpidita, lei si alza e rinfodera la pistola. La maschera del bandito è caduta definitivamente.
"Tenente Lira Vane. Servizi Informazioni."
Si pulisce le mani sui pantaloni, togliendo il fango che usava per camuffarsi.
"La mia copertura è saltata tre giorni fa. Sto cercando di raggiungere il confine, ma è un mattatoio là fuori."

Ti guarda con attenzione, ora che la minaccia immediata è passata.
"E tu? Hai riconosciuto equipaggiamento e procedure. Non sei un civile."

"Caporale. Terza Divisione Fanteria" rispondi, raddrizzando la schiena nonostante la fatica. "O quello che ne rimane. Ho perso la mia unità durante il bombardamento alla valle." Indichi i tuoi abiti logori, privi di mostrine. "Ho dovuto liberarmi della giacca per non essere un bersaglio mobile."

Lira sgrana leggermente gli occhi. Un'ombra di imbarazzo attraversa il suo viso sporco di fango.
"Dannazione. Credevo fossi un disertore o uno sciacallo locale."
Sospira, passando una mano tra i capelli corti.
"Scusa per... l'accoglienza. La mina, la pistola. Non si è mai troppo prudenti qui fuori. Non immaginavo di avere di fronte uno dei nostri."

{ HasCompanion("Elias"):
    Ti guarda, poi sposta lo sguardo su Elias. I suoi occhi si stringono, freddi.
    "E lui?" chiede, indicandolo con un cenno del mento. "Quella è l'uniforme del nemico."
    
    "È un disertore," spieghi rapidamente. "È con me."
    
    Lira non sembra convinta. Fissa Elias, che sostiene il suo sguardo senza aggressività.
    "Non mi fido," sentenzia lei, secca. "Chi tradisce la propria bandiera ha il tradimento nel sangue. Tienilo d'occhio, Caporale."
- else:
    Ti guarda, valutando la situazione.
}

"Da sola sono veloce, ma i miei ordini sono di riportare le informazioni, non di morire eroicamente. Con qualcuno che ti copre le spalle... le probabilità aumentano."
Ti porge la mano.
"Andiamo, so come uscire dal bosco."

~ AddCompanionToParty("Lira")
~ IncreaseGlobalStat("Cohesion", 20)

Accetti la stretta. La sua mano è ruvida, forte.
Hai guadagnato un'alleata preziosa, e forse una guida.

-> end_section_two

=== lira_denies ===
Lei scoppia a ridere. Una risata secca, sprezzante.
"Bella storia. Hai molta fantasia. Peccato che la fantasia non ti salvi la gamba."

{ clue_boots:
    "Stivali belli, vero? Li ho presi a un ufficiale morto tre chilometri a nord. Calzano un po' larghi, ma tengono l'acqua."
}
{ clue_accent:
    "E parlo bene la vostra lingua perché prima di questo inferno studiavo letteratura, non perché sono una spia. Sorpresa: anche il nemico legge libri."
}
{ clue_watch:
    "L'orologio? L'ho preso a un cadavere. Lo porto così perché il vetro è già crepato e non voglio romperlo del tutto."
}
{ clue_hands || clue_necklace:
    "E se pensi che mani pulite o una collanina facciano di me una dei vostri... sei disperato."
}

Non sei riuscito a fare breccia. C'è qualcosa di strano in lei, ma gli indizi che sei riuscito a raccogliere nella tensione della situazione non sono stati sufficienti per provare qualcosa.

~ lira_respect = lira_respect - 1
-> trade_execution

=== trade_with_lira ===
Non puoi rischiare. La tua vita vale più di qualche scatoletta e le tue deduzioni potrebbero essere sbagliate.
"D'accordo. Prendi quello che vuoi."

-> trade_execution

=== trade_execution ===
~ temp lira_took_something = false

{ HasCompanion("Elias"):
    Lei non si muove subito. Punta l'arma dritta alla testa di Elias.
    "Tu. Indietro. Faccia a terra, mani sulla nuca."
    Aspetta che Elias esegua, controllando ogni suo movimento. Solo quando lui è steso nel fango, immobile, lei si avvicina a te.
    Si muove restando alle tue spalle, usandoti come scudo tra lei e il tuo compagno.
- else:
    Lei ti fa cenno di alzare le mani ancora di più. Si avvicina lateralmente, mantenendo una distanza di sicurezza che renderebbe inutile ogni tuo tentativo di disarmarla.
}

Si accuccia, rovistando con una mano in una tasca interna della giacca. Ne estrae un pezzo di fil di ferro rigido, leggermente ossidato. Non è un attrezzo professionale, ma le sue dita sanno esattamente cosa fare.
"Fermo," ordina.
Infila il filo nel foro del meccanismo di sicurezza della mina. Le serve un attimo per trovare l'angolazione giusta, raspando contro il metallo arrugginito, ma alla fine senti un leggero scatto. Piega il metallo per bloccarlo in posizione.
"Fatto. Il percussore è bloccato. Scendi. Molto lentamente."

Sposti il peso, il cuore in gola. Non esplode nulla.
"Lo zaino," ordina lei, indietreggiando subito per rimettervi sotto tiro. "Lascialo a terra, aperto. Poi fai tre passi indietro."

Esegui. Lei si avvicina alla borsa con circospezione, tenendo l'arma alta, e inizia a rovistare con una mano sola.

{ HasItem("Ration") || HasItem("Bandages"):
    { HasItem("Ration") && HasItem("Bandages"):
        Le sue dita si chiudono sulla scatola di razioni, poi afferrano rapidamente anche il rotolo di bende.
        "Cibo e forniture mediche. Giornata fortunata."
        Infila tutto nelle sue tasche con avidità professionale.
        ~ lira_took_something = true
        ~ RemoveItemFromInventory("Ration", 1)
        ~ RemoveItemFromInventory("Bandages", 1)
    - else:
        { HasItem("Ration"):
            Le sue dita si chiudono su una scatola di razioni. Un lampo di sollievo le attraversa gli occhi mentre la sfila rapidamente e se la infila in tasca.
            "Bene. Questo copre il servizio."
            ~ lira_took_something = true
            ~ RemoveItemFromInventory("Ration", 1)
        - else:
            Le sue dita trovano il rotolo di bende. Esita un istante, sperando in altro, poi lo afferra.
            "Mi farò bastare queste."
            Se le infila in tasca con un gesto rapido.
            ~ lira_took_something = true
            ~ RemoveItemFromInventory("Bandages", 1)
        }
    }
    "Piacere di aver fatto affari."
- else:
    { HasItem("Crowbar"):
        La sua mano tocca il metallo freddo del piede di porco. Lo tira fuori per un attimo, lo soppesa con disgusto e lo lascia ricadere nel fango con un rumore sordo.
        "Ferro vecchio? Credevo di aver chiesto provviste, non zavorra."
        Ti guarda con disprezzo.
    - else:
        Rovista per un attimo, trovando solo il fondo vuoto.
        "Vuoto," sibila tra i denti. "Mi hai fatto perdere tempo per un sacco di niente."
        Sembra sul punto di spararti per la frustrazione, ma poi fa un passo indietro.
    }
    "Consideralo un regalo. La prossima volta non sarò così generosa."
}

~ IncreaseGlobalStat("Fatigue", 5) 

"Buona fortuna," dice, indietreggiando nel buio fino a diventare un'ombra. "Ne avrete bisogno."
In un attimo è sparita, lasciandoti vivo e con il tuo zaino<>
{ lira_took_something:
    , seppur più leggero.
}

-> end_section_two

=== end_section_two ===
~ StopMusic()

La tensione cala, lasciando il posto a una stanchezza profonda.
Davanti a te, il bosco inizia a diradarsi.
Il cielo a est si tinge di un grigio perla sporco. L'alba sta arrivando.
E con essa, il vento porta un nuovo odore. Non più terra bagnata e pini, ma qualcosa di acre, pungente.
Odore di legno bruciato e fuliggine.

{ HasCompanion("Elias"):
    Elias zoppica silenziosamente al tuo fianco. L'odore di fumo sembra metterlo in allarme ancora più della foresta; i suoi occhi scrutano l'orizzonte grigio con cupa rassegnazione.
}

{ HasCompanion("Lira"):
    Lira cammina in testa, sicura, controllando il perimetro. Con lei, le ombre sembrano meno minacciose.
}

~ COMPLETED_WOOD = true
~ CAN_SET_CAMP = true
-> END