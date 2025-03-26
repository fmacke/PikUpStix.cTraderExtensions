using Application.Business.Positioning.Instructions;
using System.Reflection;

namespace Application.Business.Positioning.Validation
{
    public interface IValidationService
    {
        void Validate<TInstruction>(TInstruction instruction) where TInstruction : PositionInstruction;
    }

    
    public class ValidationService : IValidationService
    {
        public void Validate<TInstruction>(TInstruction instruction) where TInstruction : PositionInstruction
        {
            var type = typeof(TInstruction);
            foreach (var property in type.GetProperties())
            {
                var attributes = property.GetCustomAttributes<Attribute>();
                foreach (var attribute in attributes)
                {
                    var validateMethod = attribute.GetType().GetMethod("Validate");
                    if (validateMethod != null)
                    {
                        var parameters = validateMethod.GetParameters();
                        if (parameters.Length == 2)
                        {
                            validateMethod.Invoke(attribute, new object[] { property.GetValue(instruction), instruction.Position });
                        }
                        else if (parameters.Length == 1)
                        {
                            validateMethod.Invoke(attribute, new object[] { instruction.Position });
                        }
                    }
                }
            }
        }
    }

}
