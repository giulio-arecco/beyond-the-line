INCLUDE Globals.ink

// ============================================================
// SYSTEM: CAMPING MECHANIC (THE HUB)
// ============================================================

// --- Local Variables for the Camp Session ---
VAR ate_food = false

-> camp_entry

=== camp_entry ===
~ SET_CAMP_COUNT++
Trovi un anfratto riparato dal vento, nascosto da occhi indiscreti.
Accendi un piccolo fuoco usando legna secca e corteccia. La fiamma è debole, ma il calore che emana è un lusso che quasi avevi dimenticato.

{ HasCompanion("Elias") && HasCompanion("Lira"):
    Elias si siede vicino al fuoco, massaggiandosi la gamba ferita. Lira resta in piedi per un po', controllando il perimetro con l'arma in pugno, prima di accasciarsi a sua volta, esausta.
- else:
    { HasCompanion("Elias"):
        Elias si lascia cadere a terra con un sospiro pesante. La sua uniforme è ormai coperta dello stesso fango che ricopre la tua. Ti guarda e accenna un ringraziamento silenzioso per la sosta.
    }
    { HasCompanion("Lira"):
        Lira si siede a gambe incrociate, smontando e pulendo la sua arma con gesti meccanici. I vostri sguardi non si incrociano.
    }
    { not HasCompanion("Elias") && not HasCompanion("Lira"):
        Sei solo. Il crepitio del fuoco è l'unica voce amica in questo deserto di ombre. Controlli il tuo equipaggiamento, cercando conforto nella routine.
    }
}

È il momento di recuperare le forze, se speri di tornare a casa vivo.

-> camp_hub

=== camp_hub ===
{ camp_hub == 1: 
    { GetGlobalStat("Health") < 30:
        Ti senti debole. Le ferite pulsano a ritmo col tuo cuore e il freddo ti entra nelle ossa troppo facilmente. Hai bisogno di cure.
    }
    { GetGlobalStat("Fatigue") > 70:
        Le palpebre sono pesanti come piombo. Ogni movimento richiede uno sforzo di volontà. Devi riposare.
    }
}

// --- CHOICE HUB ---
* {HasItem("Ration") && not ate_food} [Mangi una razione.]
    -> action_eat -> camp_hub
* {HasItem("Bandages") && GetGlobalStat("Health") < 100} [Usi delle bende per medicare le ferite.]
    -> action_heal_bandages
* {HasItem("Medikit") && GetGlobalStat("Health") < 100} [Usi il medikit per cure approfondite.]
    -> action_heal_medikit
* {HasCompanion("Elias")} [Controlli Elias.]
    {COMPLETED_FARMSTEAD and not ELIAS_OPTIONAL_DIALOGUE_DONE:
        -> elias_optional_dialogue
    - else:
        Elias siede accanto al fuoco, perso nei suoi pensieri.
        -> camp_hub
    } 
* {HasCompanion("Lira")} [Controlli Lira.]
    {COMPLETED_VILLAGE and not LIRA_OPTIONAL_DIALOGUE_DONE:
        -> lira_optional_dialogue
    - else:
        Lira si scalda davanti al fuoco, pulendo la sua arma da fianco.
        -> camp_hub
    }
* [Cerchi di dormire.]
    -> action_sleep

=== action_eat ===
Apri la confezione della razione militare. Il contenuto è freddo, un impasto di carne e grasso che si incolla al palato.
{ HasCompanion("Elias") || HasCompanion("Lira"):
    Dividi il pasto con i tuoi compagni.
    { HasCompanion("Elias"):
        Elias mangia con voracità, quasi con disperazione.
    }
    { HasCompanion("Lira"):
        Lira mangia lentamente, lo sguardo perso nel vuoto.
    }
    In silenzio, consumate quel poco che avete. Non è un banchetto, ma senti lo stomaco smettere di brontolare e un po' di calore diffondersi nel corpo.
- else:
    Mangi in silenzio, assaporando ogni boccone. Non è buono, ma è carburante. Senti lo stomaco smettere di brontolare e un po' di calore diffondersi nel corpo.
}

~ RemoveItemFromInventory("Ration", 1)
~ IncreaseGlobalStat("Health", 10)
~ ate_food = true
->->


=== action_heal_bandages ===
Stringi i denti e applichi le bende sulle ferite più superficiali. <nl>
Il tessuto è ruvido, ma ferma il sanguinamento e protegge la pelle dal freddo e dalla sporcizia. <nl>
Non è una soluzione definitiva, ma il dolore acuto si trasforma in un fastidio sordo e sopportabile.

