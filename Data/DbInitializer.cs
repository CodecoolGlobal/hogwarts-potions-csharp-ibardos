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
        if (
            context.Students.Any() ||
            context.Rooms.Any() ||
            context.Recipes.Any() ||
            context.Potions.Any() ||
            context.Ingredients.Any()
            )
        {
            return;
        }


        // INITIAL DATA ADDITION

        // Data creation (Student, Room, Ingredient, Recipe, Potion)
        // Students
        Student harry = new Student("Harry Potter", HouseType.Gryffindor, PetType.Owl);
        Student hermione = new Student("Hermione Granger", HouseType.Gryffindor, PetType.Cat);
        Student ron = new Student("Ron Weasley", HouseType.Gryffindor, PetType.Rat);
        Student draco = new Student("Draco Malfoy", HouseType.Slytherin, PetType.Owl);
        Student drStrange = new Student("Dr. Stephen Strange", HouseType.Ravenclaw, PetType.None);
        HashSet<Student> students = new HashSet<Student>() { harry, hermione, ron, draco, drStrange };

        // Rooms
        Room room1 = new Room(5);
        Room room2 = new Room(5);
        Room room3 = new Room(5);
        Room room4 = new Room(5);
        Room room5 = new Room(5);
        HashSet<Room> rooms = new HashSet<Room>() { room1, room2, room3, room4, room5 };

        // Ingredients
        Ingredient agrippa = new Ingredient("Agrippa");
        Ingredient batWing = new Ingredient("Bat wing");
        Ingredient eyeball = new Ingredient("Eyeball");
        Ingredient frogBrain = new Ingredient("Frog brain");
        Ingredient vinegar = new Ingredient("Vinegar");
        HashSet<Ingredient> ingredients = new HashSet<Ingredient>()
            { agrippa, batWing, eyeball, frogBrain, vinegar };

        // Ingredient collection for Recipes
        HashSet<Ingredient> ingredientsForDrStrange1 = new HashSet<Ingredient>
            { agrippa, batWing, eyeball, frogBrain, vinegar };

        // Recipes
        Recipe drStrange1 = new Recipe("Dr. Stephen Strange's discovery #1", drStrange, ingredientsForDrStrange1);

        // Potions
        Potion drStrangesPotion = new Potion("Dr. Stephen Strange's potion #1", drStrange, BrewingStatus.Discovery, drStrange1);


        // Assignment (Student-Room / Room-Residents)
        // Student-Room
        harry.ChooseRoom(room1);
        hermione.ChooseRoom(room1);
        ron.ChooseRoom(room1);
        draco.ChooseRoom(room2);
        drStrange.ChooseRoom(room3);

        // Room-Residents
        room1.AddResidents(new HashSet<Student> { harry, hermione, ron });
        room2.AddResident(draco);
        room3.AddResident(drStrange);

        // Ingredient-Potion
        drStrangesPotion.AddIngredients(ingredientsForDrStrange1);


        // Add to SQL database
        // Add Students
        context.Students.AddRange(students);

        // Add Rooms
        context.Rooms.AddRange(rooms);

        // Add Ingredients
        context.Ingredients.AddRange(ingredients);

        // Add Recipes
        context.Recipes.Add(drStrange1);

        // Add Potions
        context.Potions.Add(drStrangesPotion);
        
        context.SaveChanges();
    }
}