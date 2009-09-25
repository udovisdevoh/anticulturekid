using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class TestNameMapper
    {
        public static void Test()
        {
            Name aiName = new Name("ai");
            Name humanName = new Name("human");

            NameMapper conceptMapper = new NameMapper(aiName, humanName);
            #region Testing adding new concepts and getting their ids
            int roundId = conceptMapper.GetOrCreateConceptId("round");
            int circleId = conceptMapper.GetOrCreateConceptId("circle");
            int triangleId = conceptMapper.GetOrCreateConceptId("triangle");

            if (roundId != 0)
                throw new Exception("Concept ID doesn't match");
            if (circleId != 1)
                throw new Exception("Concept ID doesn't match");
            if (triangleId != 2)
                throw new Exception("Concept ID doesn't match");

            if (conceptMapper.GetOrCreateConceptId("round") != roundId)
                throw new Exception("Concept ID doesn't match");
            if (conceptMapper.GetOrCreateConceptId("circle") != circleId)
                throw new Exception("Concept ID doesn't match");
            if (conceptMapper.GetOrCreateConceptId("triangle") != triangleId)
                throw new Exception("Concept ID doesn't match");
            #endregion

            #region Testing concept renaming
            conceptMapper.GetOrCreateConceptId("blue");
            int blueConceptId = conceptMapper.GetOrCreateConceptId("blue");
            conceptMapper.Rename("blue", "cyan");
            List<string> namesForBlueConcept = conceptMapper.GetConceptNames(blueConceptId);
            if (namesForBlueConcept.Count != 1)
                throw new Exception("Concept name list count doesn't match");
            if (namesForBlueConcept[0] != "cyan")
                throw new Exception("Concept name doesn't match");
            if (conceptMapper.GetOrCreateConceptId("blue") == blueConceptId)
                throw new Exception("Concept id shouldn't match");
            #endregion

            #region Testing aliasing
            #region When 2nd concept was already there
            conceptMapper.GetOrCreateConceptId("cold");
            int backupCoolId = conceptMapper.GetOrCreateConceptId("cool");
            conceptMapper.AddAlias("cool", "cold");
            int coldId = conceptMapper.GetOrCreateConceptId("cold");
            int coolId = conceptMapper.GetOrCreateConceptId("cool");
            if (coldId != coolId)
                throw new Exception("Concept id do not match");

            List<string> coldNames = conceptMapper.GetConceptNames(coldId);
            if (coldNames.Count != 2)
                throw new Exception("Concept name list count doesn't match");
            if (coldNames[0] != "cold")
                throw new Exception("Concept name doesn't match");
            if (coldNames[1] != "cool")
                throw new Exception("Concept name doesn't match");

            List<string> backupCoolNames = conceptMapper.GetConceptNames(backupCoolId);
            if (backupCoolNames.Count != 0)
                throw new Exception("Concept name list count doesn't match");
            #endregion

            #region When 2nd concept doesn't exist before aliasing
            conceptMapper.GetOrCreateConceptId("hot");
            conceptMapper.AddAlias("warm", "hot");
            int hotId = conceptMapper.GetOrCreateConceptId("hot");
            int warmId = conceptMapper.GetOrCreateConceptId("warm");
            if (hotId != warmId)
                throw new Exception("Concept id do not match");

            List<string> hotNames = conceptMapper.GetConceptNames(hotId);
            if (hotNames.Count != 2)
                throw new Exception("Concept name list count doesn't match");
            if (hotNames[0] != "hot")
                throw new Exception("Concept name doesn't match");
            if (hotNames[1] != "warm")
                throw new Exception("Concept name doesn't match");
            #endregion
            #endregion

            #region Testing unaliasing
            int appleId = conceptMapper.GetOrCreateConceptId("apple");
            conceptMapper.AddAlias("grape", "apple");
            int grapeId = conceptMapper.RemoveAliasAndGetSecondConceptId("apple", "grape");

            if (appleId == grapeId)
                throw new Exception("Concept ids shouldn't match");

            if (conceptMapper.GetOrCreateConceptId("apple") == conceptMapper.GetOrCreateConceptId("grape"))
                throw new Exception("Concept ids shouldn't match");

            if (grapeId != conceptMapper.GetOrCreateConceptId("grape"))
                throw new Exception("Concept do not match");

            List<string> appleNames = conceptMapper.GetConceptNames(appleId);
            List<string> grapeNames = conceptMapper.GetConceptNames(grapeId);

            if (appleNames.Count != 1)
                throw new Exception("Concept name count doesn't match");

            if (grapeNames.Count != 1)
                throw new Exception("Concept name count doesn't match");

            if (grapeNames[0] != "grape")
                throw new Exception("Concept name doesn't match");

            if (appleNames[0] != "apple")
                throw new Exception("Concept name doesn't match");
            #endregion

            #region Testing YOU and ME special keywords conversions
            aiName.Value = "pinocchio";
            humanName.Value = "gepetto";

            int meId = conceptMapper.GetOrCreateConceptId("me");
            int youId = conceptMapper.GetOrCreateConceptId("you");

            if (conceptMapper.GetOrCreateConceptId("gepetto") != meId)
                throw new Exception("Concept ID doesn't match");

            if (conceptMapper.GetOrCreateConceptId("pinocchio") != youId)
                throw new Exception("Concept ID doesn't match");

            int gepettoId = conceptMapper.GetOrCreateConceptId("gepetto");
            if (!conceptMapper.GetConceptNames(gepettoId).Contains("you"))
                throw new Exception("Concept name doesn't match");

            int pinocchioId = conceptMapper.GetOrCreateConceptId("pinocchio");
            if (!conceptMapper.GetConceptNames(pinocchioId).Contains("me"))
                throw new Exception("Concept name doesn't match");

            #region Testing what happens when human or AI change names
            humanName.Value = "einstein";
            if (conceptMapper.GetOrCreateConceptId("einstein") == meId)
                throw new Exception("Concept ID shouldn't match");

            aiName.Value = "frankenstein";
            if (conceptMapper.GetOrCreateConceptId("frankenstein") == youId)
                throw new Exception("Concept shouldn't match");

            meId = conceptMapper.GetOrCreateConceptId("me");
            youId = conceptMapper.GetOrCreateConceptId("you");

            humanName.Value = "einstein";
            if (conceptMapper.GetOrCreateConceptId("einstein") != meId)
                throw new Exception("Concept ID should match");

            aiName.Value = "frankenstein";
            if (conceptMapper.GetOrCreateConceptId("frankenstein") != youId)
                throw new Exception("Concept should match");


            int einsteinId = conceptMapper.GetOrCreateConceptId("einstein");
            if (!conceptMapper.GetConceptNames(einsteinId).Contains("you"))
                throw new Exception("Concept name doesn't match");

            int frankensteinId = conceptMapper.GetOrCreateConceptId("frankenstein");
            if (!conceptMapper.GetConceptNames(frankensteinId).Contains("me"))
                throw new Exception("Concept name doesn't match");
            #endregion
            #endregion
        }
    }
}
