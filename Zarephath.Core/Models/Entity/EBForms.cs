using ExpressiveAnnotations.Attributes;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EBForms")]
    [PrimaryKey("ID")]
    [Sort("FormID", "DESC")]
    public class EBForms : BaseEntity
    {
        public string EBFormID { get; set; }
        public string FromUniqueID { get; set; }
        public string Id { get; set; }

        //[Required(ErrorMessage = "Please select file.")]
        // [RequiredIf("NewPdfURI == null", ErrorMessageResourceName = "Please select file.", ErrorMessageResourceType = typeof(Resource))]
        // [RequiredIf("NewPdfURI", "NULL")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf|.PDF)$", ErrorMessage = "Only PDF files allowed.")]
        public HttpPostedFileBase File { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FormId { get; set; }
        [Required(ErrorMessageResourceName = "FormNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }
        public string FormLongName { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public bool IsActive { get; set; }
        public bool HasHtml { get; set; }
        public string NewHtmlURI { get; set; }
        public bool HasPDF { get; set; }
        public string NewPdfURI { get; set; }
        [Required(ErrorMessageResourceName = "SelectCategory", ErrorMessageResourceType = typeof(Resource))]
        
        public string EBCategoryID { get; set; }
        [Required(ErrorMessageResourceName = "SelectMarket", ErrorMessageResourceType = typeof(Resource))]
        public string EbMarketIDs { get; set; }
        //[Required(ErrorMessageResourceName = "PriceRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessageResourceName = "DecimalValue", ErrorMessageResourceType = typeof(Resource))]
        public string FormPrice { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public bool IsOrbeonForm { get; set; }
    }



    //public class RequiredIfAttribute : ValidationAttribute
    //{
    //    RequiredAttribute _innerAttribute = new RequiredAttribute();
    //    public string _dependentProperty { get; set; }
    //    public object _targetValue { get; set; }

    //    public RequiredIfAttribute(string dependentProperty, object targetValue)
    //    {
    //        this._dependentProperty = dependentProperty;
    //        this._targetValue = targetValue;
    //    }
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var field = validationContext.ObjectType.GetProperty(_dependentProperty);
    //        if (field != null)
    //        {
    //            var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
    //            if ((dependentValue == null && _targetValue == null) || (dependentValue.Equals(_targetValue)))
    //            {
    //                if (!_innerAttribute.IsValid(value))
    //                {
    //                    string name = validationContext.DisplayName;
    //                    return new ValidationResult(ErrorMessage = name + " Is required.");
    //                }
    //            }
    //            return ValidationResult.Success;
    //        }
    //        else
    //        {
    //            return new ValidationResult(FormatErrorMessage(_dependentProperty));
    //        }
    //    }
    //}
     
}

