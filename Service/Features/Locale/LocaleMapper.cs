using Riok.Mapperly.Abstractions;
using Shared;
using System.Text.Json;

namespace UtcNew.Services;

[Mapper]
public static partial class LocaleMapper 
{
    #region Usable
    public static LocaleView MapToView(this LocaleEntity src) => src.To();
    public static List<LocaleView> MapToViewList(this List<LocaleEntity> src)=> src.ToList();
    public static LocaleEntity MapFromView(this LocaleView src) => src.From();
    #endregion

    #region Internal

    [MapProperty("ProductEntity", "ProductView")]
    private static partial LocaleView To(this LocaleEntity src);
    [MapProperty("ProductEntity", "ProductView")]
    private static partial List<LocaleView> ToList(this List<LocaleEntity> src);
    [MapProperty("ProductView", "ProductEntity")]
    private static partial LocaleEntity From(this LocaleView LocaleView);
    [MapProperty("ProductView", "ProductEntity")]
    public static partial void From(LocaleView personView, LocaleEntity personEntity);
    
    #endregion
}