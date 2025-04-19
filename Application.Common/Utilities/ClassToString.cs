using System.Text;

namespace Application.Common.Utilities
{
    public static class ClassToString
    {
        public static string FormatProperties(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var properties = obj.GetType().GetProperties();
            var stringBuilder = new StringBuilder();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(obj);
                stringBuilder.AppendLine($"{propertyName}: {propertyValue}");
            }

            return stringBuilder.ToString();
        }
    }
}
