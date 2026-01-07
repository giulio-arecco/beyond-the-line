INCLUDE Globals.ink

// ============================================================
// SECTION I: THE FARM
// ============================================================

// DEBUG SETUP ================================================

// ============================================================ 

-> setup

=== setup ===
~AddItemToInventory("Bandages")
-> introduction

=== introduction ===
Il risveglio non è dolce. È uno strappo violento dal nulla, accompagnato da un dolore che ti trapana la tempia a ritmo col battito cardiaco.
Apri gli occhi e il mondo è grigio. Travi di legno carbonizzato tagliano il cielo come le costole di una carcassa esposta alle intemperie.
Ti metti a sedere, la testa che gira. L'aria ha un sapore metallico, di cenere bagnata e pioggia vecchia.
Ti guardi le mani: sporche di terra, tremano leggermente. Non c'è traccia del tuo plotone. Non ci sono ordini urlati nel vento.
Solo il silenzio, pesante e innaturale, che segue la fine del mondo.
Controlli le tue tasche. Niente arma. Solo un rotolo di bende sporche. Sei vulnerabile.

+ [Ti guardi attorno per valutare la situazione.]
    -> farm_exploration

=== farm_exploration ===
Ti alzi a fatica. Le gambe sembrano fatte di piombo fuso e ogni movimento costa uno sforzo di volontà.
~ IncreaseGlobalStat("Fatigue", 5)

Il cortile della fattoria è un cimitero di fango. Il terreno è scuro, impastato con detriti e bossoli d'ottone che brillano ancora, non ossidati dalle intemperie.
Volgi lo sguardo a ovest, verso il bosco. Una colonna di fumo nero, sottile come uno spillo, sale dritta verso le nuvole. Non è un incendio vecchio che cova sotto la cenere. Qualcuno ha acceso un fuoco, o fatto saltare un veicolo, molto di recente.

Il cortile è un campo di battaglia recente, probabilmente al confine dello scontro a cui sei sopravvissuto. I corpi a terra non sono vecchie vittime: la polvere non li ha ancora coperti. I nemici sono sicuramente ancora in zona. 

Restare qui è impensabile, ma ogni passo allo scoperto è un azzardo.

+ [Esamini la piccola dispensa in pietra sulla destra.]
    -> pantry_investigation
+ [Ti dirigi verso il fienile principale.]
    -> barn_approach

=== pantry_investigation ===
La struttura principale è collassata su se stessa, ridotta a un cumulo di pietre instabili. Tuttavia, una piccola dispensa in pietra resiste ostinatamente su un lato del cortile.
La porta è scardinata, pende da un unico cardine arrugginito.
Dalla soglia, tra la polvere che danza nei raggi di luce, intravedi qualcosa. Il luccichio opaco di qualche lattina e il manico rosso di un attrezzo pesante.

Ti avvicini di un passo.
Un crepitio secco, violento come uno sparo, rompe il silenzio.
Il telaio della porta emette un gemito sinistro. L'architrave di legno è spezzato al centro, una V rovesciata che punta alla tua testa. Ogni volta che il vento spinge, una pioggia di calcinacci cade sulla soglia.

+ [Analizzi la struttura con attenzione.]
    La volta è compromessa. C'è un blocco chiave che vibra a ogni folata di vento. Se entri, devi essere veloce come un fulmine, o quella pietra ti schiaccerà. È una trappola mortale in attesa di scattare.
    -> pantry_choices
+ [Ti sembra rischioso, ma le risorse sono vitali.]
    Non sai quanto reggerà, ma la fame è una motivazione potente.
    -> pantry_choices

=== pantry_choices ===
La struttura è un castello di carte. Un movimento brusco, e quel soffitto verrà giù.

+ [Corri dentro e afferri tutto quello che puoi.]
    -> pantry_fast_risk
+ [Provi a muoverti molto lentamente per prendere solo il cibo.]
    -> pantry_slow_risk
+ [Non ne vale la pena. Troppo instabile.]
    Indietreggi lentamente. Meglio affamati che schiacciati.
    -> barn_approach

=== pantry_fast_risk ===
Prendi un respiro profondo e scatti.
Ti lanci nello spazio angusto. Le tue dita si chiudono attorno al metallo freddo dell'attrezzo e riesci ad afferrare una lattina.
Ti volti per uscire, ma lo spostamento d'aria è fatale.

Ti getti all'indietro, ma non sei abbastanza veloce. Un blocco di pietra ti colpisce di striscio alla spalla, scaraventandoti nel fango.
Il dolore è una fitta bianca, accecante.
Alle tue spalle, la dispensa crolla in una nuvola di polvere.

~ AddItemToInventory("Crowbar")
~ AddItemToInventory("Ration")
~ DecreaseGlobalStat("Health", 20)
~ IncreaseGlobalStat("Fatigue", 10)

