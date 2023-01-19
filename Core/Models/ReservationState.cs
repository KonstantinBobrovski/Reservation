using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public enum ReservationState
    {
        //default state
        Sended,
        //Canceled by client or restaurant
        Canceled,
        //Restaurant agreed to take the client
        Approved,
        //Restaurant set it to know that user came
        EndedSuccessful,
        //Restaurant set that client did not arrived
        EndedFail

    }
}
