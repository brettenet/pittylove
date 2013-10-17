using System;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;

namespace PittyLove.Model.Validators
{
    public class PitbullValidator : AbstractValidator<Pitbull>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PitbullValidator"/> class.
        /// </summary>
        public PitbullValidator()
        {
            //Pitbulls owners should give their dogs nice names!
            RuleFor(item => item.Name)
                .Must(BeANiceName)
                .WithMessage("Please supply a nice name like 'Buddy'");
        }

        /// <summary>
        /// Determines whether [is name OK] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [is name OK] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        private bool BeANiceName(string name)
        {
            return !name.Equals("Chompy", StringComparison.OrdinalIgnoreCase)
                   && !name.Equals("Killer", StringComparison.OrdinalIgnoreCase)
                   && !name.Equals("Manson", StringComparison.OrdinalIgnoreCase)
                   && !name.Equals("Ripper", StringComparison.OrdinalIgnoreCase);
        }
    }
}
