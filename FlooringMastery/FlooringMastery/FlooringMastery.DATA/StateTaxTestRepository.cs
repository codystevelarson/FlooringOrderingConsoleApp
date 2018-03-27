using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System.Collections.Generic;

namespace FlooringMastery.DATA
{
    public class StateTaxTestRepository : IStateTaxRepository
    {
        private static StateTax _stateTax = new StateTax
        {
            StateAbbreviation = "MN",
            StateName = "Minnesota",
            TaxRate = 7.7m
        };


        public StateTax GetStateTax(string state)
        {
            List<StateTax> states = List();

            foreach(StateTax s in states)
            {
                if(s.StateName == state || s.StateAbbreviation == state)
                {
                    return s;
                }
            }
            return null;
            
        }

        public List<StateTax> List()
        {
            List<StateTax> states = new List<StateTax>();
            states.Add(_stateTax);
            return states;
        }
    }
}
