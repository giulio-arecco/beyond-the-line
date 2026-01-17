INCLUDE ../Globals.ink

-> start

=== start === 
    {HasCompanion("Elias"):
        {GetGlobalStat("Fatigue")> 0:
        Test della storia CampOptional2, che non è rigiocabile. <nl>
        Questa storia è mostrata solo se il compagno TestCompanion ha la statistica "Hunger" maggiore di 0.
        - else:
            -> END
        }
    - else:
        -> END
    }
    -> END