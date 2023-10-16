using System;
using Newtonsoft.Json;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    public class BaseEntity
    {
        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long CreatedBy { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [SetValueOnUpdate((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime UpdatedDate { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        [SetValueOnUpdate((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long UpdatedBy { get; set; }

        [SetIp]
        public string SystemID { get; set; }
    }
}