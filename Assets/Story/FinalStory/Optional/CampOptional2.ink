INCLUDE ../globals.ink

-> start

=== start === 
    {HasCompanion("OldFarmer"):
        {GetCompanionStat("OldFarmer", "Hunger") > 0:
        Test della storia CampOptional2, che non è rigiocabile.\n
        Questa storia è mostrata solo se il compagno TestCompanion ha la statistica "Hunger" maggiore di 0.
        - else:
            -> END
        }
    - else:
        -> END
    }
    -> END