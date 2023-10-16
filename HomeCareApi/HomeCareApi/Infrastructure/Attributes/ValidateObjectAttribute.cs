using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HomeCareApi.Models.ApiModel;

namespace HomeCareApi.Infrastructure.Attributes
{
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var errorList = new List<ErrorList>();
                foreach (ValidationResult validationResult in results)
                {
                    var validationError = new ErrorList();

                    List<string> list = validationResult.ErrorMessage.Split('|').ToList();
                    if (list.Count > 0)
                    {
                        validationError.Field = validationResult.MemberNames.FirstOrDefault();
                        ////validationError.ErrorCode = list[0];
                        validationError.Message = list[0];
                        errorList.Add(validationError);
                    }
                }

                var compositeResults = new CompositeValidationResult(string.Join(",", errorList.Select(m => m.Field + ": " + m.Message)));
                results.ForEach(compositeResults.AddResult);
                return compositeResults;

                //var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
                //results.ForEach(compositeResults.AddResult);
                //return compositeResults;

                //for (int i = 0; i < results.Count;)
                //{
                //    var compositeResults1 = new CompositeValidationResult(results[i].ErrorMessage);
                //    results.Add(compositeResults1);
                //}
            }

            return ValidationResult.Success;
        }
    }

    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        public CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }

        public void AddResult(List<ValidationResult> validationResult)
        {
            _results.AddRange(validationResult);
        }
    }
}