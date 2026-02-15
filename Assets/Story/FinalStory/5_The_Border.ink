INCLUDE Globals.ink

// ============================================================
// SECTION V: THE BORDER (ENDINGS)
// ============================================================

-> border_arrival

=== border_arrival ===
~ PlayMusic("TheBorder", 2)

~ CPS_Fatigue_Border_Entry = GetGlobalStat("Fatigue")

Il confine è una ferita aperta nel paesaggio. <nl><>
Davanti a te, la terra di nessuno si estende come un mare di fango, costellato di crateri pieni d'acqua stagnante e reticolati di filo spinato che emergono come rovi d'acciaio.

{ COMPLETED_ROAD:
    L'asfalto della strada militare, che ti ha guidato fin qui come una cicatrice grigia nella valle, si sbriciola improvvisamente nel pantano. <nl><>
    Il rombo costante dei convogli nemici è ormai un'eco lontana alle tue spalle, inghiottita dalla nebbia. Dopo esserti nascosto nell'ombra dei camion e del cemento, il silenzio aperto della frontiera ti colpisce come uno schiaffo.
}

{ COMPLETED_MOUNTAINPASS:
    La discesa dalle miniere ti ha lasciato i polmoni che bruciano e le gambe tremanti per lo sforzo di frenare su ghiaia e rocce instabili. <nl><>
    Ti sei lasciato alle spalle il gas tossico e il vento tagliente delle cime, scendendo dalle nuvole verso il fondo della valle. Ora le montagne incombono alle tue spalle, un muro nero che sei miracolosamente riuscito a valicare.
}

Oltre quella desolazione c'è la tua terra, ma guardando la distesa di macerie, la parola "casa" ti sembra vuota, un concetto astratto che fatica a mettere radici in questo terreno morto.

{ HasCompanion("Elias") || HasCompanion("Lira"):
    -> ending_one
- else:
    -> ending_two
}

// ============================================================
// ENDING I
// ============================================================
=== ending_one ===
{ HasCompanion("Elias"):
    Elias cammina al tuo fianco. Non guarda l'orizzonte, ma il terreno, come se chiedesse perdono a ogni cadavere che calpesta.
}
{ HasCompanion("Lira"):
    Lira avanza in avanscoperta; per la prima volta non la vedi tesa come una corda di violino, con la mano destra pronta ad imbracciare l'arma. I suoi occhi tradiscono una stanchezza che nessun addestramento può mascherare.
}

Raggiungete i resti di quello che doveva essere un avamposto di guardia. Muri sbrecciati che puntano al cielo come dita spezzate. <nl><>
Un suono rompe il silenzio. Un lamento. Basso, ritmico, gorgogliante. Proviene da sotto una trave d'acciaio crollata.

Vi avvicinate. C'è un soldato incastrato lì sotto. Indossa l'uniforme del nemico. <nl><>
È giovane. Troppo giovane. Il suo viso è una maschera di polvere e lacrime che hanno tracciato solchi chiari sulla pelle sporca. Cerca di allungare la mano verso il suo fucile, distante pochi centimetri, ma le sue dita graffiano inutilmente il fango. È debole, allo stremo.

{ HasCompanion("Lira"):
    Lira scatta. L'arma si alza, puntando dritta alla testa del ragazzo. <nl><>
    "Nemico a terra" dice, la voce fredda come il ghiaccio. "Se riesce a liberarsi, potrebbe correre a informare il suo comando".  Il suo dito si tende sul grilletto. È la logica della guerra che l'addestramento vi ha impartito: eliminare la minaccia, cancellare il rischio.
    
    { HasCompanion("Elias"):
        Elias si muove più veloce del pensiero. Si frappone tra la pistola di Lira e il soldato a terra. Non alza le mani. Allarga le braccia, offrendo il petto. <nl><>
        "Na!" abbaia. "Na... krez".
        
        Lira esita. La sua certezza vacilla davanti al corpo del disertore che protegge il nemico. <nl><>
        Elias si volta verso di te. I suoi occhi ti inchiodano. Non ti chiede nulla, aspetta solo di vedere chi sei diventato.
    - else:
        Il soldato a terra ti guarda: nei suoi occhi non c'è odio, ma solo il terrore di un animale in trappola. Ti ricorda lo sguardo di {not HasItem("Notebook"):Elias|quel soldato} nel fienile. <nl><>
        Lira ti lancia un'occhiata rapida. "Caporale, non possiamo lasciare testimoni".
    }
- else:
    Elias si inginocchia accanto al ragazzo, cercando di spostare la trave, ma è troppo pesante per lui da solo. Ti guarda, implorante.
}

