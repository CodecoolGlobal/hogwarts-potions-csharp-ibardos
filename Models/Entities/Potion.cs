using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Models.Entities;

public class Potion
{
    public const int MaxIngredientsForPotions = 5;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string Name { get; set; }
    public Student Student { get; set; }
    public HashSet<Ingredient> Ingredients { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BrewingStatus BrewingStatus { get; set; }

    public Recipe Recipe { get; set; }

}