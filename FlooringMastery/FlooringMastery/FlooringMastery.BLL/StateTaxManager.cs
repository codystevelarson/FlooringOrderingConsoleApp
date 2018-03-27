using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class StateTaxManager
    {
        private IStateTaxRepository _stateTaxRepository;

        public StateTaxManager(IStateTaxRepository stateTaxRepository)
        {
            _stateTaxRepository = stateTaxRepository;
        }

        public StateLookupResponse GetStateTax(string state)
        {
            StateLookupResponse response = new StateLookupResponse();
            if (state == "")
            {
                response.Success = false;
                response.Message = "";
                return response;
            }
            response.StateTax = _stateTaxRepository.GetStateTax(state);
            if (response.StateTax == null)
            {
                response.Success = false;
                response.Message = $"State tax info does not exist for {state}";
                return response;
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
    }
}
