using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HogwartsPotions.Models.Entities;

public class Recipe
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string Name { get; set; }
    public Student Student { get; set; }
    public HashSet<Ingredient> Ingredients { get; set; }


    public Recipe(string name, Student student, HashSet<Ingredient> ingredients)
    {
        Name = name;
        Student = student;
        Ingredients = ingredients;
    }

    // Parameterless constructor to work with EF Core
    public Recipe()
    {
    }
}