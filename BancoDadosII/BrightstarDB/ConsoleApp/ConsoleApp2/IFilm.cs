using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightstarDB.EntityFramework;

namespace ConsoleApp2
{
    [Entity("Film")]
    public interface IFilm
    {

        /// <summary>
        /// Get the persistent identifier for this entity
        /// </summary>
        string Id { get; }

        string Name { get; set; }

        [InverseProperty("Films")]
        ICollection<IActor> Actors { get; }
    }
}
