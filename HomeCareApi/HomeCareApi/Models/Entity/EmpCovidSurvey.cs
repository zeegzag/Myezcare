using System;
using System.ComponentModel.DataAnnotations;
using HomeCareApi.Infrastructure.Attributes;
using HomeCareApi.Resources;
using PetaPoco;
namespace HomeCareApi.Models.Entity
{
    [TableName("EmpCovidSurvey")]
    [PrimaryKey("CovidSurveyID")]
    [Sort("CovidSurveyID", "DESC")]
    public class EmpCovidSurvey
    {
        public long CovidSurveyID { get; set; }
        public long EmployeeID { get; set; }
        public long QuestionID { get; set; }
        public long AnswersID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
    }
}