~ RemoveItemFromInventory("Bandages", 1)
~ IncreaseGlobalStat("Health", 15)
-> camp_hub

=== action_heal_medikit ===
Apri la cassetta di pronto soccorso. L'odore di antisettico ti riempie le narici. <nl>
Pulisci le ferite più profonde, applichi i punti di sutura adesivi e inietti l'antidolorifico. <nl>
Il sollievo è quasi immediato e la mente ti si schiarisce.

~ RemoveItemFromInventory("Medikit", 1)
~ IncreaseGlobalStat("Health", 40)
-> camp_hub

=== elias_optional_dialogue ===
~ ELIAS_OPTIONAL_DIALOGUE_DONE = true
Ti siedi accanto a Elias. Il calore del fuoco illumina il suo volto stanco.

Lui si guarda intorno, assicurandosi che il momento sia tranquillo, poi estrae dalla tasca interna della giacca il taccuino nero. Lo maneggia con una delicatezza che stona con le sue mani sporche e callose. <nl>
Lo apre, cercando una pagina specifica. Vedi righe fitte di scrittura spigolosa, indecifrabile. Poi si ferma su un foglio dove la grafia lascia spazio a un disegno a carboncino.

Te lo porge, indicando le figure con un dito tremante.

* [Ti sporgi per guardare meglio.]
    Vedi due figure abbozzate con tratto incerto ma affettuoso. Una donna dai capelli raccolti e una figura molto più piccola accanto a lei. <nl>
    Elias tocca la figura della donna. "Lena" mormora. La voce è roca. Poi tocca la figura piccola. "S... Sofi."
    
    Ti guarda, gli occhi lucidi che cercano comprensione.
    
    ** ["Tua moglie e tua figlia?"]
        Mimi un anello al dito e indichi l'altezza di un bambino. <nl>
        Elias annuisce vigorosamente, un sorriso triste che gli increspa le labbra. "Du. Lena... Sofi". <nl>
        Si porta una mano al petto, sopra il cuore, poi indica il buio verso nord, la direzione della sua casa.
    
    ** ["Sono... prigioniere?"]
        Aggrotti la fronte, indicando la direzione dei combattimenti. <nl>
        Elias scuote la testa con forza. "Na, na". Fa un gesto di cullare qualcosa, poi mima un tetto sopra la testa con le mani. <nl>
        Casa. Sono a casa. O almeno, è quello che spera.

    -- Elias accarezza la pagina con il pollice, lasciando una scia di fuliggine sulla carta. È l'unico legame che gli è rimasto con un mondo che non sta bruciando.
    
    Poi, lentamente, gira la pagina. <nl>
    C'è scritto qualcosa di diverso qui. Sembra una lettera. Le righe sono ordinate, la calligrafia più curata. <nl>
    Indica il testo, poi te. Scuote la testa: sa che non puoi leggerlo, ma vuole che tu capisca il senso.
    
    Si tocca la gamba ferita, quella che tu hai bendato. Poi indica te. <nl>
    Poi unisce le mani, come in preghiera, ma le apre verso di te. <nl>
    "Drah... ziek" ripete quella parola che aveva detto nel fienile.
    
    Cerca le parole nella tua lingua, faticando visibilmente. <nl>
    "Vita..." dice, con un accento terribile. Indica te, poi se stesso. "...Tu... Aiuto".
    
    ** [Annuisci in silenzio.]
        Non c'è bisogno di dire nulla. Hai fatto quello che dovevi. <nl>
        Lui capisce. Chiude il taccuino lentamente e lo rimette via, vicino al cuore.
    ** ["Siamo solo due uomini che vogliono tornare a casa, Elias."]
        Parli lentamente. Lui ascolta il tono della tua voce, più che le parole. <nl>
        Annuisce, guardando il fuoco. "Casa" ripete.
    
    -- Per un momento, il silenzio tra voi non è carico di tensione, ma di pace. Due nemici scaldati dallo stesso fuoco, uniti dal ricordo di chi li aspetta lontano.
    
    ~ IncreaseGlobalStat("Cohesion", 10)
    -> camp_hub

=== lira_optional_dialogue ===
~ LIRA_OPTIONAL_DIALOGUE_DONE = true
Ti avvicini a Lira. Ha appoggiato la pistola sulle ginocchia e per la prima volta da quando l'hai incontrata, non sembra sull'attenti. Sta semplicemente guardando le braci morire. <nl>
Il riflesso arancione ammorbidisce i tratti spigolosi del suo viso, togliendo per un istante quella maschera di freddezza professionale che indossa come una seconda pelle.

