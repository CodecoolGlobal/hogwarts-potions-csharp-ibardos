using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
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


    public Potion(string name, Student student, BrewingStatus brewingStatus, Recipe recipe)
    {
        Name = name;
        Student = student;
        Ingredients = new HashSet<Ingredient>();
        BrewingStatus = brewingStatus;
        Recipe = recipe;
    }

    // Parameterless constructor to work with EF Core
    public Potion()
    {
    }


    /// <summary>
    /// Adds ingredients into Potion, up to the maximum number of 5 pieces
    /// </summary>
    /// <param name="ingredients"></param>
    public void AddIngredients(IEnumerable<Ingredient> ingredients)
    {
        int remainingIngredientsAbleToAdd = MaxIngredientsForPotions - Ingredients.Count;

        List<Ingredient> ingredientsToAdd = ingredients.ToList();

        for (int i = 0; i < remainingIngredientsAbleToAdd; i++)
        {
            Ingredients.Add(ingredientsToAdd[i]);
        }
    }
}