Il soldato nemico emette un verso strozzato. Non sai se salvarlo ti rallenterà. Non sai se, una volta libero, cercherà di uccidervi. Non c'è guadagno tattico qui. <nl><>
C'è solo una regola da seguire, ed è quella che hai scritto tu con ogni passo di questo viaggio.

* [Aiuti il ragazzo.]
    ~ CPS_Border_Help_Boy = true
    -> help_the_boy
* [Ignori il ragazzo.]
    ~ CPS_Border_Ignore_Boy = true
    -> ignore_the_boy

=== ignore_the_boy ===
Indurisci lo sguardo. "Andiamo" ordini, la voce che suona metallica alle tue stesse orecchie. "Non possiamo sprecare tempo". <nl><>
Ti volti, lasciando il ragazzo bloccato nel fango, mentre il suo lamento diventa un singhiozzo disperato alle tue spalle.

{ HasCompanion("Elias"):
    Elias resta immobile per un istante. Guarda il soldato a terra, poi guarda te. Nei suoi occhi non c'è una delusione abissale. <nl><>
    "Vrratsk" mormora, sputando a terra. Non verso il nemico, ma vicino ai tuoi stivali. <nl><>
    Esci senza di lui, mentre cerca un modo per aiutare il suo compatriota.
    ~ RemoveCompanionFromParty("Elias")
}

{ HasCompanion("Lira"):
    Lira abbassa lentamente la pistola. Ti fissa, studiando il tuo profilo impassibile. Rinfodera l'arma con un rapido gesto. <nl><>
    "Non amo la persona in cui questa guerra mi ha trasformato" commenta sottovoce, superandoti senza guardarti in faccia. "Ma immagino che la sopravvivenza non abbia un'anima".
}

Attraversi gli ultimi metri di terra di nessuno. Il confine è superato. Sei salvo.

{ COMPLETED_MOUNTAINPASS:
    Mentre il silenzio della tua terra ti accoglie, ti tornano in mente le parole dell'eremita nella baita, tra le cime avvelenate dal gas: "La guerra scava via l'uomo e lascia il guscio". <nl><>
    Aveva ragione. Hai attraversato il fronte, hai vinto la tua battaglia per la sopravvivenza, ma l'uomo che aveva iniziato il viaggio è morto da qualche parte lungo la strada.
    
    Quello che torna a casa è solo un involucro vuoto, capace di guardare un ragazzo morire senza battere ciglio.
- else:
    Hai riportato a casa la pelle, ma hai lasciato indietro tutto il resto. Sei diventato parte del paesaggio desolato che ti lasci alle spalle: una corpo vivo con un'anima spenta.
}

~ COMPLETED_BORDER = true
~ END_OF_STORY = true
-> END

=== help_the_boy ===
Fai un passo avanti, ignorando la pistola di Lira, ignorando l'istinto che ti urla di sopravvivere a ogni costo. <nl><>
Ti chini accanto alla trave d'acciaio. Il metallo è freddo, tagliente.

{ HasCompanion("Elias"):
    Elias capisce subito. Si mette al tuo fianco, spalla contro spalla. <nl><>
    "Drah... ziek" dice con tono fermo. Insieme, spingete.
- else:
    Afferri la trave con entrambe le mani. I muscoli urlano per lo sforzo.
}

~ IncreaseGlobalStat("Fatigue", 10)

La trave si alza. Un centimetro. Due. <nl><>
Il soldato nemico boccheggia, tirando via la gamba schiacciata mentre urla di dolore.

{ HasCompanion("Lira"):
    Lira osserva la scena. La pistola è ancora in pugno, ma la canna si abbassa lentamente verso terra. Per la prima volta, la sua equazione tattica non torna: hai rischiato la vita per salvare il nemico. <nl><>
    Non spara.
}

Il ragazzo vi guarda. Non prova a prendere il fucile. Si rannicchia su se stesso, piangendo. <nl><>
Gli poggi una mano sulla spalla e gli stringi l'uniforme mentre singhiozza. <nl><>
"Torna a casa" gli sussurri, prima di alzarti e rimetterti in marcia.

Raggiungete il fiume che segna il confine vero e proprio. Ti guardi indietro: le rovine si confondono con il cielo grigio. <nl><>
Hai attraversato l'inferno per tornare a casa.

