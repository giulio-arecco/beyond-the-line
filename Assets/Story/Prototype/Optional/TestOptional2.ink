INCLUDE ../globals.ink

-> start

=== start === 
    {GetCompanionStat("TestCompanion", "Hunger") > 0:
        Test optional story 2, which is not replayable
    - else:
        -> END
    }
    -> END