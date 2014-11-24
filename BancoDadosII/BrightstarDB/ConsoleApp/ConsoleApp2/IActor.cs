using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightstarDB.EntityFramework;

namespace ConsoleApp2
{
    [Entity]
    public interface IActor
    {
        /// <summary>
        /// Get the persistent identifier for this entity
        /// </summary>
        string Id { get; }

        string Name { get; set; }
        DateTime DateOfBirth { get; set; }
        ICollection<IFilm> Films { get; set; }

        void GetOlder(int years);
    }
}