{ COMPLETED_MOUNTAINPASS:
    Il pensiero vola per un istante alla baita fumosa nel valico e alle parole ciniche dell'eremita. Lui sosteneva che la guerra lascia solo gusci vuoti, che siamo solo variabili in un'equazione che dà sempre zero. <nl><>
    Sorridi. Si sbagliava.
    
    Non sei un guscio. Hai scelto di non esserlo. Hai scelto di vedere l'uomo oltre l'uniforme, di spezzare il ciclo dell'indifferenza. <nl><>
    L'eremita ha scelto di morire nella sua menzogna nichilista lassù tra le rocce; tu hai scelto di vivere, e di portare la tua umanità a casa con te.
}

{ HasCompanion("Lira"):
    Lira ripone la pistola nella fondina. Ti guarda, scuotendo la testa. <nl><>
    "Sei un pessimo soldato, caporale" mormora, sorridendo con una dolcezza che non aveva mai lasciato trasparire.
}
{ HasCompanion("Elias"):
    Elias ti stringe l'avambraccio. Un gesto solido, fraterno.
}

Ce l'avete fatta. Siete sopravvissuti.

~ IncreaseGlobalStat("Cohesion", 20)
~ COMPLETED_BORDER = true
~ CPS_Reached_Ending_1 = true
~ END_OF_STORY = true
-> END

// ============================================================
// ENDING II
// ============================================================
=== ending_two ===
Sei solo.

Il vento fischia attraverso i reticolati, portando con sé l'odore della putrefazione. Ti trascini verso le prime trincee della frontiera. <nl><>
La tua patria ti accoglie con un mare di morti: uniformi grigie, uniformi verdi... sono talmente incrostate di fango e sangue secco che non riesci più a distinguerle.

La fame ti torce lo stomaco. La sete ti secca la gola. <nl><>
Davanti a te, riverso a faccia in giù in una pozza d'acqua sporca, c'è un cadavere. Sulla schiena porta una bisaccia che sembra ancora gonfia. Potrebbe esserci del cibo.

* [Giri il corpo per perquisirlo.]
    -> loot_the_corpse

=== loot_the_corpse ===
Lo giri con un calcio, facendolo rotolare sulla schiena con un tonfo umido. <nl><>
Ti accasci sopra di lui, le mani che frugano freneticamente nelle tasche, cercando qualcosa, qualsiasi cosa che ti permetta di vivere un minuto in più.

Trovi una scatoletta di latta arrugginita. Vuota. <nl><>
Poi, il tuo sguardo si ferma sul volto del morto. È giovane, la pelle è cerosa sotto la maschera di fango. Gli occhi sono aperti, vitrei, fissi sul cielo grigio.

{ READ_NOTEBOOK:
    Ti tornano alla mente le parole che avevi tradotto da quel maledetto taccuino. <nl><>
    "Mia piccola Sofie. Oggi la neve sembra lo zucchero della tua torta... Vorrei mandartela, ma si scioglierebbe. Mi manchi" <nl><>
    Il morto ti fissa con occhi vacui. Non tornerà da nessuna Sofie. Non sentirà più l'odore di casa.
    
    "...Se torno, giuro che non toccherò mai più un fucile". <nl><>
    L'uomo che stai guardando ha il fucile a mezzo metro dalla mano irrigidita dalla morte. Le promesse dei morti non valgono niente.
}

Ti sporgi verso l'acqua torbida della pozzanghera in cui giace la testa del morto. Vedi il tuo riflesso accanto al suo viso.

Ti blocchi.

Il volto nell'acqua è irriconoscibile. Sporco di terra, segnato dalla disperazione, gli occhi infossati e vuoti. <nl><>
Guardi il morto. Guardi te stesso. <nl><>
Siete identici.

Non c'è differenza tra la tua uniforme e la sua. <nl><>
Hai combattuto, hai ucciso e hai abbandonato per sopravvivere al nemico, ma mentre fissi quei due volti nell'acqua, capisci la regola finale di questo gioco perverso.

{ COMPLETED_MOUNTAINPASS:
    Quel vecchio alla baita aveva ragione. <nl><>
    "Non siete soldati. Siete variabili in un'equazione che è già stata risolta. Il risultato è sempre zero". <nl><>
    Tu e il corpo dell'uomo dinnanzi a te non siete altro che il lubrificante di un ingranaggio rotto.
}

Non c'è nessun "Loro". Non c'è nessun "Noi". <nl><>
C'è solo un'immensa, sterminata distesa di vittime.

Guardi l'orizzonte: non vedi altro che desolazione in ogni direzione.

Ti rimetti in cammino, verso una casa che accoglierà un uomo profondamente diverso da quello che l'ha lasciata.

~ COMPLETED_BORDER = true
~ CPS_Reached_Ending_2 = true
~ END_OF_STORY = true
-> END