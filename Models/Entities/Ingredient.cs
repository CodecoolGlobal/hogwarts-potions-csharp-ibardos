using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HogwartsPotions.Models.Entities;

public class Ingredient
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; }

    // Navigation properties to establish connection to other tables in SQL DB
    [JsonIgnore]
    public HashSet<Potion> Potions { get; set; }
    [JsonIgnore]
    public HashSet<Recipe> Recipes { get; set; }

    public Ingredient(string name)
    {
        Name = name;
    }
}