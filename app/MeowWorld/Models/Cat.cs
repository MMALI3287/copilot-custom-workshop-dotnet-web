using System.ComponentModel.DataAnnotations;

namespace MeowWorld.Models;

/// <summary>
/// 猫のエンティティ
/// </summary>
public class Cat
{
    /// <summary>識別子</summary>
    public int Id { get; set; }

    /// <summary>猫の名前</summary>
    [Required(ErrorMessage = "Validation_Name_Required")]
    [MaxLength(50, ErrorMessage = "Validation_Name_MaxLength")]
    [Display(Name = "Col_Name")]
    public required string Name { get; set; }

    /// <summary>年齢</summary>
    [Range(0, 30, ErrorMessage = "Validation_Age_Range")]
    [Display(Name = "Col_Age")]
    public int Age { get; set; }

    /// <summary>品種</summary>
    [Required(ErrorMessage = "Validation_Breed_Required")]
    [MaxLength(50, ErrorMessage = "Validation_Breed_MaxLength")]
    [Display(Name = "Col_Breed")]
    public required string Breed { get; set; }

    /// <summary>説明（任意）</summary>
    [MaxLength(500, ErrorMessage = "Validation_Description_MaxLength")]
    [Display(Name = "Col_Description")]
    public string? Description { get; set; }

    /// <summary>お気に入り状態</summary>
    [Display(Name = "Col_IsFavorite")]
    public bool IsFavorite { get; set; }

    /// <summary>登録日時</summary>
    [Display(Name = "Col_CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
