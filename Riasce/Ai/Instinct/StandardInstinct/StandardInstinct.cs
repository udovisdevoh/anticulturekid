﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCulture.Kid
{
    class StandardInstinct : AbstractInstinct
    {
        #region Fields
        private List<string> statementList = new List<string>();
        #endregion

        #region Constructors
        public StandardInstinct()
        {
            //We link the default operators together
            Add("pine isa tree = tree someare pine");
            Add("tree madeof wood = wood partof tree");
            Add("life need water = water allow life");
            Add("mother make baby = baby madeby mother");
            Add("human was ape = ape become human");
            Add("black contradict white = white contradict black");
            Add("pollution oppress earth = earth oppressedby pollution");
            Add("indian synergize nature = nature synergize indian");
            Add("muslim antagonize jew = jew antagonize muslim");
            Add("grunge from seattle = seattle originof grunge");
            Add("earth largerthan moon = moon smallerthan earth");
            Add("pope own catholicism = catholicism ownedby pope");
            Add("vertebrate without exoskeleton = exoskeleton notpartof vertebrate");

            //IsA AND SomeAre
            Add("someare conservative_thinking someare");
            Add("IF pine madeof wood THEN wood UNLIKELY isa pine");
            Add("IF tree madeof wood THEN tree UNLIKELY isa wood");
            Add("IF animal contradict plant THEN animal CANT isa plant");
            Add("IF pine isa tree THEN tree CANT someare pine");
            Add("IF pine isa tree AND tree isa plant THEN pine isa plant");
            //Add("IF homo_ergaster isa hominid THEN homo_ergaster was hominid"); //not sure if it's useful

            //MadeOf AND PartOf
            Add("IF tree madeof wood THEN tree UNLIKELY partof wood");//Used to be "cant"
            Add("IF pine isa tree THEN pine UNLIKELY madeof tree");//Used to be "cant"
            Add("IF tree someare pine THEN tree UNLIKELY madeof pine");//Used to be "cant"
            Add("IF pine madeof wood AND wood madeof carbon THEN pine madeof carbon");
            Add("IF pine isa tree AND tree madeof wood THEN pine madeof wood");
            Add("IF pine madeof wood AND wood isa material THEN pine madeof material");
            Add("IF big_mac madeof trans_fat AND trans_fat oppress lifeform THEN big_mac oppress lifeform");

            //Need AND Allow
            Add("IF bird need tree AND tree need light THEN bird need light");
            Add("IF bird need tree AND tree madeof wood THEN bird need wood");
            Add("IF bird need tree AND tree isa lifeform THEN bird need lifeform");
            Add("IF crow isa bird AND bird need tree THEN crow need tree");
            Add("IF lifeform madeof cell AND cell need water THEN lifeform need water");

            //Make AND MadeBy
            Add("IF daughter madeby mother THEN daughter CANT make mother");
            Add("IF grandmother make mother AND mother make daughter THEN grandmother make daughter");
            Add("IF mozart make music AND music isa art THEN mozart make art");
            Add("IF mom isa mother AND mother make child THEN mom make child");
            //Add("IF mcdonalds make big_mac AND big_mac madeof trans_fat THEN mcdonalds make trans_fat"); //Should be replaced to mcdonalds promote trans_fat
            //Add("IF sergy make google AND google originof gmail THEN sergy make gmail");//Not sure at all

            //Oppress and OppressedBy
            Add("IF pollution oppress human AND me isa human THEN pollution oppress me");
            Add("IF acidrain isa pollution AND pollution oppress earth THEN acidrain oppress earth");
            Add("IF pollution oppress earth AND me need earth THEN pollution oppress me");
            Add("IF car make pollution AND pollution oppress me THEN car oppress me");
            //Add("IF earth madeof nature AND gm oppress nature THEN gm oppress earth");
            Add("IF pollution oppress nature THEN pollution unlikely allow nature");
            //Add("IF dogma oppress critical_thinking AND critical_thinking synergize intelligence THEN dogma oppress intelligence"); //Create some inconsistency bug... must be fixed

            //Was AND Become
            Add("IF human madeof flesh THEN human UNLIKELY was flesh");
            Add("IF human madeof flesh THEN human UNLIKELY become flesh");
            Add("IF child become adult THEN child CANT was adult");
            Add("IF adult was child AND child was baby THEN adult was baby");
            Add("IF bird was egg AND egg isa sphere THEN bird was sphere");
            //Add("IF human become ape AND ape isa primate THEN human become primate");

            //Contradict
            Add("IF human isa animal THEN human CANT contradict animal");
            Add("IF human isa animal AND animal contradict plant THEN human contradict plant");

            //Synergize
            Add("IF me synergize nature THEN me cant oppress nature");
            Add("IF me isa indian AND indian synergize nature THEN me synergize nature");

            //Antagonize
            Add("IF me isa jew AND jew antagonize muslim THEN me antagonize muslim");
            Add("IF me antagonize jew THEN me unlikely isa jew");
            Add("IF me antagonize jew THEN me unlikely allow jew");
            Add("IF me antagonize jew THEN me cant synergize jew");

            //From and originof
            Add("IF grunge from seattle AND seattle partof usa THEN grunge from usa");
            Add("IF dragon_ball isa manga AND manga from japan THEN dragon_ball from japan");

            //Largerthan and smallerthan
            Add("IF sun largerthan earth AND earth largerthan moon THEN sun largerthan moon");
            //Add("IF sun isa star AND star largerthan planet THEN sun largerthan planet");
            //Add("IF earth isa planet AND planet smallerthan sun THEN earth smallerthan sun");

            //Own and ownedby
            Add("IF catholic isa christian AND christian ownedby christianism THEN catholic ownedby christianism");
            Add("IF dalai_lama own buddhism AND buddhism own buddhist THEN dalai_lama own buddhist");
            Add("IF joe isa hat_wearer AND hat_wearer own hat THEN joe own hat");

            //Without and notpartof
            Add("IF insect madeof exoskeleton THEN insect cant without exoskeleton");
            Add("IF joe without tooth AND tooth someare molar THEN joe without molar");
            Add("IF invertebrate without vertebrate_column AND invertebrate someare arthropod THEN arthropod without vertebrate_column");
            Add("IF joe without hat THEN joe cant own hat");
        }
        #endregion

        #region Methods
        public override IEnumerator<string> GetEnumerator()
        {
            return statementList.GetEnumerator();
        }

        private void Add(string statement)
        {
            statementList.Add(statement.ToLower().Trim());
        }
        #endregion

        #region Properties
        public override int Count
        {
            get { return  statementList.Count; }
        }
        #endregion
    }
}