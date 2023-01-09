using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Core.Models
{
    public class Restaurant : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// THE path to image showing how the tables are setted in the building
        /// </summary>
        public string AddressOfSchemaImage { get; set; }

        public List<Table> Tables { get; set; } = new List<Table>();
    }
}
