using System.Collections.Generic;

namespace FlooringMastery.Models.Interfaces
{
    public interface IStateTaxRepository
    {
        StateTax GetStateTax(string stateName);
        List<StateTax> List();
    }
}
