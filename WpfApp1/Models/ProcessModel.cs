using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class ProcessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountHandle { get; set; }
        public int ThreadHandle { get; set; }
        public int? SessionId{ get; set; }

    }
}
