using System.Collections.Generic;
using System.Linq;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Data;

public static class DbInitializer
{
    public static void Initialize(HogwartsContext context)
    {
        context.Database.EnsureCreated();

        // Check if SQL tables are exist, if so, no action
        if (context.Students.Any() || context.Rooms.Any())
        {
            return;
        }
        
        // Initial data addition
        
        // Data creation (Student/Room)
        // Students
        Student harry = new Student("Harry Potter", HouseType.Gryffindor, PetType.Owl);
        Student hermione = new Student("Hermione Granger", HouseType.Gryffindor, PetType.Cat);
        Student ron = new Student("Ron Weasley", HouseType.Gryffindor, PetType.Rat);
        Student draco = new Student("Draco Malfoy", HouseType.Slytherin, PetType.Owl);
        HashSet <Student> students = new HashSet<Student>() {harry, hermione, ron, draco};

        // Rooms
        Room room1 = new Room(5);
        Room room2 = new Room(5);    
        Room room3 = new Room(5);
        Room room4 = new Room(5);
        Room room5 = new Room(5);
        HashSet<Room> rooms = new HashSet<Room>(){room1, room2, room3, room4, room5};

        // Assignments (Student-Room / Room-Residents)
        // Student-Room
        harry.ChooseRoom(room1);
        hermione.ChooseRoom(room1);
        ron.ChooseRoom(room1);
        draco.ChooseRoom(room2);
        
        // Room-Residents
        room1.AddResidents(new HashSet<Student> {harry, hermione, ron});
        room2.AddResident(draco);
        
        // Add to SQL database
        // Add Students
        context.Students.AddRange(students);
        context.SaveChanges();
        
        // Add Rooms
        context.Rooms.AddRange(rooms);
        context.SaveChanges();
    }
}