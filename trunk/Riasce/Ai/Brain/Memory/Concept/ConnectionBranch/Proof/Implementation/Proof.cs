using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    public class Proof : AbstractProof
    {
        #region Fields
        private List<List<Concept>> argumentList;

        private List<Concept> statementToProove = null;
        #endregion

        #region Constructors
        public Proof()
        {
            argumentList = new List<List<Concept>>();
        }

        public Proof(Concept subject, Concept verb, Concept complement)
        {
            statementToProove = new List<Concept>() { subject, verb, complement };
            argumentList = new List<List<Concept>>();
        }
        #endregion

        #region Methods
        public override void AddArgument(Concept subject, Concept verb, Concept complement)
        {
            argumentList.Add(new List<Concept>() { subject, verb, complement });
        }

        public override void AddProof(Proof otherProof)
        {
            if (this == otherProof)
            {
                argumentList.Clear();
                throw new TotologyException("Totology detected");
            }
            else if (statementToProove != null && otherProof.ContainsArgument(statementToProove[0], statementToProove[1], statementToProove[2]))
            {
                argumentList.Clear();
                throw new TotologyException("Totology detected");
            }

            foreach (List<Concept> argument in otherProof.argumentList)
                this.AddArgument(argument[0], argument[1], argument[2]);
        }

        public override IEnumerator<List<Concept>> GetEnumerator()
        {
            return argumentList.GetEnumerator();
        }

        public override bool Equals(AbstractProof otherAbstractProof)
        {
            Proof otherProof = (Proof)(otherAbstractProof);

            if (this.argumentList.Count != otherProof.argumentList.Count)
                return false;

            int counter = 0;
            foreach (List<Concept> currentArgument in this.argumentList)
            {
                if (otherProof.argumentList[counter][0] != currentArgument[0])
                    return false;
                else if (otherProof.argumentList[counter][1] != currentArgument[1])
                    return false;
                else if (otherProof.argumentList[counter][2] != currentArgument[2])
                    return false;

                counter++;
            }

            return true;
        }

        public override bool ContainsArgument(Concept subject, Concept verb, Concept complement)
        {
            foreach (List<Concept> argument in argumentList)
                if (argument[0] == subject && argument[1] == verb && argument[2] == complement)
                    return true;
            return false;
        }

        public override Concept GetLastWord()
        {
            return argumentList[argumentList.Count - 1][2];
        }

        public override Concept GetFirstWord()
        {
            return argumentList[0][0];
        }

        public override bool ContainsDuplicateArgument()
        {
            HashSet<string> ignoreList = new HashSet<string>();
            string currentHashCode;
            foreach (List<Concept> argument in argumentList)
            {
                currentHashCode = argument[0].GetHashCode() + "-" + argument[1].GetHashCode() + "-" + argument[2].GetHashCode() + "|" + argument[0].DebuggerName + "|" + argument[1].DebuggerName + "|" + argument[2].DebuggerName;
                if (ignoreList.Contains(currentHashCode))
                    return true;

                ignoreList.Add(currentHashCode);
            }
            return false;
        }
        #endregion

        #region Properties
        public override int Count
        {
            get { return argumentList.Count; }
        }
        #endregion
    }
}
