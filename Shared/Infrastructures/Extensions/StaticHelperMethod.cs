using Newtonsoft.Json;
using Shared.Features;

namespace Shared.Infrastructures
{
    public static class StaticHelperMethod
    {
        #region Banner
        public static List<string> ValidateCreateBannerCommand(CreateBannerCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.Link))
                errors.Add(ErrorMessage("Link", "Link is required", "Ссылка обязательна", "Link majburiy"));

            if (command?.Entity?.Sort <= 0)
                errors.Add(ErrorMessage("Sort", "Sort is required", "Сортировка обязательна", "Tartib majburiy"));

            if (command?.Entity?.PhotoView == null)
                errors.Add(ErrorMessage("Photo", "Photo is required", "Фото обязателен", "Rasm majburiy"));

            return errors;
        }

        public static List<string> ValidateUpdateBannerCommand(UpdateBannerCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.Link))
                errors.Add(ErrorMessage("Link", "Link is required", "Ссылка обязательна", "Link majburiy"));

            if (command?.Entity?.Sort <= 0)
                errors.Add(ErrorMessage("Sort", "Sort is required", "Сортировка обязательна", "Tartib majburiy"));

            if (command?.Entity?.PhotoView == null)
                errors.Add(ErrorMessage("Photo", "Photo is required", "Фото обязателен", "Rasm majburiy"));

            return errors;
        }
        #endregion
        #region Brand
        public static List<string> ValidateCreateBrandCommand(CreateBrandCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.Link))
                errors.Add(ErrorMessage("Link", "Link is required", "Ссылка обязательна", "Link majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.Name))
                errors.Add(ErrorMessage("Name", "Name is required", "Имя обязательна", "Nomi majburiy"));

            if (command?.Entity?.Photo == null)
                errors.Add(ErrorMessage("Photo", "Photo is required", "Фото обязателен", "Rasm majburiy"));

            return errors;
        }

        public static List<string> ValidateUpdateBrandCommand(UpdateBrandCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.Link))
                errors.Add(ErrorMessage("Link", "Link is required", "Ссылка обязательна", "Link majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.Name))
                errors.Add(ErrorMessage("Name", "Name is required", "Имя обязательна", "Nomi majburiy"));

            if (command?.Entity?.Photo == null)
                errors.Add(ErrorMessage("Photo", "Photo is required", "Фото обязателен", "Rasm majburiy"));

            return errors;
        }
        #endregion
        #region Category
        public static List<string> ValidateCreateCategoryCommand(CreateProductCategoryCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.NameUz))
                errors.Add(ErrorMessage("NameUz", "NameUz is required", "NameUz обязательна", "NameUz majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.NameRu))
                errors.Add(ErrorMessage("NameRu", "NameRu is required", "NameRu обязательна", "NameRu majburiy"));

            return errors;
        }

        public static List<string> ValidateUpdateCategoryCommand(UpdateProductCategoryCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.NameUz))
                errors.Add(ErrorMessage("NameUz", "NameUz is required", "NameUz обязательна", "NameUz majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.NameRu))
                errors.Add(ErrorMessage("NameRu", "NameRu is required", "NameRu обязательна", "NameRu majburiy"));

            return errors;
        }
        #endregion

        #region SubCategory
        public static List<string> ValidateCreateSubCategoryCommand(CreateProductSubCategoryCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.NameUz))
                errors.Add(ErrorMessage("NameUz", "NameUz is required", "NameUz обязательна", "NameUz majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.NameRu))
                errors.Add(ErrorMessage("NameRu", "NameRu is required", "NameRu обязательна", "NameRu majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.Href))
                errors.Add(ErrorMessage("Href", "Href is required", "Href обязательна", "Href majburiy"));

            return errors;
        }

        public static List<string> ValidateUpdateSubCategoryCommand(UpdateProductSubCategoryCommand command)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(command?.Entity?.NameUz))
                errors.Add(ErrorMessage("NameUz", "NameUz is required", "NameUz обязательна", "NameUz majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.NameRu))
                errors.Add(ErrorMessage("NameRu", "NameRu is required", "NameRu обязательна", "NameRu majburiy"));

            if (string.IsNullOrEmpty(command?.Entity?.Href))
                errors.Add(ErrorMessage("Href", "Href is required", "Href обязательна", "Href majburiy"));

            return errors;
        }
        #endregion

        #region Desirialize Object
        public static string ErrorMessage(string key, string msg_en, string msg_ru, string msg_uz)
        {
            var errorDict = new Dictionary<string, string>
            {
                ["key"] = key,
                ["msg_en"] = msg_en,
                ["msg_ru"] = msg_ru,
                ["msg_uz"] = msg_uz
            };

            return JsonConvert.SerializeObject(errorDict);
        }
        public static Dictionary<string, string> DeserializeError(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        #endregion
    }
}
