using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace FlooringMastery.DATA
{
    public class StateTaxFileRepository : IStateTaxRepository
    {
        private string _filePath;

        public StateTaxFileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public StateTax GetStateTax(string stateName)
        {
            List<StateTax> taxes = List();

            foreach (StateTax t in taxes)
            {
                if (t.StateAbbreviation == stateName || t.StateName == stateName)
                {
                    return t;
                }
            }
            return null;
        }

        public List<StateTax> List()
        {
            List<StateTax> taxes = new List<StateTax>();
            using (StreamReader sr = new StreamReader(_filePath))
            {
                //Skip header
                sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    StateTax stateTax = new StateTax();
                    string[] columns = line.Split(',');

                    stateTax.StateAbbreviation = columns[0];
                    stateTax.StateName = columns[1];
                    stateTax.TaxRate = decimal.Parse(columns[2]);

                    taxes.Add(stateTax);
                }
            }
            return taxes;
        }
    }
}
