using System.Collections.Generic;
using BrightstarDB.EntityFramework;

namespace ConsoleApp2.Entidades
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
