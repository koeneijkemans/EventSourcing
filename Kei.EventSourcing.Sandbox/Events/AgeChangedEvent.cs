using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kei.EventSourcing.Sandbox.Events
{
    internal class AgeChangedEvent : Event
    {
        public int Age { get; set; }
    }
}
