using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HogwartsPotions.Models.Entities
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public int Capacity { get; set; }

        public HashSet<Student> Residents { get; set; } = new HashSet<Student>();

        public Room(int capacity)
        {
            Capacity = capacity;
        }

        /// <summary>
        /// Adds one resident into room.
        /// </summary>
        /// <param name="resident"></param>
        public void AddResident(Student resident)
        {
            Residents.Add(resident);
        }

        /// <summary>
        /// Adds all of the residents from a Hashset into room.
        /// </summary>
        /// <param name="residents"></param>
        public void AddResidents(HashSet<Student> residents)
        {
            foreach (Student resident in residents)
            {
                AddResident(resident);
            }
        }
    }
}
