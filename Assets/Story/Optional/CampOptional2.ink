INCLUDE ../FinalStory/Globals.ink

-> start

=== start === 
    {HasCompanion("Elias"):
        {GetGlobalStat("Fatigue")> 0:
        Test della storia CampOptional2, che non è rigiocabile. <nl>
        Questa storia è mostrata solo se la statistica globale "Fatigue" è maggiore di 0.
        - else:
            -> END
        }
    - else:
        -> END
    }
    -> END