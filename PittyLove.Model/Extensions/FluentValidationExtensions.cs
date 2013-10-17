using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using FluentValidation.Resources;

namespace PittyLove.Model.Extensions
{
    /// <summary>
    /// Extension to the Fluent Validation Types
    /// </summary>
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Converts the Fluent Validation result to the type the entity framework expects
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> ToValidationResult(
            this FluentValidation.Results.ValidationResult validationResult)
        {
            return validationResult.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