Stringi il bottino al petto mentre ti rialzi, ansimando. I lividi si faranno sentire, ma hai un po' di cibo e un piede di porco.
-> barn_approach

=== pantry_slow_risk ===
Provi a scivolare dentro come un'ombra, evitando di toccare le pareti.
Allunghi la mano verso una lattina...
Un sassolino cade proprio sul tuo braccio. Alzi lo sguardo: la crepa si sta allargando a vista d'occhio.
Afferri la razione e ti butti fuori un istante prima che l'architrave ceda.

~ AddItemToInventory("Ration")
// No Crowbar obtained, but Health preserved.

Il cuore ti martella nel petto. Hai del cibo, ma l'attrezzo che avevi intravisto è andato.
-> barn_approach

=== barn_approach ===
Ti ripulisci il fango di dosso. L'unico altro edificio che offre una parvenza di riparo è il vecchio fienile.
Le pareti sono crivellate di colpi, ma il tetto sembra reggere.
Le grandi porte di legno sono chiuse, ma non sbarrate.

Ti avvicini, il silenzio rotto solo dal vento.
Poi, ti blocchi.
Un suono.
Dall'interno proviene un rumore soffocato. Un colpo di tosse secco, subito represso, seguito dal fruscio di paglia che viene schiacciata.
C'è qualcuno lì dentro. E sta cercando di non farsi sentire.