"Silenzio" mormora, senza alzare lo sguardo. "È la cosa che mi manca di più. Il silenzio vero. Non quello che precede uno sparo, ma quello che c'è quando non devi aspettarti nulla."

Ti siedi a distanza rispettosa. "Prima della guerra?" chiedi.

Lira accenna un sorriso amaro. "Prima di tutto questo. Prima che il silenzio diventasse un'arma." <nl>
Si gira verso di te. "Tu sei della fanteria, giusto? Terza Divisione."

* ["Coscritto. Mi hanno chiamato due anni fa."]
    Annuisci. "Ero in officina quando è arrivata la lettera. Tre settimane dopo marciavo nel fango con scarponi troppo stretti."

- Lira annuisce lentamente. "Addestramento standard. Marciare, sparare, obbedire. Ti rompono per ricostruirti come un ingranaggio. Ti insegnano a fidarti del compagno alla tua destra e a odiare quello di fronte."

"E tu?" chiedi. "Non sembri uscita da una caserma."

Lei ride, un suono basso e roco. <nl>
"No. La mia caserma era un'aula universitaria, poi una stanza senza finestre in un seminterrato governativo. Mentre a te insegnavano a pulire un fucile bendato, a me insegnavano a mentire guardando qualcuno negli occhi. A memorizzare accenti, a riconoscere la paura dal modo in cui trema una mano".

Stringe le dita intorno all'impugnatura della pistola, ma non la solleva. <nl>
"Il mio addestramento non era mirato a creare un soldato, ma un fantasma. Mi hanno insegnato che non esistono compagni, solo strumenti. E che il nemico non è qualcuno a cui spari da cento metri, ma qualcuno a cui sorridi mentre gli offri da bere, aspettando il momento giusto per tradirlo".

Ti guarda, lasciandoti intravedere una profonda stanchezza nei suoi occhi. <nl>
"A volte invidio la tua guerra, caporale. Il fango è onesto. Ti sporca fuori. La mia guerra... ti sporca dentro in modi che non puoi lavare via."

* ["Il fango non va via così facilmente, Lira".]
    "Ho visto cose nelle trincee che non mi lasceranno mai. Non c'è onestà nel vedere un amico saltare in aria". <nl>
    Lei ti guarda, colpita dalla tua franchezza. "Hai ragione. Forse siamo solo rotti in modi diversi".
* ["Siamo entrambi qui, ora. Conta solo questo".]
    "Fantasma o soldato, se sbagliamo moriamo entrambi. Le differenze spariscono quando ti sparano addosso". <nl>
    Lira annuisce, apprezzando il pragmatismo. "Già. Il piombo non fa distinzioni di grado".

- Per un lungo istante, il crepitio del fuoco è l'unico suono.
Lira ripone l'arma nella fondina. Il movimento è fluido, ma meno aggressivo del solito. <nl>
"Grazie" dice semplicemente. Non specifica per cosa, ma non serve. È stato un momento di tregua. Un riconoscimento che, sotto le divise e l'addestramento, siete solo due sopravvissuti che cercano di non perdere l'ultima parte di umanità che vi rimane.

~ IncreaseGlobalStat("Cohesion", 10)
-> camp_hub

=== action_sleep ===
{ action_sleep == 1:
    Il fuoco è ormai brace. Il freddo della notte preme contro la tua schiena, cercando un varco nei vestiti.
}

{ ate_food:
    -> sleep_well
}

{ not HasItem("Ration"):
    -> sleep_badly
}

Ti brontola lo stomaco. Faresti meglio a mangiare qualcosa prima di addormentarti.

* [Mangi una razione prima di dormire.]
    -> action_eat ->
    -> sleep_well  

* [Ti addormenti senza mangiare.]
    -> sleep_badly

= sleep_well
Con lo stomaco pieno, il corpo finalmente si rilassa. La stanchezza prende il sopravvento sulla paura. <nl>
Chiudi gli occhi e sprofondi in un sonno senza sogni. Per qualche ora, la guerra non esiste.
~ DecreaseGlobalStat("Fatigue", 40)
~ CAN_SET_CAMP = false
-> END

= sleep_badly
Lo stomaco vuoto è un nodo doloroso. Cerchi di rannicchiarti per conservare calore, ma la fame ti tiene in uno stato di dormiveglia agitato. Ogni rumore ti fa scattare, impedendoti di scivolare nel sonno profondo di cui avresti bisogno. Il riposo è scarso e frammentato.
~ CAN_SET_CAMP = false
-> END