using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Helpers
{
    class CompareObject
    {
        public AuditTrailModel GetAuditTrailModel<T>(AuditActionType Action, long parentKeyFieldId, long? childKeyFieldId, T OldObject, T NewObject)
        {
            // get the differance
            CompareLogic compObjects = new CompareLogic();
            compObjects.Config.MaxDifferences = 99;
            ComparisonResult compResult = compObjects.Compare(OldObject, NewObject);
            List<AuditDelta> DeltaList = new List<AuditDelta>();
            foreach (var change in compResult.Differences)
            {
                AuditDelta delta = new AuditDelta();
                if (change.PropertyName.Substring(0, 1) == ".")
                    delta.FieldName = change.PropertyName.Substring(1, change.PropertyName.Length - 1);


                PropertyInfo prop = NewObject.GetType().GetProperty(delta.FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);



                var includeInAuditAttribute = prop.GetCustomAttribute(typeof(IncludeInAuditAttribute), false) as IncludeInAuditAttribute;
                if (includeInAuditAttribute == null)
                {

                    var ignoreAttribute = prop.GetCustomAttribute(typeof (IgnoreAttribute), false) as IgnoreAttribute;
                    if (ignoreAttribute != null) continue;

                    var resultColumnAttribute =
                        prop.GetCustomAttribute(typeof (ResultColumnAttribute), false) as ResultColumnAttribute;
                    if (resultColumnAttribute != null) continue;

                    var setValueOnAddAttribute =
                        prop.GetCustomAttribute(typeof (SetValueOnAddAttribute), false) as SetValueOnAddAttribute;
                    if (setValueOnAddAttribute != null) continue;

                    var setIpAttribute = prop.GetCustomAttribute(typeof (SetIpAttribute), false) as SetIpAttribute;
                    if (setIpAttribute != null) continue;

                    var ignoreInAuditAttribute =
                        prop.GetCustomAttribute(typeof (IgnoreInAuditAttribute), false) as IgnoreInAuditAttribute;
                    if (ignoreInAuditAttribute != null) continue;
                }



                var displayAttributes = prop.GetCustomAttribute(typeof(DisplayAttribute), false) as DisplayAttribute;
                if (displayAttributes != null)
                {
                    var resource = new ResourceManager(displayAttributes.ResourceType);
                    delta.FieldName = resource.GetString(displayAttributes.Name);
                }

                var tableNameAttributes = prop.GetCustomAttribute(typeof(GetTableNameAttribute), false) as GetTableNameAttribute;
                if (tableNameAttributes != null) delta.TableName = tableNameAttributes.TableName;


                //object[] attrs = prop.GetCustomAttributes(true);
                //foreach (object attr in attrs)
                //{

                //    DisplayAttribute authAttr = attr as DisplayAttribute;
                //    if (authAttr != null)
                //    {
                //        string propName = prop.Name;
                //        string auth = authAttr.Name;

                //    }
                //}



                


                delta.ValueBefore = change.Object1Value;
                delta.ValueAfter = change.Object2Value;


                var auditMakeBeforeAfterAttribute = prop.GetCustomAttribute(typeof(AuditMakeBeforeAfterAttribute), false) as AuditMakeBeforeAfterAttribute;
                if (auditMakeBeforeAfterAttribute != null) delta.ValueBefore = change.Object2Value;

                var auditMakeAfterBeforeAttribute = prop.GetCustomAttribute(typeof(AuditMakeAfterBeforeAttribute), false) as AuditMakeAfterBeforeAttribute;
                if (auditMakeAfterBeforeAttribute != null) delta.TableName = change.Object1Value;


                DeltaList.Add(delta);
            }

            if (DeltaList.Count == 0)
                return null;

            AuditTrailModel audit = new AuditTrailModel();
            audit.AuditActionType = Enum.GetName(typeof (AuditActionType), Action);// GetDisplayValue(Action);
            audit.DataModel = NewObject.GetType().Name;
            audit.DateTimeStamp = DateTime.UtcNow;
            audit.ParentKeyFieldID = parentKeyFieldId;
            audit.ChildKeyFieldID = childKeyFieldId;
            audit.ValueBefore = JsonConvert.SerializeObject(OldObject); // if use xml instead of json, can use xml annotation to describe field names etc better
            audit.ValueAfter = JsonConvert.SerializeObject(NewObject);
            audit.Changes = JsonConvert.SerializeObject(DeltaList);

            return audit;
        }


        public string GetDisplayValue<T>(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            // return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
            var resource = new ResourceManager(descriptionAttributes[0].ResourceType);
            string displayName = resource.GetString(descriptionAttributes[0].Name);

            return string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", descriptionAttributes[0].Name)
                : displayName;
        }
    }




    public class AuditDelta
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
    }

    public enum AuditActionType
    {
        //[Display(ResourceType = typeof(Resource), Name = "Create")]
        Create = 1,
        //[Display(ResourceType = typeof(Resource), Name = "Update")]
        Update,
        //[Display(ResourceType = typeof(Resource), Name = "Delete")]
        Delete
    }

    public class AuditTrailModel
    {
        public int ID { get; set; }
        public long ParentKeyFieldID { get; set; }
        public long? ChildKeyFieldID { get; set; }
        public System.DateTime DateTimeStamp { get; set; }
        public string DataModel { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
        public string Changes { get; set; }
        public string AuditActionType { get; set; }
    }


    /////////TABLE SCRIPT///////////////////////////
    // CREATE TABLE [dbo].[AuditTable](
    // [ID] [int] IDENTITY(1,1) NOT NULL,
    // [KeyFieldID] [int] NOT NULL,
    // [AuditActionTypeENUM] [int] NOT NULL,
    // [DateTimeStamp] [datetime] NOT NULL,
    // [DataModel] [nvarchar](100) NOT NULL,
    // [Changes] [nvarchar](max) NOT NULL,
    // [ValueBefore] [nvarchar](max) NOT NULL,
    // [ValueAfter] [nvarchar](max) NOT NULL,
    //      CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
    //     (
    //         [ID] ASC
    //     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    //     ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

}
