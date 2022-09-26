using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HogwartsPotions.Models.Entities;

public class Ingredient
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string Name { get; set; }

    // Navigation properties to establish connection to other tables in SQL DB
    public HashSet<Potion> Potions { get; set; }
    public HashSet<Recipe> Recipes { get; set; }

    public Ingredient(string name)
    {
        Name = name;
    }
}