+ [Spingi l'anta lentamente, pronto a tutto.]
    -> elias_encounter

=== elias_encounter ===
La luce filtra attraverso le assi sconnesse, illuminando il pulviscolo che galleggia nell'aria.
C'è un uomo accasciato contro una balla di fieno, nell'angolo più in ombra. Non avrà più di trent'anni.
Riconosci subito il grigio-verde della divisa. È l'uniforme del nemico.

Ti irrigidisci, i pugni stretti. Non hai un'arma. Sei vulnerabile.
Anche lui sembra inerme. Non c'è nessun fucile accanto a lui.
Le sue mani sono premute sul fianco, le dita intrecciate e sporche di un rosso scuro e lucido che si allarga sulla giacca. Il suo respiro è un rantolo gorgogliante, umido.

Lui alza gli occhi verso di te. Si tende, come se volesse scattare, ma un sussulto di dolore lo inchioda dov'è.
Accanto a lui c'è una bisaccia aperta, all'interno della quale intravedi una razione militare e un taccuino rilegato in pelle nera.

Le sue labbra si muovono. Emette una serie di suoni gutturali, aspri.
"Vrratsk... na... krez..."
È la lingua del nemico. Non capisci una sola parola. Il tono non sembra una minaccia, ma una constatazione amara. O forse una preghiera.

Sei disarmato di fronte al nemico. Lui è ferito, ma potrebbe nascondere un coltello. Potrebbe urlare per chiamare una pattuglia. 
Oppure è solo un uomo a cui rimane poco da vivere.


+ {HasItem("Bandages")} [Hai delle bende: ti avvicini con le mani in vista e offri aiuto.]
    -> choice_cooperative
+ [Ti servono quelle provviste. Gliele sottrai con la forza.]
    -> choice_aggressive
+ [Non è un tuo problema. Lo ignori e te ne vai.]
    -> choice_avoidant

=== choice_cooperative ===
Ti inginocchi lentamente, tenendo le mani ben visibili. Lui si ritrae contro il fieno, gli occhi spalancati, aspettandosi un colpo.
Invece, tiri fuori le tue bende.
Lui ti fissa, confuso. Non si muove mentre premi il tessuto sulla ferita. Geme, digrigna i denti dal dolore, ma non cerca di colpirti.

~ RemoveItemFromInventory("Bandages", 1) 
~ IncreaseGlobalStat("Cohesion", 30)
~ AddCompanionToParty("Elias")

Quando l'emorragia rallenta, il suo respiro si fa meno convulso.
Ti guarda e annuisce.
Spinge la bisaccia verso di te, estraendo una razione e offrendotela.
Dice qualcosa: "Drah... ziek."
Non hai idea di cosa significhi, ma lo sguardo è riconoscente. Si appoggia a te per alzarsi. È un peso morto, ma è vivo.

-> patrol_arrival

=== choice_aggressive ===
La guerra è sopravvivenza. Lui è il nemico.
Ti avventi su di lui. Lui è troppo debole per reagire. Cerca di proteggere la borsa, ma lo spingi via con una spallata.
Lui cade su un fianco, emettendo un grido strozzato.
Strappi la bisaccia dalla sua presa.

Controlli il bottino. C'è del cibo. E quel taccuino nero.
Lo apri un istante. Le pagine sono fitte di una scrittura spigolosa, aliena. È la loro lingua. Non capisci nulla, a parte qualche schizzo di persone e luoghi a te sconosciuti. È carta straccia, per ora.
Lui ti guarda dal pavimento, ansimante. I suoi occhi ti giudicano in silenzio.
Ti volti e ti allontani.

~ AddItemToInventory("Notebook")
~ AddItemToInventory("Ration")
~ IncreaseGlobalStat("Notoriety", 35)

-> patrol_arrival

=== choice_avoidant ===
Indietreggi verso la porta. Non puoi sprecare risorse per lui, ma non hai il coraggio di infierire.
Lui capisce che te ne stai andando.
Lancia un mattone spezzato nella tua direzione, evidentemente senza l'intento di colpirti.
Voltandoti, lo vedi estrarre il taccuino dalla bisaccia con mano tremante, per poi farlo scivolare sul pavimento verso di te.
Lo raccogli. Lo apri.
È pieno di quella scrittura incomprensibile, fitta e indecifrabile. Sembrano memorie o ordini, corredati dagli schizzi di persone e luoghi a te sconosciuti. Per te sono solo scarabocchi senza senso.
Lui ti guarda, esausto. Accenna un sorriso e chiude gli occhi inumiditi dalle lacrime.
Metti via il taccuino senza dire una parola ed esci.

~ AddItemToInventory("Notebook")

-> patrol_arrival

=== patrol_arrival ===
Il sole è calato, inghiottito da nubi nere.
Improvvisamente, un rombo sordo fa tremare la terra sotto i tuoi piedi.
Fasci di luce bianca tagliano l'oscurità del cortile. Un camion si è fermato all'ingresso della fattoria.
Una pattuglia.
Senti il clangore di equipaggiamento pesante e voci che latrano ordini in quella lingua dura e incomprensibile.
Stanno venendo a controllare gli edifici.

{ HasCompanion("Elias"):
    -> escape_with_elias
- else:
    -> escape_alone
}

=== escape_with_elias ===
Ti schiacci contro la parete esterna del fienile. Il panico ti assale.
Elias ti afferra il braccio. La sua presa è salda, nonostante la ferita.
Dall'altra parte del muro, le voci dei soldati si fanno più vicine. Parlano la sua lingua. Sono i suoi compagni, i suoi fratelli d'armi.
Un pensiero gelido ti attraversa la mente: perché non li chiama? Basterebbe un urlo. Lui sarebbe salvo, soccorso dai suoi medici. Tu saresti finito.
Invece, ti fa segno di tacere, portandosi un dito alle labbra sporche di sangue. Nei suoi occhi non c'è la complicità del traditore, ma il terrore della preda.

Con l'altra mano indica una scolina coperta dai rovi, dietro il fienile. È un sentiero per bestiame, invisibile dalla strada principale.

+ [Ti fidi di lui e lo segui nella scolina.]
    Deglutisci il sospetto e decidi di seguire l'istinto. Lui conosce questo posto.
    Vi muovete bassi, nel fango, strisciando come vermi. I soldati entrano nel cortile urlando ordini che ti fanno accapponare la pelle, ma voi siete già oltre la linea visiva, inghiottiti dalle ombre della vegetazione.
    Arrivate al limitare del bosco illesi. Il frastuono della pattuglia si fa ovattato, distante.
    ~ IncreaseGlobalStat("Fatigue", 5)
    -> end_section_one

+ [Sembra troppo stretto: se vi scoprono sarete bersagli facili. Corri verso il bosco aperto.]
    Il dubbio è troppo forte. E se ti stesse guidando in una trappola?
    Scuoti la testa e scatti verso gli alberi, ignorando il suo consiglio. Elias è costretto a seguirti zoppicando, imprecando sottovoce.
    Il movimento attira l'attenzione di una guardia.
    Scoppia un proiettile alle vostre spalle, che scheggia la corteccia di un albero a un metro da te.
    Vi buttate nel sottobosco, graffiandovi viso e mani, correndo finché il fiato non manca e i polmoni bruciano.
    Dopo diversi minuti di corsa disperata il vociare alle vostre spalle si fa sempre più indistinto, fino a placarsi del tutto. Pare che la pattuglia abbia perso le vostre tracce.
    ~ IncreaseGlobalStat("Fatigue", 15)
    ~ DecreaseGlobalStat("Health", 5)
    -> end_section_one

=== escape_alone ===
Sei solo. Le luci dei fari spazzano il cortile.
Senti passi pesanti avvicinarsi al fienile. Tra pochi secondi saranno qui.

+ [Ti nascondi dietro una catasta di legna e aspetti che passino.]
    Ti rannicchi nel buio, trattenendo il respiro.
    Due soldati passano a pochi metri. Uno si ferma. Punta la torcia verso di te.
    Ti vedono!
    Urla. Scatti in piedi e corri disperatamente verso il retro, mentre i proiettili fischiano intorno a te.
    Uno ti colpisce di striscio al fianco mentre ti tuffi tra gli alberi.
    Dopo diversi minuti di corsa disperata il vociare alle tue spalle si fa sempre più indistinto, fino a placarsi del tutto. Pare che la pattuglia abbia perso le tue tracce.
    ~ DecreaseGlobalStat("Health", 10)
    ~ IncreaseGlobalStat("Fatigue", 20)
    -> end_section_one

+ [Corri subito verso il retro, sfruttando il momento di confusione.]
    Non aspetti. Appena le luci si spostano, scatti.
    Il fango rallenta la tua corsa. Inciampi in un filo spinato nascosto nell'erba alta, lacerandoti i vestiti e la pelle.
    Ti rialzi e ti butti nella macchia scura degli alberi prima che possano vederti.
    Dopo esserti accertato che non ti abbiano visto, inizi a farti strada nel sottobosco. Non sei sicuro di dove tu stia andando; per ora ti basta sapere che ti stai allontanando da quegli uomini.
    ~ DecreaseGlobalStat("Health", 10)
    ~ IncreaseGlobalStat("Fatigue", 15)
    -> end_section_one

=== end_section_one ===
Passa almeno un'ora.
Il sole è scomparso da tempo oltre l'orizzonte e il bagliore proveniente dalla fattoria è ormai un ricordo lontano, inghiottito dai tronchi neri degli alberi.
Il bosco è freddo, umido, indifferente alla guerra.

{ HasItem("NoteBook"): 
    Il taccuino pesa nella tasca come un mattone, il suo contenuto un mistero indecifrabile.
}

{ HasCompanion("Elias"):
    -> elias_confrontation
- else:
    Sei di nuovo solo con i tuoi fantasmi. Il silenzio è rotto solo dal tuo respiro affannoso.
    ~ COMPLETED_FARMSTEAD = true
    -> END
}

=== elias_confrontation ===
Elias si siede su una radice sporgente. La mano corre istintivamente al fianco, ma non preme più con la disperazione di prima.
La macchia rossa sulla benda che gli hai applicato non si è allargata ulteriormente; il tessuto ha tenuto. Il suo respiro, sebbene ancora pesante, ha perso quel rantolo gorgogliante che ti aveva preoccupato nel fienile. Sembra che l'emorragia si sia arrestata, almeno per ora.
Siete due sopravvissuti, ma l'uniforme che indossa è ancora quella del nemico.
Non riesci a toglierti dalla testa quello che è successo. Quegli uomini erano la sua gente. Eppure, ti ha coperto le spalle. Ha scelto la fuga con uno sconosciuto nemico invece della salvezza con i suoi.

+ [Cerchi di chiedergli perché.]
    Ti avvicini di un passo, indicando col pollice la direzione da cui siete venuti, dove i motori della pattuglia si sono spenti.
    "Perché?" chiedi, la voce roca. "Erano i tuoi."
    Lui alza lo sguardo. Ti fissa, poi scuote la testa lentamente.
    "Vrratsk... ni... kraz," mormora.
    Non capisci le parole, ma il tono è privo di ostilità. Sembra stanco.
    Mima il gesto di legare una ferita. Poi fa un cenno vago verso il buio, lontano dalla direzione della pattuglia, scuotendo la testa con un'espressione amara. C'è qualcosa nel suo sguardo, un rifiuto che va oltre la paura della cattura. Forse quei soldati non stavano cercando solo un disperso. Forse stavano dando la caccia a qualcuno che non voleva essere trovato.
    -> final_reflection

+ [Lo osservi in silenzio, diffidente.]
    Resti a distanza, studiando ogni suo movimento. Non sai se ringraziarlo o aspettarti una coltellata alla schiena.
    Lui intercetta il tuo sguardo sospettoso. Non si difende. Non sorride.
    Si limita a indicare il sentiero che si inoltra nel bosco, poi fa un gesto secco con la mano: avanti.
    Capisce che la fiducia è un lusso che non potete permettervi. Ma la sopravvivenza richiede cooperazione.
    -> final_reflection

+ [Gli fai un cenno secco di ringraziamento.]
    Annuisci, un gesto breve, militare. Riconosci l'aiuto tattico, niente di più.
    Lui ricambia con un cenno altrettanto impercettibile.
    Non servono parole. Avete entrambi scelto di vivere.
    -> final_reflection

=== final_reflection ===
Elias si alza a fatica, facendoti cenno di proseguire.
Siete due estranei, nemici per giuramento, uniti solo dalla volontà di non morire stanotte.
Vi incamminate nel buio, l'uno l'ombra dell'altro.
~ COMPLETED_FARMSTEAD = true

-> END