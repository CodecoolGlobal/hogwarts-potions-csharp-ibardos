using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Models.Entities
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HouseType HouseType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PetType PetType { get; set; }

        public Room Room { get; set; }


        public Student(string name, HouseType houseType, PetType petType)
        {
            Name = name;
            HouseType = houseType;
            PetType = petType;
        }

        public void ChooseRoom(Room room)
        {
            Room = room;
        }
    }
}